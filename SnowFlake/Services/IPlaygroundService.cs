using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Playground;
using SnowFlake.Dtos.APIs.Playground.UpdatePlayground;

namespace SnowFlake.Services;

public interface IPlaygroundService
{
    Task<PlaygroundEntity> Create(CreatePlaygroundRequest createPlaygroundRequest);
    Task<PlaygroundEntity> GetPlayground(string roomcode);
    Task<string> UpdatePlaygroundRoundStatus(UpdatePlaygroundRequest updatePlaygroundRequest);
    Task<bool> UpdateRoundStatusToFinished(string roomCode, int roundNumber);
}
