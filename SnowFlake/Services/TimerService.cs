using Microsoft.AspNetCore.SignalR;
using SnowFlake.Hubs;
using SnowFlake.Services;
using SnowFlake.Utilities;                                                                                                                                                                                                                                                                                                                                                     

public class TimerService : ITimerService
{
    private TimerState _timerStates = new TimerState();
    private readonly IHubContext<TimerHub> _timerHubContext;
    public static HashSet<string> _connectedIds = new HashSet<string>();


    public TimerService(IHubContext<TimerHub> timerHubContext)
    {
        _timerHubContext = timerHubContext;
    }

    // Method to start the timer
    public async Task CreateTimer(string connectionId, int seconds)
    {
        if (await ContainUserId(connectionId) == false) return;

        // Stop any existing timer for this connection
        StopTimer(connectionId);

        // Create a new timer state
        _timerStates = new TimerState
        {
            TotalSeconds = seconds,
            RemainingSeconds = seconds,
            Status = TimerStatus.Running,
            CancellationTokenSource = new CancellationTokenSource()
        };
    }

    public async Task JoinUserGroup(string connectionId)
    {
        if (await ContainUserId(connectionId) == true) return;
        _connectedIds.Add(connectionId);
    }

    public async Task LeaveUserGroup(string connectionId)
    {
        if (await ContainUserId(connectionId) == false) return;
        _connectedIds.Remove(connectionId);
    }

    // Background method to run the timer
    public async Task StartTimer(string connectionId)
    {
        if (await ContainUserId(connectionId) == false) return;
        // Get the timer state for this connection
        var timerState = _timerStates;

        // Continue until time is up or timer is stopped
        while (timerState.RemainingSeconds > 0 && timerState.Status == TimerStatus.Running)
        {
            var remainingSeconds = Utils.SecondsToString(timerState.RemainingSeconds);

            // You would typically broadcast the update to the client here
            await _timerHubContext.Clients.Clients(_connectedIds).SendAsync("TimerUpdate", remainingSeconds);

            // Wait for 1 second
            await Task.Delay(1000);

            // Reduce remaining time
            timerState.RemainingSeconds--;

            // Check if timer is complete
            if (timerState.RemainingSeconds <= 0)
            {
                _timerStates = new TimerState();
                break;
            }
        }
    }

    // Method to pause the timer
    public async Task PauseTimer(string connectionId)
    {
        if (await ContainUserId(connectionId) == false) return;

        // Check if timer exists and is running
        if (_timerStates is not null &&
            _timerStates.Status == TimerStatus.Running)
        {
            // Cancel the current timer
            _timerStates.CancellationTokenSource.Cancel();
            _timerStates.CancellationTokenSource = new CancellationTokenSource();
            _timerStates.Status = TimerStatus.Paused;

            await _timerHubContext.Clients.Clients(_connectedIds).SendAsync("TimerPaused");
        }
    }

    // Method to resume the timer
    public async Task ResumeTimer(string connectionId)
    {
        if (await ContainUserId(connectionId) == false) return;

        // Check if timer exists and is paused
        if (_timerStates is not null &&
            _timerStates.Status == TimerStatus.Paused)
        {
            _timerStates.RemainingSeconds++;
            // Reset cancellation token and status
            _timerStates.CancellationTokenSource.Cancel();
            _timerStates.CancellationTokenSource = new CancellationTokenSource();
            _timerStates.Status = TimerStatus.Running;

            // Restart the timer
            StartTimer(connectionId);
        }
    }

    // Method to stop the timer
    public async Task StopTimer(string connectionId)
    {
        if (await ContainUserId(connectionId) == false) return;

        // Check if timer exists
        if (_timerStates is not null && _timerStates.CancellationTokenSource is not null)
        {
            // Cancel the timer
            _timerStates.CancellationTokenSource.Cancel();
            _timerStates.CancellationTokenSource = new CancellationTokenSource();
            _timerStates.Status = TimerStatus.Stopped;

            // Remove the timer state
            _timerStates = new TimerState();

            await _timerHubContext.Clients.Clients(_connectedIds).SendAsync("TimerStopped");
        }
    }

    public async Task ModifyTimer(string connectionId, int secondsToModify)
    {
        if (await ContainUserId(connectionId) == false) return;

        // Check if the timer exists
        if (_timerStates is not null && _timerStates.CancellationTokenSource is not null)
        {
            if (_timerStates.RemainingSeconds > 0)
            {
                // Update the remaining time
                _timerStates.RemainingSeconds += secondsToModify;
                // Notify the client about the updated time
                var remainingSeconds = Utils.SecondsToString(_timerStates.RemainingSeconds);
                await _timerHubContext.Clients.Clients(_connectedIds).SendAsync("TimerUpdated", remainingSeconds);
            }
            else
            {
                // Handle the case where the timer has already expired
                await _timerHubContext.Clients.Clients(_connectedIds).SendAsync("Error", "Timer has already expired.");
            }
        }
        else
        {
            // Handle the case where the timer does not exist
            await _timerHubContext.Clients.Clients(_connectedIds).SendAsync("Error", "Timer not found.");
        }
    }

    private async Task<bool> ContainUserId(string connectionId)
    {
        if (_connectedIds.Contains(connectionId))
        {
            return true;
        }
        return false;
    }
}
