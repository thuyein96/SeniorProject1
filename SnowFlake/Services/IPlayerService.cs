using MongoDB.Bson;
using SnowFlake.Dtos.APIs;
using SnowFlake.Dtos.APIs.Player.GetPlayer;
using SnowFlake.Dtos.APIs.Player.GetPlayerList;
using SnowFlake.Dtos.APIs.Player.UpdatePlayer;

namespace SnowFlake.Services;

public interface IPlayerService
{
    //Create 
    bool Create(CreatePlayerRequest createPlayerRequest);
    //Reterive
    GetPlayersResponse GetAll();
    //GetById
    GetPlayerResponse GetByPlayerId(string playerId);

    GetPlayersResponse GetPlayersByTeamId(string teamId);
    //Update 
    void Update(UpdatePlayerRequest updatePlayerRequest);
    //delete 
    bool Delete(string playerId);
}