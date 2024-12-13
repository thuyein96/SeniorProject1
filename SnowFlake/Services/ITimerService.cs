namespace SnowFlake.Services;

public interface ITimerService
{
    Task StartTimer(string connectionId, int seconds);
    Task PauseTimer(string connectionId);
    Task ResumeTimer(string connectionId);
    Task StopTimer(string connectionId);
    Task ModifyTimer(string connectionId, int secondsToModify);
}