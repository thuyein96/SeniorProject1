using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SnowFlake.Dtos.APIs.Leaderboard.CreateLeaderboard;
using SnowFlake.Dtos.APIs.Leaderboard.GetLeaderboard;
using SnowFlake.Managers;

namespace SnowFlake.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LeaderboardController : ControllerBase
{
    private readonly ILeaderboardManager _leaderboardManager;

    public LeaderboardController(ILeaderboardManager leaderboardManager)
    {
        _leaderboardManager = leaderboardManager;
    }

    [HttpGet("{hostRoomCode}")]
    public async Task<IActionResult> GetLeaderboardByHostRoomCode(string hostRoomCode)
    {
        try
        {
            if (string.IsNullOrEmpty(hostRoomCode))
            {
                return BadRequest();
            }

            var leaderboard = await _leaderboardManager.GetLeaderboardByHostRoomCode(hostRoomCode); 
            if (leaderboard == null)
            {
                return NotFound(new GetLeaderboardResponse
                {
                    Success = false,
                    Message = null
                });
            }
            return Ok(new GetLeaderboardResponse
            {
                Success = true,
                Message = leaderboard
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost("{hostRoomCode}")]
    public async Task<IActionResult> CreateLeaderboard(string hostRoomCode)
    {
        try
        {
            if (string.IsNullOrEmpty(hostRoomCode))
            {
                return BadRequest();
            }

            var leaderboard = await _leaderboardManager.CreateLeaderboard(hostRoomCode);

            if (leaderboard.Count <= 0 || leaderboard is null)
            {
                return NotFound(new CreateLeaderboardResponse
                {
                    Success = false,
                    Message = null
                });
            }
            return Ok(new CreateLeaderboardResponse
            {
                Success = true,
                Message = leaderboard
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}