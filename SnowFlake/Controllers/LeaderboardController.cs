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

    [HttpGet]
    public async Task<IActionResult> GetLeaderboardByHostRoomCode(string? hostRoomCode, string? playerRoomCode)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(hostRoomCode) && string.IsNullOrWhiteSpace(playerRoomCode))
            {
                return BadRequest("Require player or host room code.");
            }

            var teamsRank = await _leaderboardManager.GetLeaderboard(hostRoomCode, playerRoomCode);

            return teamsRank.Success ? Ok(teamsRank) : NotFound(teamsRank);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost("{hostRoomCode}")]
    public async Task<IActionResult> CreateLeaderboard(string? hostRoomCode, string? playerRoomCode)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(hostRoomCode) && string.IsNullOrWhiteSpace(playerRoomCode))
            {
                return BadRequest("Require player or host room code.");
            }

            var teamsRank = await _leaderboardManager.CreateLeaderboard(hostRoomCode, playerRoomCode);
            
            return teamsRank.Success ? Ok(teamsRank): NotFound(teamsRank);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}