using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs;
using SnowFlake.Dtos.APIs.Player;
using SnowFlake.Dtos.APIs.Player.UpdatePlayer;
using System.Runtime.InteropServices;
using PlayerItem = SnowFlake.Dtos.PlayerItem;

namespace SnowFlake.Services;

public interface IPlayerService
{
    //Create 
    Task<PlayerItem> Create(CreatePlayerRequest createPlayerRequest);
    //Reterive
    Task<List<Dtos.APIs.Player.PlayerItem>> GetAll();
    //GetById
    Task<Dtos.APIs.Player.PlayerItem> GetByPlayerId(string playerId);
    Task<PlayerItem> GetPlayerByName(string playerName, [Optional] string? teamId, [Optional] string? playerRoomCode);
    Task<PlayerItem> GetPlayerByRoomCode(string playerName, string roomCode);

    Task<List<Dtos.APIs.Player.PlayerItem>> GetPlayersByTeamId(string teamId);
    //Update 
    Task<string> Update(UpdatePlayerRequest updatePlayerRequest);
    //delete 
    Task<string> Delete(string playerId);
}