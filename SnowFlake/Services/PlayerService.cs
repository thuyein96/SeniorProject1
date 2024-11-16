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
                StudentId = createPlayerRequest.StudentId,
                Major = createPlayerRequest.Major,
                Faculty = createPlayerRequest.Faculty,
                FirebaseId = createPlayerRequest.FirebaseId,
                TeamId = createPlayerRequest.TeamId,
                ProfileImageUrl = createPlayerRequest.ProfileImageUrl,
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

    public GetPlayersResponse GetAll()
    {
        var response = new GetPlayersResponse
        {
            Players = new List<PlayerEntity>()
        };
        response.Players = _unitOfWork.PlayerRepository.GetAll().Take(50).ToList();
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
            StudentId = p.StudentId,
            Major = p.Major,
            Faculty = p.Faculty,
            FirebaseId = p.FirebaseId,
            TeamId = p.TeamId,
            ProfileImageUrl = p.ProfileImageUrl,
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
            Name = updatePlayerRequest.Name,
            Email = updatePlayerRequest.Email,
            StudentId = updatePlayerRequest.StudentId,
            Major = updatePlayerRequest.Major,
            Faculty = updatePlayerRequest.Faculty,
            FirebaseId = updatePlayerRequest.FirebaseId,
            TeamId = ObjectId.Parse(updatePlayerRequest.TeamId),
            ProfileImageUrl = updatePlayerRequest.ProfileImageUrl,
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