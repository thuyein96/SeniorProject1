using Microsoft.AspNetCore.Mvc;
using SnowFlake.Dtos.APIs;
using SnowFlake.Dtos.APIs.Player.DeletePlayer;
using SnowFlake.Dtos.APIs.Player.GetPlayer;
using SnowFlake.Dtos.APIs.Player.GetPlayerList;
using SnowFlake.Dtos.APIs.Player.UpdatePlayer;
using SnowFlake.Services;

namespace SnowFlake.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PlayerController : ControllerBase
{
    private readonly IPlayerService _playerService;

    public PlayerController(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    [HttpPost]
    public async Task<IActionResult> Entry(CreatePlayerRequest request)
    {
        try
        {
            if (request == null) return BadRequest(new CreatePlayerResponse
            {
                Success = false,
                Message = null
            });

            var player = await _playerService.Create(request);
            if (player is null)
            {
                return NotFound(new CreatePlayerResponse
                {
                    Success = false,
                    Message = null
                });
            }
            return Ok(new CreatePlayerResponse
            {
                Success = true,
                Message = player
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }

    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var players = await _playerService.GetAll();
            if (players == null)
            {
                return NotFound(new GetPlayersResponse
                {
                    Success = false,
                    Message = null
                });
            }
            return Ok(new GetPlayersResponse
            {
                Success = true,
                Message = players
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{teamid}/search")]
    public async Task<IActionResult> GetTeamPlayers([FromQuery] string teamId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(teamId)) return BadRequest(new GetPlayersResponse
            {
                Success = false,
                Message = null
            });

            var players = await _playerService.GetPlayersByTeamId(teamId);

            if (players == null || players.Count == 0)
            {
                return NotFound(new GetPlayersResponse
                {
                    Success = false,
                    Message = null
                });
            }

            return Ok(new GetPlayersResponse
            {
                Success = true,
                Message = players
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{playerid}")]
    public async Task<IActionResult> GetPlayerAsync(string playerid)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(playerid)) return BadRequest();

            var player = await _playerService.GetByPlayerId(playerid);

            if (player == null) return NotFound(new GetPlayerResponse
            {
                Success = false,
                Message = null
            });
            return Ok(new GetPlayerResponse
            {
                Success = true,
                Message = player
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdatePlayerRequest request)
    {
        try
        {
            if (request == null) return BadRequest(new UpdatePlayerResponse
            {
                Success = false,
                Message = "Request body is empty."
            });

            var updateMessage = await _playerService.Update(request);

            if (string.IsNullOrWhiteSpace(updateMessage))
            {
                return NotFound(new UpdatePlayerResponse
                {
                    Success = false,
                    Message = $"[ID: {request.Id}] Failed to update player."
                });
            }
            return Ok(new UpdatePlayerResponse
            {
                Success = true,
                Message = updateMessage
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete("{playerid}")]
    public async Task<IActionResult> Delete(string playerid)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(playerid)) return BadRequest(new DeletePlayerResponse
            {
                Success = false,
                Message = "Input correct player Id."
            });

            var deleteMessage = await _playerService.Delete(playerid);

            if (string.IsNullOrWhiteSpace(deleteMessage)) return NotFound(new DeletePlayerResponse
            {
                Success = false,
                Message = $"[ID: {playerid}] Failed to delete player."
            });

            return Ok(new DeletePlayerResponse
            {
                Success = true,
                Message = deleteMessage
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}