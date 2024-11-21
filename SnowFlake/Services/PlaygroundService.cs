using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SnowFlake.DAO;
using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs;
using SnowFlake.Dtos.APIs.Playground;
using SnowFlake.Dtos.APIs.Team.CreateTeam;
using SnowFlake.Hubs;
using SnowFlake.UnitOfWork;

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

        public bool Create(CreatePlaygroundRequest createPlaygroundRequest)
        {
            try
            {
                if (createPlaygroundRequest is null) return false;

                var playground = new PlaygroundEntity
                {
                    Id = createPlaygroundRequest.Id.ToString(),
                    RoomCode = createPlaygroundRequest.RoomCode,
                    Rounds = createPlaygroundRequest.Rounds,
                    MaxTeam = createPlaygroundRequest.MaxTeam,
                    TeamToken = createPlaygroundRequest.TeamToken,
                    CreationDate = DateTime.Now,
                    ModifiedDate = null
                };

                _unitOfWork.PlaygroundRepository.Create(playground);
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception)
            {

                return false;
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
