namespace SnowFlake.Services;

public interface ITimerService
{
    Task StartTimer(string connectionId, List<int> durations, int IntervalPeriod);
    Task PauseTimer(string connectionId);
    Task ResumeTimer(string connectionId);
    Task StopTimer(string connectionId);
    Task SkipTimer(string connectionId);
}
