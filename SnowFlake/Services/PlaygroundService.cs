using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;
using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Playground;
using SnowFlake.Dtos.APIs.Playground.UpdatePlayground;
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

        public async Task<PlaygroundEntity> Create(CreatePlaygroundRequest createPlaygroundRequest)
        {
            try
            {
                if (createPlaygroundRequest is null) return null;

                //var rounds = new List<RoundEntity>();
                //foreach (var round in createPlaygroundRequest.Rounds)
                //{
                //    rounds.Add(new RoundEntity
                //    {
                //        RoundNumber = round.Key,
                //        Duration = round.Value,
                //        Progress = RoundState.Pending.Name
                //    });
                //}

                //for (int i = 1; i < createPlaygroundRequest.NumberOfTeam + 1; i++)
                //{
                //    var team = new TeamEntity
                //    {
                //        Id = ObjectId.GenerateNewId().ToString(),
                //        OwnerId = i,
                //        Tokens = createPlaygroundRequest.TeamToken,
                //        HostRoomCode = createPlaygroundRequest.HostRoomCode,
                //        PlayerRoomCode = createPlaygroundRequest.PlayerRoomCode,
                //        TeamStocks = createPlaygroundRequest.Shop.Select(p => new ProductEntity
                //        {
                //            ProductName = p.ProductName,
                //            Price = p.Price,
                //            RemainingStock = 0
                //        }).ToList(),
                //        CreationDate = DateTime.Now,
                //        ModifiedDate = null
                //    };

                //    await _unitOfWork.TeamRepository.Create(team);
                //    await _unitOfWork.Commit();
                //}

                //var shop = new ShopEntity
                //{
                //    Id = ObjectId.GenerateNewId().ToString(),
                //    HostRoomCode = createPlaygroundRequest.HostRoomCode,
                //    PlayerRoomCode = createPlaygroundRequest.PlayerRoomCode,
                //    Tokens = createPlaygroundRequest.ShopToken,
                //    ShopStocks = createPlaygroundRequest.Shop
                //};

                var playground = new PlaygroundEntity
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    HostRoomCode = createPlaygroundRequest.HostRoomCode,
                    PlayerRoomCode = createPlaygroundRequest.PlayerRoomCode,
                    //NumberOfTeam = createPlaygroundRequest.NumberOfTeam,
                    //TeamToken = createPlaygroundRequest.TeamToken,
                    Rounds = createPlaygroundRequest.Rounds,
                    CreationDate = DateTime.Now,
                    ModifiedDate = null
                };

                //await _unitOfWork.ShopRepository.Create(shop);
                await _unitOfWork.PlaygroundRepository.Create(playground);
                await _unitOfWork.Commit();

                return playground;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public async Task<PlaygroundEntity> GetPlayground(string roomcode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(roomcode)) return null;
                var playground = (await _unitOfWork.PlaygroundRepository.GetBy(p => p.HostRoomCode == roomcode)).FirstOrDefault();
                playground.Rounds = playground.Rounds?.OrderBy(n => n.RoundNumber).ToList();
                return playground;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<string> UpdatePlaygroundRoundStatus(UpdatePlaygroundRequest updatePlaygroundRequest)
        {
            try
            {
                if (updatePlaygroundRequest is null) return string.Empty;
                if (!string.IsNullOrWhiteSpace(updatePlaygroundRequest.HostId))
                    if (!Utils.IsValidObjectId(updatePlaygroundRequest.HostId))
                        return string.Empty;

                var existingPlayground = (await _unitOfWork.PlaygroundRepository.GetBy(w => w.HostRoomCode == updatePlaygroundRequest.HostRoomCode)).SingleOrDefault();

                if (existingPlayground is null || existingPlayground.Id != updatePlaygroundRequest.Id) return string.Empty;

                existingPlayground.Rounds = updatePlaygroundRequest.Rounds;
                existingPlayground.ModifiedDate = DateTime.Now;

                await _unitOfWork.PlaygroundRepository.Update(existingPlayground);
                await _unitOfWork.Commit();

                return $"[ID: {existingPlayground.Id}] Successfully Updated";
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public async Task<bool> UpdateRoundStatusToFinished(string roomCode, int roundNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(roomCode) || roundNumber <= 0) return false;

                var playground = (await _unitOfWork.PlaygroundRepository.GetBy(w => w.HostRoomCode == roomCode)).SingleOrDefault();
                playground.Rounds[roundNumber].Progress = RoundState.Finished.Name;
                playground.ModifiedDate = DateTime.Now;

                await _unitOfWork.PlaygroundRepository.Update(playground);
                await _unitOfWork.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
