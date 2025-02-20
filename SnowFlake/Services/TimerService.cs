using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver.Core.Connections;
using SnowFlake.Hubs;
using SnowFlake.Services;
using SnowFlake.Utilities;
using System.Collections.Concurrent;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

public class TimerService : ITimerService
{
    private readonly IHubContext<TimerHub> _hubContext;
    private readonly ILogger<TimerService> _logger;
    private static readonly ConcurrentDictionary<string, TimerState> _timers = new();
    private static readonly ConcurrentDictionary<string, HashSet<string>> _groupConnections = new();

    public TimerService(IHubContext<TimerHub> hubContext,
                        ILogger<TimerService> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task AddClientToGroup(string groupName, string connectionId)
    {
        try
        {
            await _hubContext.Groups.AddToGroupAsync(connectionId, groupName);

            _groupConnections.AddOrUpdate(
                groupName,
                new HashSet<string> { connectionId },
                (_, connections) =>
                {
                    connections.Add(connectionId);
                    return connections;
                });

            await _hubContext.Clients.Group(groupName).SendAsync("JoinUserGroup", $"{connectionId} joined the '{groupName}' group.");
            await AddLog(groupName, $"Client joined the group.");
        }
        catch (Exception e)
        {
            await AddLog(groupName, $"Error occur while adding client to group: {e}");
            await _hubContext.Clients.Group(groupName).SendAsync("JoinUserGroup",
                $"Error occur while adding client to group ({groupName}): {e}");
        }
    }

    public async Task RemoveClientFromGroup(string groupName, string connectionId)
    {
        try
        {
            await _hubContext.Groups.RemoveFromGroupAsync(connectionId, groupName);

            if (_groupConnections.TryGetValue(groupName, out var connections))
            {
                connections.Remove(connectionId);
                if (connections.Count == 0)
                {
                    // If no more connections in group, cleanup timer
                    await StopCountdown(groupName);
                    _groupConnections.TryRemove(groupName, out _);
                }
            }

            await _hubContext.Clients.Group(groupName).SendAsync("LeaveUserGroup", $"{connectionId} leaved the '{groupName}' group.");
            await AddLog(groupName, $"Client leave from the group.");
        }
        catch (Exception e)
        {
            await AddLog(groupName, $"Error occur while adding client to group: {e}");
            await _hubContext.Clients.Group(groupName).SendAsync("LeaveUserGroup",
                $"Error occur while removing client from group ({groupName}): {connectionId}");
        }
    }

    public async Task CreateCountdown(string groupName, string duration, string gameState)
    {
        try
        {
            var seconds = Utils.ConvertToSeconds(duration);
            if (_timers.ContainsKey(groupName))
            {
                StopCountdown(groupName);
            }

            _timers[groupName] = new TimerState
            {
                RemainingSeconds = seconds + 1,
                TotalSeconds = seconds,
                GameState = GameState.FromValue(gameState)
            };

            await _hubContext.Clients.Group(groupName).SendAsync("CreateTimer", gameState);
            await AddLog(groupName, $"Created a new countdown with {duration} seconds.");
        }
        catch (Exception e)
        {
            _logger.LogError($"Error occur while creating countdown: {e}");
            await _hubContext.Clients.Group(groupName).SendAsync("CreateTimer",
                $"Error occur while creating countdown: {e}");
        }
        

    }

    public async Task StartCountdown(string groupName)
    {
        try
        {
            _timers[groupName].Status = TimerStatus.Running;

            _hubContext.Clients.Group(groupName).SendAsync("TimerStarted");
            await AddLog(groupName, "Started the countdown.");

            _timers[groupName].Timer = new Timer(async _ =>
            {
                if (_timers.TryGetValue(groupName, out var timerState) && timerState.RemainingSeconds > 0 && timerState.Status == TimerStatus.Running)
                {
                    _timers[groupName].RemainingSeconds--;

                    var time = Utils.SecondsToString(timerState.RemainingSeconds);
                    await _hubContext.Clients.Group(groupName).SendAsync("TimerUpdate", time);

                    if (timerState.RemainingSeconds == 0)
                    {
                        StopCountdown(groupName);
                        await _hubContext.Clients.Group(groupName).SendAsync("CountdownCompleted");
                        await AddLog(groupName, "Countdown completed.");
                    }
                }
            }, null, 0, 1000);
        }
        catch (Exception e)
        {
            await AddLog(groupName, $"Error occur while running countdown: {e}");
            await _hubContext.Clients.Group(groupName).SendAsync("TimerUpdate",
                $"Error occur while running countdown: {e}");
        }
    }

    public async Task PauseCountdown(string groupName)
    {
        try
        {
            if (_timers.TryGetValue(groupName, out var timerState) && timerState.Status == TimerStatus.Running)
            {
                timerState.RemainingSeconds++;
                timerState.Status = TimerStatus.Paused;

                await _hubContext.Clients.Group(groupName).SendAsync("TimerPaused");
                await AddLog(groupName, "Paused the countdown.");
            }
        }
        catch (Exception e)
        {
            await AddLog(groupName, $"Error occur while pausing countdown: {e}");
            await _hubContext.Clients.Group(groupName).SendAsync("TimerPaused",
                $"Error occur while pausing countdown: {e}");
        }
        
    }

    public async Task ResumeCountdown(string groupName)
    {
        try
        {
            if (_timers.TryGetValue(groupName, out var timerState) && timerState.Status == TimerStatus.Paused)
            {
                timerState.Status = TimerStatus.Running;

                await _hubContext.Clients.Group(groupName).SendAsync("TimerResume");
                await AddLog(groupName, "Resumed the countdown.");
            }
        }
        catch (Exception e)
        {
            await AddLog(groupName, $"Error occur while resuming countdown: {e}");
            await _hubContext.Clients.Group(groupName).SendAsync("TimerResume",
                $"Error occur while resuming countdown: {e}");
        }
        
    }

    public async Task StopCountdown(string groupName)
    {
        try
        {
            if (_timers.TryRemove(groupName, out var timer))
            {
                timer.Timer?.Dispose();
            }

            await _hubContext.Clients.Group(groupName).SendAsync("TimerStopped");
            await AddLog(groupName, $"Stopped the countdown.");
        }
        catch (Exception e)
        { 
            await AddLog(groupName, $"Error occur while stopping countdown: {e}");
            await _hubContext.Clients.Group(groupName).SendAsync("TimerStopped",
                $"Error occur while stopping countdown: {e}");
        }
        
    }

    public async Task AddCountdown(string groupName, string duration)
    {
        try
        {
            if (_timers.TryGetValue(groupName, out var timer))
            {
                var seconds = Utils.ConvertToSeconds(duration);
                timer.RemainingSeconds = timer.RemainingSeconds + seconds;
                timer.TotalSeconds = timer.TotalSeconds + seconds;
                var time = Utils.SecondsToString(timer.RemainingSeconds);

                await _hubContext.Clients.Group(groupName).SendAsync("TimerModify", time);
                await AddLog(groupName, $"Added {duration} to the countdown.");
            }
        }
        catch (Exception e)
        {
            await AddLog(groupName, $"Error occur while adding extra time to countdown: {e}");
            await _hubContext.Clients.Group(groupName).SendAsync("TimerModify",
                $"Error occur while adding extra time to countdown: {e}");
        }
        
    }

    public async Task MinusCountdown(string groupName, string duration)
    {
        try
        {
            if (_timers.TryGetValue(groupName, out var timer))
            {
                var seconds = Utils.ConvertToSeconds(duration);
                timer.RemainingSeconds = timer.RemainingSeconds - seconds;
                timer.TotalSeconds = timer.TotalSeconds - seconds;
                var time = Utils.SecondsToString(timer.RemainingSeconds);

                await _hubContext.Clients.Group(groupName).SendAsync("TimerModify", time);
                await AddLog(groupName, $"Reduced {duration} from countdown.");
            }
        }
        catch (Exception e)
        {
            await AddLog(groupName, $"Error occur while reducing time from countdown: {e}");
            await _hubContext.Clients.Group(groupName).SendAsync("TimerModify",
                $"Error occur while reducing time from countdown: {e}");
        }
        
    }

    public async Task SendMessage(string groupName, string message)
    {
        try
        {
            await _hubContext.Clients.Group(groupName).SendAsync("ReceiveMessage", message);
            await AddLog(groupName, "Send message to all clients.");
        }
        catch (Exception e)
        {
            await AddLog(groupName, $"Error occur while sending message: {e}");
            await _hubContext.Clients.Group(groupName).SendAsync("ReceiveMessage",
                $"Error occur while sending message: {e}");
        }
        
    }

    public async Task GetReconnectionState(string groupName, string connectionId)
    {
        if (_groupConnections.TryGetValue(connectionId, out var groups))
        {
            AddClientToGroup(groupName, connectionId);
        }
    }

    public async Task AddLog(string groupName, string logMessage)
    {
        _logger.LogInformation($"[Timer][Group: {groupName}] {logMessage}");
    }
}