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
    private static readonly ConcurrentDictionary<string, TimerState> _timers = new();
    private static readonly ConcurrentDictionary<string, HashSet<string>> _groupConnections = new();

    public TimerService(IHubContext<TimerHub> hubContext)
    {
        _hubContext = hubContext;
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
        }
        catch (Exception e)
        {
            await _hubContext.Clients.Group(groupName).SendAsync("JoinUserGroup",
                $"Error occur while adding client to group: {e}");
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
        }
        catch (Exception e)
        {
            await _hubContext.Clients.Group(groupName).SendAsync("LeaveUserGroup",
                $"Error occur while removing client from group: {connectionId}");
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
        }
        catch (Exception e)
        {
            await _hubContext.Clients.Group(groupName).SendAsync("CreateTimer",
                $"Error occur while creating timer: {e}");
        }
        

    }

    public async Task StartCountdown(string groupName)
    {
        try
        {
            _timers[groupName].Status = TimerStatus.Running;

            _hubContext.Clients.Group(groupName).SendAsync("TimerStarted");

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
                    }
                }
            }, null, 0, 1000);
        }
        catch (Exception e)
        {
            await _hubContext.Clients.Group(groupName).SendAsync("TimerUpdate",
                $"Error occur while starting timer: {e}");
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

                // Notify the group
                await _hubContext.Clients.Group(groupName).SendAsync("TimerPaused");
            }
        }
        catch (Exception e)
        {
            await _hubContext.Clients.Group(groupName).SendAsync("TimerPaused",
                $"Error occur while pausing timer: {e}");
        }
        
    }

    public async Task ResumeCountdown(string groupName)
    {
        try
        {
            if (_timers.TryGetValue(groupName, out var timerState) && timerState.Status == TimerStatus.Paused)
            {
                timerState.Status = TimerStatus.Running;

                // Notify the group
                await _hubContext.Clients.Group(groupName).SendAsync("TimerResume");
            }
        }
        catch (Exception e)
        {
            await _hubContext.Clients.Group(groupName).SendAsync("TimerResume",
                $"Error occur while resuming timer: {e}");
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
        }
        catch (Exception e)
        {
            await _hubContext.Clients.Group(groupName).SendAsync("TimerStopped",
                $"Error occur while stopping timer: {e}");
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
                // Notify the group
                await _hubContext.Clients.Group(groupName).SendAsync("TimerModify", time);
            }
        }
        catch (Exception e)
        {
            await _hubContext.Clients.Group(groupName).SendAsync("TimerModify",
                $"Error occur while adding more timer: {e}");
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
                // Notify the group
                await _hubContext.Clients.Group(groupName).SendAsync("TimerModify", time);
            }
        }
        catch (Exception e)
        {
            await _hubContext.Clients.Group(groupName).SendAsync("TimerModify",
                $"Error occur while reducing timer: {e}");
        }
        
    }

    public async Task SendMessage(string groupName, string message)
    {
        try
        {
            await _hubContext.Clients.Group(groupName).SendAsync("ReceiveMessage", message);
        }
        catch (Exception e)
        {
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
}