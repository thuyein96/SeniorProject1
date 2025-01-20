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

    public async Task CreateTimer(string groupName, string durationSeconds)
        => await _countdownService.CreateCountdown(groupName, durationSeconds);
    public Task StartCountdown(string groupName)
        => _countdownService.StartCountdown(groupName);

    public Task PauseCountdown(string groupName)
        => _countdownService.PauseCountdown(groupName);

    public Task ResumeCountdown(string groupName)
        => _countdownService.ResumeCountdown(groupName);

    public Task StopCountdown(string groupName)
        => _countdownService.StopCountdown(groupName);

    public Task JoinGroup(string groupName)
        => _countdownService.AddClientToGroup(groupName, Context.ConnectionId);

    public Task LeaveGroup(string groupName)
        => _countdownService.RemoveClientFromGroup(groupName, Context.ConnectionId);

    public override async Task OnDisconnectedAsync(Exception exception) 
        => await base.OnDisconnectedAsync(exception);
}
