using MongoDB.Bson;
using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs;
using SnowFlake.Dtos.APIs.Player.GetPlayer;
using SnowFlake.Dtos.APIs.Player.GetPlayerList;
using SnowFlake.Dtos.APIs.Player.UpdatePlayer;
using SnowFlake.UnitOfWork;

namespace SnowFlake.Services;

public class PlayerService : IPlayerService
{
    private readonly IUnitOfWork _unitOfWork;
    public PlayerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public bool Create(CreatePlayerRequest createPlayerRequest)
    {
        try
        {
            if (createPlayerRequest is null) return false;

            var player = new PlayerEntity
            {
                Id = createPlayerRequest.Id.ToString(),
                Name = createPlayerRequest.Name,
                Email = createPlayerRequest.Email,
                FirebaseId = createPlayerRequest.FirebaseId,
                TeamId = ObjectId.Parse(createPlayerRequest.TeamId),
                CreationDate = DateTime.Now,
                ModifiedDate = null
            };

            _unitOfWork.PlayerRepository.Create(player);
            _unitOfWork.Commit();
            return true;
        }
        catch (Exception)
        {

            return false;
        }
    }

    //public GetPlayersResponse GetAll()
    //{
    //    var response = new GetPlayersResponse
    //    {
    //        Players = new List<GetPlayerResponse>()
    //    };
    //    response.Players = _unitOfWork.PlayerRepository.GetAll().Take(50).Select(p => new GetPlayerResponse
    //    {
    //        Id = p.Id,
    //        Name = p.Name,
    //        Email = p.Email,
    //        FirebaseId = p.FirebaseId,
    //        TeamId = p.TeamId.ToString(),
    //        CreatedAt = p.CreationDate,
    //        ModifiedAt = p.ModifiedDate
    //    }).ToList();
    //    return response;
    //}

    public GetPlayersResponse GetAll(string? teamId)
    {
        var response = new GetPlayersResponse
        {
            Players = new List<GetPlayerResponse>()
        };

        if(teamId is not null)
        {
            response.Players = _unitOfWork.PlayerRepository.GetAll().Where(p => p.TeamId == ObjectId.Parse(teamId)).Select(p => new GetPlayerResponse
            {
                Id = p.Id,
                Name = p.Name,
                Email = p.Email,
                FirebaseId = p.FirebaseId,
                TeamId = p.TeamId.ToString(),
                CreatedAt = p.CreationDate,
                ModifiedAt = p.ModifiedDate
            }).ToList();
        }

        response.Players = _unitOfWork.PlayerRepository.GetAll().Take(50).Select(p => new GetPlayerResponse
        {
            Id = p.Id,
            Name = p.Name,
            Email = p.Email,
            FirebaseId = p.FirebaseId,
            TeamId = p.TeamId.ToString(),
            CreatedAt = p.CreationDate,
            ModifiedAt = p.ModifiedDate
        }).ToList();

        return response;
    }

    public GetPlayerResponse GetById(string playerId)
    {
        if (string.IsNullOrWhiteSpace(playerId)) return null;

        var player = _unitOfWork.PlayerRepository.GetBy(t => t.Id == playerId).Select(p => new GetPlayerResponse
        {
            Id = p.Id,
            Name = p.Name,
            Email = p.Email,
            FirebaseId = p.FirebaseId,
            TeamId = p.TeamId.ToString(),
            CreatedAt = p.CreationDate,
            ModifiedAt = p.ModifiedDate
        }).FirstOrDefault()!;
        return player;
    }

    public void Update(UpdatePlayerRequest updatePlayerRequest)
    {
        if (updatePlayerRequest is null) return;

        var player = new PlayerEntity
        {
            Id = updatePlayerRequest.Id.ToString(),
            Name = updatePlayerRequest.PlayerName,
            Email = updatePlayerRequest.Email,
            FirebaseId = updatePlayerRequest.FirebaseId,
            TeamId = ObjectId.Parse(updatePlayerRequest.TeamId),
            CreationDate = updatePlayerRequest.CreatedAt,
            ModifiedDate = DateTime.Now
        };

        _unitOfWork.PlayerRepository.Update(player);
        _unitOfWork.Commit();
    }

    public bool Delete(string playerId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(playerId)) return false;

            var player = _unitOfWork.PlayerRepository.GetBy(w => w.Id == playerId).SingleOrDefault();

            if (player is null) return false;
            _unitOfWork.PlayerRepository.Delete(player);
            _unitOfWork.Commit();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}