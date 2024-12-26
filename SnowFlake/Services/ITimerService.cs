namespace SnowFlake.Services;

public interface ITimerService
{
    Task JoinUserGroup(string connectionId);
    Task LeaveUserGroup(string connectionId);
    Task CreateTimer(string connectionId, int seconds); 
    Task StartTimer(string connectionId);
    Task PauseTimer(string connectionId);
    Task ResumeTimer(string connectionId);
    Task StopTimer(string connectionId);
    Task ModifyTimer(string connectionId, int secondsToModify);
}