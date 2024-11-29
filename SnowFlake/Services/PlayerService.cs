using MongoDB.Bson;
using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs;
using SnowFlake.Dtos.APIs.Player;
using SnowFlake.Dtos.APIs.Player.UpdatePlayer;
using SnowFlake.UnitOfWork;
using SnowFlake.Utilities;

namespace SnowFlake.Services;

public class PlayerService : IPlayerService
{
    private readonly IUnitOfWork _unitOfWork;
    public PlayerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<string> Create(CreatePlayerRequest createPlayerRequest)
    {
        if (createPlayerRequest is null) return string.Empty;
        if (!string.IsNullOrWhiteSpace(createPlayerRequest.TeamId))
            if (!Utils.IsValidObjectId(createPlayerRequest.TeamId))
                return string.Empty;

        var player = new PlayerEntity
        {
            Id = createPlayerRequest.Id.ToString(),
            Name = createPlayerRequest.Name,
            Email = createPlayerRequest.Email,
            TeamId = createPlayerRequest.TeamId,
            CreationDate = DateTime.Now,
            ModifiedDate = null
        };

        _unitOfWork.PlayerRepository.Create(player);
        _unitOfWork.Commit();

        return $"[Name: {player.Name}] Successfully Created";
    }

    public async Task<List<PlayerItem>> GetAll()
    {
        var players = _unitOfWork.PlayerRepository.GetAll().Take(50).Select(p => new PlayerItem
        {
            Id = p.Id,
            Name = p.Name,
            Email = p.Email,
            TeamId = p.TeamId,
            CreationDate = p.CreationDate,
            ModifiedDate = p.ModifiedDate
        }).ToList();

        return players;
    }

    public async Task<PlayerItem> GetByPlayerId(string playerId)
    {
        if (string.IsNullOrWhiteSpace(playerId)) return null;
        if (!Utils.IsValidObjectId(playerId)) return null;

        var player = _unitOfWork.PlayerRepository.GetBy(t => t.Id == playerId).Select(p => new PlayerItem
        {
            Id = p.Id,
            Name = p.Name,
            Email = p.Email,
            TeamId = p.TeamId,
            CreationDate = p.CreationDate,
            ModifiedDate = p.ModifiedDate
        }).FirstOrDefault()!;

        return player;
    }

    public async Task<List<PlayerItem>> GetPlayersByTeamId(string teamId)
    {
        if (string.IsNullOrWhiteSpace(teamId)) return null;
        if (!Utils.IsValidObjectId(teamId)) return null;

        var players = _unitOfWork.PlayerRepository.GetBy(t => t.TeamId == teamId).Select(p => new PlayerItem
        {
            Id = p.Id,
            Name = p.Name,
            Email = p.Email,
            TeamId = p.TeamId,
            CreationDate = p.CreationDate,
            ModifiedDate = p.ModifiedDate
        }).ToList()!;
        return players;
    }

    public async Task<string> Update(UpdatePlayerRequest updatePlayerRequest)
    {
        if (updatePlayerRequest is null) return string.Empty;
        if (!string.IsNullOrWhiteSpace(updatePlayerRequest.TeamId))
            if (!Utils.IsValidObjectId(updatePlayerRequest.TeamId))
                return string.Empty;

        var existingPlayer = _unitOfWork.PlayerRepository.GetBy(w => w.Id == updatePlayerRequest.Id).SingleOrDefault();

        if (existingPlayer is null || existingPlayer.Id != updatePlayerRequest.Id) return string.Empty;

        existingPlayer.Name = updatePlayerRequest.PlayerName;
        existingPlayer.Email = updatePlayerRequest.Email;
        existingPlayer.TeamId = updatePlayerRequest.TeamId;
        existingPlayer.ModifiedDate = DateTime.Now;

        _unitOfWork.PlayerRepository.Update(existingPlayer);
        _unitOfWork.Commit();

        return $"[ID: {existingPlayer.Id}][Name: {existingPlayer.Name}] Successfully Updated";
    }

    public async Task<string> Delete(string playerId)
    {
        if (string.IsNullOrWhiteSpace(playerId)) return string.Empty;
        if (!Utils.IsValidObjectId(playerId)) return string.Empty;

        var player = _unitOfWork.PlayerRepository.GetBy(w => w.Id == playerId).SingleOrDefault();

        if (player is null) return string.Empty;

        _unitOfWork.PlayerRepository.Delete(player);
        _unitOfWork.Commit();

        return $"[ID: {player.Id}][Name: {player.Name}] Successfully Deleted";
    }
}