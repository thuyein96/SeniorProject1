using Microsoft.AspNetCore.SignalR;
using SnowFlake.Services;

namespace SnowFlake.Hubs
{
    public class TimerHub : Hub
    {
        private readonly ITimerService _timerService;

        public TimerHub(ITimerService timerService)
        {
            _timerService = timerService;
        }
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("ReceivedMessage", $"{Context.ConnectionId} is connected");
        }

        public async Task StartTimer(int durationSeconds)
        {
            await _timerService.StartTimer(Context.ConnectionId, durationSeconds);
        }

        public async Task PauseTimer()
        {
            await _timerService.PauseTimer(Context.ConnectionId);
        }

        public async Task ResumeTimer()
        {
            await _timerService.ResumeTimer(Context.ConnectionId);
        }

        public async Task StopTimer()
        {
            await _timerService.StopTimer(Context.ConnectionId);
        }

        public async Task SkipTimer()
        {
            await _timerService.StopTimer(Context.ConnectionId);
        }

        public async Task AddSeconds(int seconds)
        {
            await _timerService.ModifyTimer(Context.ConnectionId, seconds);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
