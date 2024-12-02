using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs;
using SnowFlake.Dtos.APIs.Playground;

namespace SnowFlake.Services;

public interface IPlaygroundService
{
    Task<PlaygroundEntity> Create(CreatePlaygroundRequest createPlaygroundRequest);
    Task<PlaygroundEntity> GetPlayground(string user, string roomcode);
}
