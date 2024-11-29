using MongoDB.Bson;
using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs;
using SnowFlake.Dtos.APIs.Player;
using SnowFlake.Dtos.APIs.Player.GetPlayer;
using SnowFlake.Dtos.APIs.Player.GetPlayerList;
using SnowFlake.Dtos.APIs.Player.UpdatePlayer;

namespace SnowFlake.Services;

public interface IPlayerService
{
    //Create 
    Task<string> Create(CreatePlayerRequest createPlayerRequest);
    //Reterive
    Task<List<PlayerItem>> GetAll();
    //GetById
    Task<PlayerItem> GetByPlayerId(string playerId);

    Task<List<PlayerItem>> GetPlayersByTeamId(string teamId);
    //Update 
    Task<string> Update(UpdatePlayerRequest updatePlayerRequest);
    //delete 
    Task<string> Delete(string playerId);
}