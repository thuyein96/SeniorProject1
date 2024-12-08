using Microsoft.AspNetCore.SignalR;
using SnowFlake.Hubs;
using SnowFlake.Services;
using SnowFlake.Utilities;
using System.Diagnostics;

public class TimerService : ITimerService
{
    // Dictionary to store timer information for different connections
    private Dictionary<string, TimerState> _timerStates = new Dictionary<string, TimerState>();
    private readonly IHubContext<TimerHub> _hubContext;

    public TimerService(IHubContext<TimerHub> hubContext)
    {
        _hubContext = hubContext;
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
        while (timerState.RemainingSeconds > 0 && !timerState.CancellationTokenSource.IsCancellationRequested)
        {
            // Wait for 1 second
            await Task.Delay(1000);

            // Reduce remaining time
            timerState.RemainingSeconds--;

            // You would typically broadcast the update to the client here
            await _hubContext.Clients.Client(connectionId).SendAsync("TimerUpdate", timerState.RemainingSeconds);

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
            timerState.Status = TimerStatus.Paused;

            await _hubContext.Clients.Client(connectionId).SendAsync("TimerPaused");
        }
    }

    // Method to resume the timer
    public async Task ResumeTimer(string connectionId)
    {
        // Check if timer exists and is paused
        if (_timerStates.TryGetValue(connectionId, out var timerState) &&
            timerState.Status == TimerStatus.Paused)
        {
            // Reset cancellation token and status
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

            await _hubContext.Clients.Client(connectionId).SendAsync("TimerStopped");
        }
    }
}