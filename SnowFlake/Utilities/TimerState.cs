namespace SnowFlake.Utilities;

public class TimerState
{
    public int TotalSeconds { get; set; }
    public int RemainingSeconds { get; set; }
    public TimerStatus? Status { get; set; }
    public Timer? Timer { get; set; }
}
