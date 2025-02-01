using MongoDB.Bson;
using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs;
using SnowFlake.Dtos.APIs.Player;
using SnowFlake.Dtos.APIs.Player.UpdatePlayer;
using SnowFlake.UnitOfWork;
using SnowFlake.Utilities;
using System.Runtime.InteropServices;

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
            TeamId = createPlayerRequest.TeamId,
            RoomCode = createPlayerRequest.RoomdCode,
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
            PlayerName = p.Name,
            TeamId = p.TeamId,
            PlayerRoomCode = p.RoomCode,
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
            PlayerName = p.Name,
            TeamId = p.TeamId,
            PlayerRoomCode = p.RoomCode,
            CreationDate = p.CreationDate,
            ModifiedDate = p.ModifiedDate
        }).FirstOrDefault()!;

        return player;
    }

    public async Task<PlayerEntity> GetPlayerByName(string playerName, [Optional] string? teamId, [Optional] string? playerRoomCode)
    {
        if (string.IsNullOrWhiteSpace(playerName)) return null;
        if (string.IsNullOrWhiteSpace(teamId) &&
            string.IsNullOrWhiteSpace(playerRoomCode))
        {
            return (await _unitOfWork.PlayerRepository.GetBy(p => p.Name == playerName && p.TeamId == null)).FirstOrDefault();
        }
        return (await _unitOfWork.PlayerRepository.GetBy(p => p.Name == playerName && p.TeamId == teamId && p.RoomCode == playerRoomCode)).FirstOrDefault();
    }

    public async Task<PlayerEntity> GetPlayerByRoomCode(string playerName, string roomCode)
    {
        if (string.IsNullOrWhiteSpace(playerName)) return null;
        if(string.IsNullOrWhiteSpace(roomCode)) return null;

        var player = (await _unitOfWork.PlayerRepository.GetBy(p => p.Name == playerName && p.RoomCode == roomCode)).FirstOrDefault();

        return player;
    }

    public async Task<List<PlayerItem>> GetPlayersByTeamId(string teamId)
    {
        if (string.IsNullOrWhiteSpace(teamId)) return null;
        if (!Utils.IsValidObjectId(teamId)) return null;

        var players = (await _unitOfWork.PlayerRepository.GetBy(t => t.TeamId == teamId)).Select(p => new PlayerItem
        {
            Id = p.Id,
            PlayerName = p.Name,
            TeamId = p.TeamId,
            PlayerRoomCode = p.RoomCode,
            CreationDate = p.CreationDate,
            ModifiedDate = p.ModifiedDate
        }).ToList()!;
        return players;
    }

    public async Task<string> Update(UpdatePlayerRequest updatePlayerRequest)
    {
        if (updatePlayerRequest is null) return string.Empty;
        //if (!string.IsNullOrWhiteSpace(updatePlayerRequest.OwnerId))
        //    if (!Utils.IsValidObjectId(updatePlayerRequest.OwnerId))
        //        return string.Empty;

        var existingPlayer = (await _unitOfWork.PlayerRepository.GetBy(w => w.Id == updatePlayerRequest.Id)).SingleOrDefault();

        if (existingPlayer is null || existingPlayer.Id != updatePlayerRequest.Id) return string.Empty;

        existingPlayer.Name = updatePlayerRequest.PlayerName;
        existingPlayer.TeamId = updatePlayerRequest.TeamId;
        existingPlayer.RoomCode = updatePlayerRequest.RoomCode;
        existingPlayer.ModifiedDate = DateTime.Now;

        await _unitOfWork.PlayerRepository.Update(existingPlayer);
        await _unitOfWork.Commit();

        return $"Player {existingPlayer.Name} is successfully updated";
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