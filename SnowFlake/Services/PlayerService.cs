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
    public async Task<PlayerEntity> Create(CreatePlayerRequest createPlayerRequest)
    {
        if (createPlayerRequest is null) return null;
        if (!string.IsNullOrWhiteSpace(createPlayerRequest.TeamId))
            if (!Utils.IsValidObjectId(createPlayerRequest.TeamId))
                return null;

        var player = new PlayerEntity
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = createPlayerRequest.Name,
            Email = createPlayerRequest.Email,
            TeamId = createPlayerRequest.TeamId,
            PlaygroundId = createPlayerRequest.PlaygroundId,
            CreationDate = DateTime.Now,
            ModifiedDate = null
        };

        await _unitOfWork.PlayerRepository.Create(player);
        await _unitOfWork.Commit();

        return player;
    }

    public async Task<List<PlayerItem>> GetAll()
    {
        var players = (await _unitOfWork.PlayerRepository.GetAll()).Take(Utils.BatchSize).Select(p => new PlayerItem
        {
            Id = p.Id,
            Name = p.Name,
            Email = p.Email,
            TeamId = p.TeamId,
            PlaygroundId = p.PlaygroundId,
            RoomCode = p.RoomCode,
            CreationDate = p.CreationDate,
            ModifiedDate = p.ModifiedDate
        }).ToList();

        return players;
    }

    public async Task<PlayerItem> GetByPlayerId(string playerId)
    {
        if (string.IsNullOrWhiteSpace(playerId)) return null;
        if (!Utils.IsValidObjectId(playerId)) return null;

        var player = (await _unitOfWork.PlayerRepository.GetBy(t => t.Id == playerId)).Select(p => new PlayerItem
        {
            Id = p.Id,
            Name = p.Name,
            Email = p.Email,
            TeamId = p.TeamId,
            PlaygroundId = p.PlaygroundId,
            RoomCode = p.RoomCode,
            CreationDate = p.CreationDate,
            ModifiedDate = p.ModifiedDate
        }).FirstOrDefault()!;

        return player;
    }

    public async Task<List<PlayerItem>> GetPlayersByTeamId(string teamId)
    {
        if (string.IsNullOrWhiteSpace(teamId)) return null;
        if (!Utils.IsValidObjectId(teamId)) return null;

        var players = (await _unitOfWork.PlayerRepository.GetBy(t => t.TeamId == teamId)).Select(p => new PlayerItem
        {
            Id = p.Id,
            Name = p.Name,
            Email = p.Email,
            TeamId = p.TeamId,
            PlaygroundId = p.PlaygroundId,
            RoomCode = p.RoomCode,
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

        var existingPlayer = (await _unitOfWork.PlayerRepository.GetBy(w => w.Id == updatePlayerRequest.Id)).SingleOrDefault();

        if (existingPlayer is null || existingPlayer.Id != updatePlayerRequest.Id) return string.Empty;

        existingPlayer.Name = updatePlayerRequest.PlayerName;
        existingPlayer.Email = updatePlayerRequest.Email;
        existingPlayer.TeamId = updatePlayerRequest.TeamId;
        existingPlayer.PlaygroundId = updatePlayerRequest.PlaygroundId;
        existingPlayer.RoomCode = updatePlayerRequest.RoomCode;
        existingPlayer.ModifiedDate = DateTime.Now;

        await _unitOfWork.PlayerRepository.Update(existingPlayer);
        await _unitOfWork.Commit();

        return $"[ID: {existingPlayer.Id}] Successfully Updated";
    }


    public async Task<string> Delete(string playerId)
    {
        if (string.IsNullOrWhiteSpace(playerId)) return string.Empty;
        if (!Utils.IsValidObjectId(playerId)) return string.Empty;

        var player = (await _unitOfWork.PlayerRepository.GetBy(w => w.Id == playerId)).SingleOrDefault();

        if (player is null) return string.Empty;

        await _unitOfWork.PlayerRepository.Delete(player);
        await _unitOfWork.Commit();

        return $"[ID: {player.Id}][Name: {player.Name}] Successfully Deleted";
    }
}