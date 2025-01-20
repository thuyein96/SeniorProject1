namespace SnowFlake.Services;

public interface ITimerService
{
    Task CreateCountdown(string groupName, string duration);
    Task StartCountdown(string groupName);
    Task PauseCountdown(string groupName);
    Task ResumeCountdown(string groupName);
    Task StopCountdown(string groupName);
    Task AddClientToGroup(string groupName, string connectionId);
    Task RemoveClientFromGroup(string groupName, string connectionId);
}