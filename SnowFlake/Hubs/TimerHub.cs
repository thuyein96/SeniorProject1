using Microsoft.AspNetCore.SignalR;

namespace SnowFlake.Hubs;

public class TimerHub : Hub
{
    public async Task SendTimerUpdate(string connectionId, int secondsRemaining)
    {
        for (int i = 0; i < secondsRemaining; i++)
        {
            var currentTime = DateTime.Now;

            Thread.Sleep(1000);
            await Clients.Client(connectionId).SendAsync("ReceiveTimerUpdate", currentTime);
        }
    }
}
