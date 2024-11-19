using Microsoft.AspNetCore.SignalR;
using SnowFlake.DAO;
using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs;
using SnowFlake.Dtos.APIs.Playground;
using SnowFlake.Dtos.APIs.Team.CreateTeam;
using SnowFlake.UnitOfWork;

namespace SnowFlake.Services
{
    public class PlaygroundService : IPlaygroundService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext _hubContext;

        public PlaygroundService(IUnitOfWork unitOfWork, IHubContext hubContext)
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
                    RoomCode = createPlaygroundRequest.RoomCode,
                    Rounds = createPlaygroundRequest.Rounds,
                    MaxTeam = createPlaygroundRequest.MaxTeam,
                    TeamToken = createPlaygroundRequest.TeamToken,
                    CreationDate = createPlaygroundRequest.CreatedAt,
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
    }
}
