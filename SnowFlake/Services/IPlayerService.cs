using SnowFlake.Dtos.APIs;
using SnowFlake.Dtos.APIs.Player.GetPlayer;
using SnowFlake.Dtos.APIs.Player.GetPlayerList;
using SnowFlake.Dtos.APIs.Player.UpdatePlayer;

namespace SnowFlake.Services;

public interface IPlayerService
{
    //Create 
    Task<CreatePlayerResponse> Create(CreatePlayerRequest createPlayerRequest);
    //Reterive
    Task<GetPlayerListResponse> GetAll(GetPlayerListRequest getPlayerListRequest);
    //GetById
    Task<GetPlayerResponse> GetById(string playerId);
    //Update 
    Task Update(UpdatePlayerRequest updatePlayerRequest);
    //delete 
    Task<bool> Delete(string Id);
}