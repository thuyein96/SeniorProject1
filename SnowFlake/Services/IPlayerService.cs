using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs;
using SnowFlake.Dtos.APIs.Player;
using SnowFlake.Dtos.APIs.Player.UpdatePlayer;
using System.Runtime.InteropServices;

namespace SnowFlake.Services;

public interface IPlayerService
{
    //Create 
    Task<PlayerEntity> Create(CreatePlayerRequest createPlayerRequest);
    //Reterive
    Task<List<PlayerItem>> GetAll();
    //GetById
    Task<PlayerItem> GetByPlayerId(string playerId);
    Task<PlayerEntity> GetPlayerByName(string playerName, [Optional] string? teamId, [Optional] string? playerRoomCode);

    Task<List<PlayerItem>> GetPlayersByTeamId(string teamId);
    //Update 
    Task<string> Update(UpdatePlayerRequest updatePlayerRequest);
    //delete 
    Task<string> Delete(string playerId);
}