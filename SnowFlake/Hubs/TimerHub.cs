using System.Threading;
using Microsoft.AspNetCore.SignalR;

namespace SnowFlake.Hubs;

public class TimerHub : Hub
{
    public async Task SendTimerUpdate(int secondsRemaining)
    {
        while (secondsRemaining > 0) // Loop until the countdown reaches 0
        {
            var timer = TimeOnly.FromTimeSpan(TimeSpan.FromSeconds(secondsRemaining));

            // Send the timer update to all clients
            await Clients.All.SendAsync("ReceiveTimerUpdate", timer.ToString("mm:ss"));

            // Decrement the remaining seconds
            secondsRemaining--;

            // Wait for 1 second
            await Task.Delay(1000);
        }

        // Send a final update to indicate the timer has reached 0
        await Clients.All.SendAsync("ReceiveTimerUpdate", "00:00");
    }
}
