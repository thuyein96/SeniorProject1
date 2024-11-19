using SnowFlake.Dtos.APIs;
using SnowFlake.Dtos.APIs.Playground;

namespace SnowFlake.Services;

public interface IPlaygroundService
{
    bool Create(CreatePlaygroundRequest createPlaygroundRequest);
}
