using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;
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
                foreach (var round in createPlaygroundRequest.Rounds)
                {
                    rounds.Add(new RoundEntity
                    {
                        RoundNumber = round.Key,
                        Duration = round.Value,
                        Progress = GameProgress.Pending.Name,
                    });
                }

                var playground = new PlaygroundEntity
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    HostRoomCode = createPlaygroundRequest.HostRoomCode,
                    PlayerRoomCode = createPlaygroundRequest.PlayerRoomCode,
                    HostId = createPlaygroundRequest.HostId,
                    NumberOfTeam = createPlaygroundRequest.NumberOfTeam,
                    MaxTeamMember = createPlaygroundRequest.MaxTeamMember,
                    TeamToken = createPlaygroundRequest.TeamToken,
                    Rounds = rounds,
                    CreationDate = DateTime.Now,
                    ModifiedDate = null
                };

                for (int i = 1; i < createPlaygroundRequest.NumberOfTeam + 1; i++)
                {
                    var team = new TeamEntity
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        TeamNumber = i,
                        Tokens = createPlaygroundRequest.TeamToken,
                        MaxMembers = createPlaygroundRequest.MaxTeamMember,
                        PlaygroundId = playground.Id,
                        CreationDate = DateTime.Now,
                        ModifiedDate = null
                    };

                    _unitOfWork.TeamRepository.Create(team);
                    _unitOfWork.Commit();
                }

                _unitOfWork.PlaygroundRepository.Create(playground);
                _unitOfWork.Commit();
                return playground;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public async Task<PlaygroundEntity> GetPlayground(string user, string roomcode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(roomcode)) return null;
                if (user == "Host")
                {
                    return (await _unitOfWork.PlaygroundRepository.GetBy(p => p.HostRoomCode == roomcode)).FirstOrDefault();
                }
                if (user == "Player")
                {
                    return (await _unitOfWork.PlaygroundRepository.GetBy(p => p.PlayerRoomCode == roomcode)).FirstOrDefault();
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
