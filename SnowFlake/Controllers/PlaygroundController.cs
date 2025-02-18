using Microsoft.AspNetCore.Mvc;
using SnowFlake.Dtos.APIs.Playground;
using SnowFlake.Dtos.APIs.Playground.ConfigurePlayground;
using SnowFlake.Dtos.APIs.Playground.GetPlayground;
using SnowFlake.Managers;
using SnowFlake.Services;

namespace SnowFlake.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlaygroundController : ControllerBase
{
    private readonly IPlaygroundService _playgroundService;
    private readonly IPlaygroundManager _playgroundManager;

    public PlaygroundController(IPlaygroundService playgroundService,
        IPlaygroundManager playgroundManager)
    {
        _playgroundService = playgroundService;
        _playgroundManager = playgroundManager;
    }

    [HttpPost]
    public async Task<IActionResult> Entry(ConfigurePlaygroundRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.HostRoomCode) ||
                string.IsNullOrWhiteSpace(request.PlayerRoomCode))
            {
                return BadRequest("Require player or host room code.");
            }

            if (request.Rounds.Count <= 0) return BadRequest("Need to have at least 1 round.");

            var playgroundResponse = await _playgroundManager.SetupPlayground(request);

            if (playgroundResponse == null)
            {
                return NotFound(new CreatePlaygroundResponse
                {
                    Success = false,
                    Message = null
                });
            }
            return Ok(new CreatePlaygroundResponse
            {
                Success = true,
                Message = playgroundResponse
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("HostRoomCode")]
    public async Task<IActionResult> GetByRoomCode(string hostRoomCode)
    {
        try
        {
            var playgroundResponse = await _playgroundService.GetPlayground(hostRoomCode);
            if (playgroundResponse == null)
            {
                return NotFound(new GetPlaygroundByRoomCodeResponse
                {
                    Success = false,
                    Message = null
                });
            }
            return Ok(new GetPlaygroundByRoomCodeResponse
            {
                Success = true,
                Message = playgroundResponse
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}