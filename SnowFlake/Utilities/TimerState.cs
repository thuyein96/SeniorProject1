namespace SnowFlake.Utilities;

public class TimerState
{
    public CancellationTokenSource CancellationTokenSource { get; set; }
    public int TotalSeconds { get; set; }
    public int RemainingSeconds { get; set; }
    public TimerStatus Status { get; set; }
}
