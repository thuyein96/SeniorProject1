﻿using Microsoft.AspNetCore.SignalR;
using SnowFlake.Hubs;
using SnowFlake.Services;
using SnowFlake.Utilities;
using System.Collections.Concurrent;

public class TimerService : ITimerService
{
    private readonly IHubContext<TimerHub> _hubContext;
    private static readonly ConcurrentDictionary<string, TimerState> _timers = new();

    public TimerService(IHubContext<TimerHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task AddClientToGroup(string groupName, string connectionId)
    {
        await _hubContext.Groups.AddToGroupAsync(connectionId, groupName);
        await _hubContext.Clients.Group(groupName).SendAsync("JoinUserGroup", $"{connectionId} joined the '{groupName}' group.");
    }

    public async Task RemoveClientFromGroup(string groupName, string connectionId)
    {
        await _hubContext.Groups.RemoveFromGroupAsync(connectionId, groupName);
        await _hubContext.Clients.Group(groupName).SendAsync("LeaveUserGroup", $"{connectionId} leaved the '{groupName}' group.");
    }

    public async Task CreateCountdown(string groupName, string duration)
    {
        var seconds = Utils.ConvertToSeconds(duration);
        if (_timers.ContainsKey(groupName))
        {
            StopCountdown(groupName);
        }

        _timers[groupName] = new TimerState
        {
            RemainingSeconds = seconds+1,
            TotalSeconds = seconds,
        };
        await _hubContext.Clients.Group(groupName).SendAsync("CreateTimer", $"{duration} Timer is created");

    }

    public Task StartCountdown(string groupName)
    {
        _timers[groupName].Status = TimerStatus.Running;

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

        return _hubContext.Clients.Group(groupName).SendAsync("TimerStarted");
    }

    public async Task PauseCountdown(string groupName)
    {
        if (_timers.TryGetValue(groupName, out var timerState) && timerState.Status == TimerStatus.Running)
        {
            timerState.RemainingSeconds++;
            timerState.Status = TimerStatus.Paused;

            // Notify the group
            await _hubContext.Clients.Group(groupName).SendAsync("TimerPaused");
        }
    }

    public async Task ResumeCountdown(string groupName)
    {
        if (_timers.TryGetValue(groupName, out var timerState) && timerState.Status == TimerStatus.Paused)
        {
            timerState.Status = TimerStatus.Running;

            // Notify the group
            await _hubContext.Clients.Group(groupName).SendAsync("TimerResume");
        }
    }

    public async Task StopCountdown(string groupName)
    {
        if (_timers.TryRemove(groupName, out var timer))
        {
            timer.Timer.Dispose();
        }

        _timers.TryRemove(groupName, out _);

        await _hubContext.Clients.Group(groupName).SendAsync("TimerStopped");
    }

    public async Task AddCountdown(string groupName, string duration)
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

    public async Task MinusCountdown(string groupName, string duration)
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
}