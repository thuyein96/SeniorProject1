using SnowFlake.Dtos.APIs.Playground.ConfigurePlayground;
using SnowFlake.Dtos;

namespace SnowFlake.Managers;

public interface IPlaygroundManager
{
    Task<PlaygroundEntity> SetupPlayground(ConfigurePlaygroundRequest configurePlaygroundRequest);
}