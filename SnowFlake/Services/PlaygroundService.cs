using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Playground;
using SnowFlake.Hubs;
using SnowFlake.UnitOfWork;
using SnowFlake.Utilities;

namespace SnowFlake.Services
{
    public class PlaygroundService : IPlaygroundService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<TimerHub> _hubContext;

        public PlaygroundService(IUnitOfWork unitOfWork, IHubContext<TimerHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }

        //public async Task StartGame(string playerId, string roomCode)
        //{
        //    var gameRules = _unitOfWork.PlaygroundRepository.GetBy(p => p.PlayerId == playerId && p.RoomCode == roomCode).FirstOrDefault();
        //    if (gameRules == null) return;

        //    if (gameRules.Rounds[0].Progress == GameProgress.Pending.ToString())
        //        gameRules.Rounds[0].Progress = GameProgress.OnGoing.ToString();


        //    _unitOfWork.PlaygroundRepository.Update(gameRules);
        //    _unitOfWork.Commit();

        //    await RunCountdown(gameRules.Rounds[0].Id, gameRules.Rounds[0].Duration);
        //}

        //private async Task RunCountdown(string roundId, string duration)
        //{
        //    var remainingTime = Utils.ConvertToSeconds(duration);

        //    // Send the initial countdown time to all clients
        //    await _hubContext.Clients.All.SendAsync("SendTimerUpdate", remainingTime);

        //    // Countdown loop
        //    while (remainingTime > 0)
        //    {
        //        // Broadcast the updated time to all connected clients
        //        await _hubContext.Clients.All.SendAsync("SendTimerUpdate", remainingTime);

        //        await Task.Delay(1000); // Wait for 1 second
        //        remainingTime--;
        //    }

        //    // Round has ended, notify clients
        //    await EndRound(roundId);
        //}

        //public async Task EndRound(string playgroundId, string roundId)
        //{
        //    var round = _unitOfWork.PlaygroundRepository.GetBy(p => p.Id == playgroundId).FirstOrDefault();
        //    if (round != null)
        //    {
        //        var progress = round.Rounds.Where(r => r.Id == roundId).FirstOrDefault().Progress;
        //        if(string.IsNullOrWhiteSpace(progress) || progress != GameProgress.Finished.Name)
        //        {
        //            progress = GameProgress.Finished.Name;
        //        }
        //        await _context.SaveChangesAsync();
        //    }

        //    // Notify all clients that the round has ended
        //    await _hubContext.Clients.All.SendAsync("RoundEnded", roundId);
        //}








        public async Task<PlaygroundEntity> Create(CreatePlaygroundRequest createPlaygroundRequest)
        {
            try
            {
                if (createPlaygroundRequest is null) return null;

                var rounds = new List<RoundEntity>();
                foreach(var round in createPlaygroundRequest.Rounds)
                {
                    rounds.Add(new RoundEntity
                    {
                        RoundNumber = round.Key,
                        Duration = round.Value,
                        Progress = GameProgress.Pending.Name
                    });
                }
                var playground = new PlaygroundEntity
                {
                    Id = createPlaygroundRequest.Id.ToString(),
                    RoomCode = createPlaygroundRequest.RoomCode,
                    Rounds = rounds,
                    MaxTeam = createPlaygroundRequest.MaxTeam,
                    TeamToken = createPlaygroundRequest.TeamToken,
                    CreationDate = DateTime.Now,
                    ModifiedDate = null
                };

                _unitOfWork.PlaygroundRepository.Create(playground);
                _unitOfWork.Commit();
                return playground;
            }
            catch (Exception)
            {

                return null;
            }
        }

        [HttpPost("start-timer")]
        public async Task<bool> StartTimer(int durationInSeconds)
        {
            for (int i = durationInSeconds; i >= 0; i--)
            {
                // Send timer updates to all clients
                await _hubContext.Clients.All.SendAsync("ReceiveTimerUpdate", i);
                Thread.Sleep(1000);
            }

            return true;
        }
    }
}
