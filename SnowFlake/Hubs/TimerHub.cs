using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.SignalR;
using SnowFlake.Services;
using SnowFlake.Utilities;

namespace SnowFlake.Hubs;

public class TimerHub : Hub
{
    private readonly ITimerService _countdownService;

    public TimerHub(ITimerService countdownService)
    {
        _countdownService = countdownService;
    }
    
    public override async Task OnConnectedAsync() 
        => await Clients.Caller.SendAsync("ReceivedMessage", $"{Context.ConnectionId} is connected");

    public async Task CreateTimer(string groupName, string durationSeconds, string gameState)
        => await _countdownService.CreateCountdown(groupName, durationSeconds, gameState);
    public Task StartCountdown(string groupName)
        => _countdownService.StartCountdown(groupName);

    public Task PauseCountdown(string groupName)
        => _countdownService.PauseCountdown(groupName);

    public Task ResumeCountdown(string groupName)
        => _countdownService.ResumeCountdown(groupName);

    public Task StopCountdown(string groupName)
        => _countdownService.StopCountdown(groupName);

    public Task AddCountdown(string groupName, string duration) 
        => _countdownService.AddCountdown(groupName, duration);

    public Task MinusCountdown(string groupName, string duration)
        => _countdownService.MinusCountdown(groupName, duration);

    public Task JoinGroup(string groupName)
        => _countdownService.AddClientToGroup(groupName, Context.ConnectionId);

    public Task LeaveGroup(string groupName)
        => _countdownService.RemoveClientFromGroup(groupName, Context.ConnectionId);

    public Task SendMessage(string groupName, string message)
        => _countdownService.SendMessage(groupName, message);

    public Task Reconnect(string gameName)
        => _countdownService.GetReconnectionState(gameName, Context.ConnectionId);

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await Clients.Caller.SendAsync("ReceivedMessage", $"{Context.ConnectionId} is disconnected");
        await base.OnDisconnectedAsync(exception);

    }
}
