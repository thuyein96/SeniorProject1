using SnowFlake.Dtos.APIs;
using SnowFlake.Dtos.APIs.Player.GetPlayer;
using SnowFlake.Dtos.APIs.Player.GetPlayerList;
using SnowFlake.Dtos.APIs.Player.UpdatePlayer;

namespace SnowFlake.Services;

public class PlayerService : IPlayerService
{
    public Task<CreatePlayerResponse> Create(CreatePlayerRequest createPlayerRequest)
    {
        throw new NotImplementedException();
    }

    public Task<GetPlayerListResponse> GetAll(GetPlayerListRequest getPlayerListRequest)
    {
        throw new NotImplementedException();
    }

    public Task<GetPlayerResponse> GetById(string playerId)
    {
        throw new NotImplementedException();
    }

    public Task Update(UpdatePlayerRequest updatePlayerRequest)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(string Id)
    {
        throw new NotImplementedException();
    }
}