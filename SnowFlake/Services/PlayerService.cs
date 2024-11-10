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
    public void Create(CreatePlayerRequest createPlayerRequest)
    {
        if (createPlayerRequest is null) return;
        var player = new PlayerEntity
        {
            Id = Guid.NewGuid().ToString(),
            Name = createPlayerRequest.Name,
            Email = createPlayerRequest.Email,
            StudentId = createPlayerRequest.StudentId,
            Major = createPlayerRequest.Major,
            Faculty = createPlayerRequest.Faculty,
            TeamId = createPlayerRequest.TeamId,
            FirebaseId = createPlayerRequest.FireBaseId,
            ProfileImageUrl = createPlayerRequest.ProfileImageUrl,
            CreationDate = DateTime.Now,
            ModifiedDate = null
        };
        
        _unitOfWork.PlayerRepository.Create(player);
        _unitOfWork.Commit();
    }

    public GetPlayersResponse GetAll()
    {
        GetPlayersResponse response = new GetPlayersResponse
        {
            Players = new List<PlayerEntity>()
        };
        response.Players = _unitOfWork.PlayerRepository.GetAll().Select(p => new PlayerEntity
        {
            Id = p.Id,
            Name = p.Name,
            Email = p.Email,
            StudentId = p.StudentId,
            Major = p.Major,
            Faculty = p.Faculty,
            TeamId = p.TeamId,
            FirebaseId = p.FirebaseId,
            ProfileImageUrl = p.ProfileImageUrl,
            CreationDate = p.CreationDate,
            ModifiedDate = p.ModifiedDate
        }).ToList();
        return response;
    }

    public GetPlayerResponse GetById(string playerId)
    {
        if(string.IsNullOrWhiteSpace(playerId)) return null;
        
        return _unitOfWork.PlayerRepository.GetBy(t => t.Id == playerId).Select(p => new GetPlayerResponse
        {
            PlayerId = p.Id,
            Name = p.Name,
            Email = p.Email,
            StudentId = p.StudentId,
            Major = p.Major,
            Faculty = p.Faculty,
            TeamId = p.TeamId,
            FireBaseId = p.FirebaseId,
            ProfileImageUrl = p.ProfileImageUrl,
            CreatedAt = p.CreationDate,
            ModifiedOn = p.ModifiedDate
        }).FirstOrDefault();
    }

    public void Update(UpdatePlayerRequest updatePlayerRequest)
    {
        if(updatePlayerRequest is null) return;
        var player = new PlayerEntity
        {
            Id = updatePlayerRequest.PlayerId,
            Name = updatePlayerRequest.Name,
            Email = updatePlayerRequest.Email,
            StudentId = updatePlayerRequest.StudentId,
            Major = updatePlayerRequest.Major,
            Faculty = updatePlayerRequest.Faculty,
            TeamId = updatePlayerRequest.TeamId,
            FirebaseId = updatePlayerRequest.FireBaseId,
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
            var player = _unitOfWork.PlayerRepository.GetBy(w => w.Id == playerId).SingleOrDefault();
            
            if(player is null) return false;
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