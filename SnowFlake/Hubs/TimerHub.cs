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

        public async Task StartTimer(List<int> durationSeconds, int intervalPeriod)
        {
            await _timerService.StartTimer(Context.ConnectionId, durationSeconds, intervalPeriod);
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
            await _timerService.SkipTimer(Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
