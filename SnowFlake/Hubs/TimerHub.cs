using Microsoft.AspNetCore.SignalR;

namespace SnowFlake.Hubs;

public class TimerHub : Hub
{
    public async Task SendTimerUpdate(int secondsRemaining)
    {
        await Clients.All.SendAsync("ReceiveTimerUpdate", secondsRemaining);
    }
}
