using Microsoft.AspNetCore.SignalR;
using SnowFlake.Hubs;
using SnowFlake.Services;
using SnowFlake.UnitOfWork;
using SnowFlake.Utilities;

public class TimerService : ITimerService
{
    // Dictionary to store timer information for different connections
    private Dictionary<string, TimerState> _timerStates = new Dictionary<string, TimerState>();
    private readonly IHubContext<TimerHub> _hubContext;
    private Dictionary<string, Queue<int>> _timerQueues = new Dictionary<string, Queue<int>>();
    private int _currentRoundNumber = 0;

    public TimerService(IHubContext<TimerHub> hubContext)
    {
        _hubContext = hubContext;
    }


    // Method to start the timer
    public async Task StartTimer(string connectionId, List<int> durations, int intervalPeriod)
    {
        StopTimer(connectionId);

        // Initialize queue for the connection
        var timerQueue = new Queue<int>();
        AddIntervalPeriodInBetween(durations, intervalPeriod, timerQueue);
        _timerQueues[connectionId] = timerQueue;

        // Initialize timer state
        if (timerQueue.TryDequeue(out var duration))
        {
            var timerState = new TimerState
            {
                TotalSeconds = duration,
                RemainingSeconds = duration,
                Status = TimerStatus.Running,
                CancellationTokenSource = new CancellationTokenSource()
            };
            _timerStates[connectionId] = timerState;
            // Start the first timer
            RunTimer(connectionId);
        }
    }

    private static void AddIntervalPeriodInBetween(List<int> durations, int intervalPeriod, Queue<int> timerQueue)
    {
        for (int i = 0; i < durations.Count; i++)
        {
            timerQueue.Enqueue(durations[i]);
            timerQueue.Enqueue(intervalPeriod);
        }
    }

    // Background method to run the timer
    private async void RunTimer(string connectionId)
    {
        if (!_timerStates.TryGetValue(connectionId, out var timerState)) return;

        while (timerState.RemainingSeconds > 0 && !timerState.CancellationTokenSource.IsCancellationRequested)
        {
            var time = Utils.SecondsToString(timerState.RemainingSeconds);
            await _hubContext.Clients.Client(connectionId).SendAsync("TimerUpdate", time);
            await Task.Delay(1000);
            timerState.RemainingSeconds--;
        }

        if (timerState.RemainingSeconds <= 0)
        {
            _currentRoundNumber++;
            // Check for the next duration in the queue
            if (_timerQueues.TryGetValue(connectionId, out var timerQueue) && timerQueue.TryDequeue(out var nextDuration))
            {
                if (_currentRoundNumber % 2 == 0)
                {
                    await _hubContext.Clients.Client(connectionId).SendAsync("TimerUpdate", "Interval Period");
                }
                else
                {
                    await _hubContext.Clients.Client(connectionId).SendAsync("TimerUpdate", $"Finished Round {_currentRoundNumber}");
                }

                timerState.TotalSeconds = nextDuration;
                timerState.RemainingSeconds = nextDuration;
                timerState.CancellationTokenSource = new CancellationTokenSource();
                RunTimer(connectionId);
            }
            else
            {
                // No more durations, stop the timer
                _timerStates.Remove(connectionId);
                _timerQueues.Remove(connectionId);
                await _hubContext.Clients.Client(connectionId).SendAsync("TimerFinished");
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

    public async Task SkipTimer(string connectionId)
    {
        if (_timerStates.TryGetValue(connectionId, out var timerState))
        {
            timerState.CancellationTokenSource.Cancel();
            _currentRoundNumber++;

            // Start the next round if available
            if (_timerQueues.TryGetValue(connectionId, out var timerQueue) && timerQueue.TryDequeue(out var nextDuration))
            {
                timerState.TotalSeconds = nextDuration;
                timerState.RemainingSeconds = nextDuration;
                timerState.Status = TimerStatus.Running;
                timerState.CancellationTokenSource = new CancellationTokenSource();
                RunTimer(connectionId);
            }
            else
            {
                // No more rounds, end the timer
                _timerStates.Remove(connectionId);
                _timerQueues.Remove(connectionId);
                await _hubContext.Clients.Client(connectionId).SendAsync("TimerFinished");
            }
        }
    }

}