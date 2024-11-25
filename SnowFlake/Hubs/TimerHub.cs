using System.Threading;
using Microsoft.AspNetCore.SignalR;

namespace SnowFlake.Hubs;

public class TimerHub : Hub
{
    private static CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private static int _currentTimeRemaining; // Stores the remaining time

    // Start or Resume the Timer
    public async Task SendTimerUpdate(int secondsRemaining)
    {
        // Set initial value if the timer is being started for the first time
        if (_currentTimeRemaining == 0)
        {
            _currentTimeRemaining = secondsRemaining;
        }

        _cancellationTokenSource = new CancellationTokenSource(); // Create a fresh token

        try
        {
            while (_currentTimeRemaining > 0)
            {
                // Check if the cancellation was requested
                _cancellationTokenSource.Token.ThrowIfCancellationRequested();

                var timer = TimeOnly.FromTimeSpan(TimeSpan.FromSeconds(_currentTimeRemaining));

                // Send the timer update to all clients
                await Clients.All.SendAsync("ReceiveTimerUpdate", timer.ToString("mm:ss"));

                // Decrement the remaining seconds
                _currentTimeRemaining--;

                // Wait for 1 second, respecting cancellation
                await Task.Delay(1000, _cancellationTokenSource.Token);
            }

            // Send a final update to indicate the timer has reached 0
            await Clients.All.SendAsync("ReceiveTimerUpdate", "00:00");
        }
        catch (OperationCanceledException)
        {
            // Handle cancellation (for pause or stop)
        }
    }

    // Stop the Timer
    public async Task StopTimer()
    {
        _cancellationTokenSource.Cancel(); // Cancel the task
        _currentTimeRemaining = 0; // Reset the timer

        // Notify all clients that the timer is stopped
        await Clients.All.SendAsync("ReceiveTimerUpdate", "00:00");
    }

    // Pause the Timer
    public async Task PauseTimer()
    {
        _cancellationTokenSource.Cancel(); // Signal cancellation

        // Notify all clients that the timer is paused
        await Clients.All.SendAsync("ReceiveTimerPaused", _currentTimeRemaining);
    }

    // Resume the Timer
    public async Task ResumeTimer()
    {
        if (_currentTimeRemaining > 0) // Only resume if there is time remaining
        {
            _cancellationTokenSource = new CancellationTokenSource(); // Create a fresh token
            await SendTimerUpdate(_currentTimeRemaining); // Resume with the remaining time
        }
        else
        {
            await Clients.All.SendAsync("ReceiveTimerUpdate", "00:00");
        }
    }

    public async Task AddTime(int addExtraTime)
    {
        _currentTimeRemaining += addExtraTime;

        // Notify all clients of the updated time
        var updatedTime = TimeOnly.FromTimeSpan(TimeSpan.FromSeconds(_currentTimeRemaining)).ToString("mm:ss");
        await Clients.All.SendAsync("ReceiveTimerUpdate", updatedTime);
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await Clients.All.SendAsync("ReceiveMessage", "disconnect");
        await base.OnDisconnectedAsync(exception);
    }
}
