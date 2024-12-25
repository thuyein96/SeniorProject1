using Microsoft.AspNetCore.SignalR;
using SnowFlake.Services;
using SnowFlake.Utilities;

namespace SnowFlake.Hubs
{
    public class TimerHub : Hub
    {
        private readonly ITimerService _timerService;

        public TimerHub(ITimerService timerService, IPlaygroundService playgroundService)
        {
            _timerService = timerService;
        }
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("ReceivedMessage", $"{Context.ConnectionId} is connected");
        }

        public async Task StartTimer(string durationSeconds)
        {
            var timer = Utils.ConvertToSeconds(durationSeconds);
            await _timerService.StartTimer(Context.ConnectionId, timer);
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

        public async Task AddTimer(string extraDuration)
        {
            var timer = Utils.ConvertToSeconds(extraDuration);
            await _timerService.ModifyTimer(Context.ConnectionId, timer);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
