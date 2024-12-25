using Microsoft.AspNetCore.SignalR;
using SnowFlake.Hubs;
using SnowFlake.Services;
using SnowFlake.Utilities;

public class TimerService : ITimerService
{
    private Dictionary<string, TimerState> _timerStates = new Dictionary<string, TimerState>();
    private readonly IHubContext<TimerHub> _timerHubContext;

    public TimerService(IHubContext<TimerHub> timerHubContext)
    {
        _timerHubContext = timerHubContext;
    }

    // Method to start the timer
    public async Task StartTimer(string connectionId, int seconds)
    {
        // Stop any existing timer for this connection
        StopTimer(connectionId);

        // Create a new timer state
        var timerState = new TimerState
        {
            TotalSeconds = seconds,
            RemainingSeconds = seconds,
            Status = TimerStatus.Running,
            CancellationTokenSource = new CancellationTokenSource()
        };

        // Store the timer state
        _timerStates[connectionId] = timerState;

        // Start the timer in the background
        RunTimer(connectionId);
    }

    // Background method to run the timer
    private async void RunTimer(string connectionId)
    {
        // Get the timer state for this connection
        var timerState = _timerStates[connectionId];

        // Continue until time is up or timer is stopped
        while (timerState.RemainingSeconds > 0 && timerState.Status == TimerStatus.Running)
        {
            var remainingSeconds = Utils.SecondsToString(timerState.RemainingSeconds);

            
            // You would typically broadcast the update to the client here
            await _timerHubContext.Clients.Client(connectionId).SendAsync("TimerUpdate", remainingSeconds);

            // Wait for 1 second
            await Task.Delay(1000);

            // Reduce remaining time
            timerState.RemainingSeconds--;

            // Check if timer is complete
            if (timerState.RemainingSeconds <= 0)
            {
                _timerStates.Remove(connectionId);
                break;
            }
        }
    }

    // Method to pause the timer
    public async Task PauseTimer(string connectionId)
    {
        // Check if timer exists and is running
        if (_timerStates.TryGetValue(connectionId, out var timerState) &&
            timerState.Status == TimerStatus.Running)
        {
            // Cancel the current timer
            timerState.CancellationTokenSource.Cancel();
            timerState.CancellationTokenSource = new CancellationTokenSource();
            timerState.Status = TimerStatus.Paused;

            await _timerHubContext.Clients.Client(connectionId).SendAsync("TimerPaused");
        }
    }

    // Method to resume the timer
    public async Task ResumeTimer(string connectionId)
    {
        // Check if timer exists and is paused
        if (_timerStates.TryGetValue(connectionId, out var timerState) &&
            timerState.Status == TimerStatus.Paused)
        {
            timerState.RemainingSeconds++;
            // Reset cancellation token and status
            timerState.CancellationTokenSource.Cancel();
            timerState.CancellationTokenSource = new CancellationTokenSource();
            timerState.Status = TimerStatus.Running;

            // Restart the timer
            RunTimer(connectionId);
        }
    }

    // Method to stop the timer
    public async Task StopTimer(string connectionId)
    {
        // Check if timer exists
        if (_timerStates.TryGetValue(connectionId, out var timerState))
        {
            // Cancel the timer
            timerState.CancellationTokenSource.Cancel();
            timerState.Status = TimerStatus.Stopped;

            // Remove the timer state
            _timerStates.Remove(connectionId);

            await _timerHubContext.Clients.Client(connectionId).SendAsync("TimerStopped");
        }
    }

    public async Task ModifyTimer(string connectionId, int secondsToModify)
    {
        // Check if the timer exists
        if (_timerStates.TryGetValue(connectionId, out var timerState))
        {
            if (timerState.RemainingSeconds > 0)
            {
                // Update the remaining time
                timerState.RemainingSeconds += secondsToModify;
                // Notify the client about the updated time
                var remainingSeconds = Utils.SecondsToString(timerState.RemainingSeconds);
                await _timerHubContext.Clients.Client(connectionId).SendAsync("TimerUpdated", remainingSeconds);
            }
            else
            {
                // Handle the case where the timer has already expired
                await _timerHubContext.Clients.Client(connectionId).SendAsync("Error", "Timer has already expired.");
            }
        }
        else
        {
            // Handle the case where the timer does not exist
            await _timerHubContext.Clients.Client(connectionId).SendAsync("Error", "Timer not found.");
        }
    }
}
