using Microsoft.AspNetCore.Mvc;
using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Team.CreateTeam;
using SnowFlake.Dtos.APIs.Team.DeleteTeam;
using SnowFlake.Dtos.APIs.Team.GetTeam;
using SnowFlake.Dtos.APIs.Team.GetTeams;
using SnowFlake.Dtos.APIs.Team.GetTeamsByRoomCode;
using SnowFlake.Dtos.APIs.Team.SearchPlayerInTeam;
using SnowFlake.Dtos.APIs.Team.UpdateTeam;
using SnowFlake.Managers;
using SnowFlake.Services;

namespace SnowFlake.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TeamController : ControllerBase
{
    private readonly ITeamService _teamService;
    private readonly ITeamManager _teamManager;

    public TeamController(ITeamService teamService,
                          ITeamManager teamManager)
    {
        _teamService = teamService;
        _teamManager = teamManager;
    }

    [HttpPost]
    public async Task<IActionResult> Entry(CreateTeamRequest createTeamRequest)
    {
        try
        {
            if (createTeamRequest == null)
            {
                return BadRequest();
            }
            var team = await _teamService.Create(createTeamRequest);

            if (team == null)
            {
                return NotFound(new CreateTeamResponse
                {
                    Success = false,
                    Message = null
                });
            }

            return Ok(new CreateTeamResponse
            {
                Success = true,
                Message = team
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
        var teams = await _teamService.GetAll();
        if (teams == null || teams.Count == 0)
        {
            return base.NotFound(new GetTeamsResponse
            {
                Success = false,
                Message = null
            });
        }

        return base.Ok(new GetTeamsResponse
        {
            Success = true,
            Message = teams
        });
    }

    [HttpGet("teamdetails")]
    public async Task<IActionResult> GetTeam([FromQuery] int teamNumber, [FromQuery] string? playerRoomCode, [FromQuery] string? hostRoomCode)
    {
        if (teamNumber <= 0) return BadRequest();
        var team = await _teamService.GetTeam(teamNumber, playerRoomCode, hostRoomCode);
        if (team == null)
        {
            return base.NotFound(new GetTeamResponse
            {
                Success = false,
                Message = null
            });
        }
        return base.Ok(new GetTeamResponse
        {
            Success = true,
            Message = team
        });
    }

    [HttpGet("search")]
    public async Task<IActionResult> GetTeamByRoomCode([FromQuery] string? hostRoomCode, [FromQuery] string? playerRoomCode)
    {
        if (string.IsNullOrWhiteSpace(hostRoomCode) && string.IsNullOrWhiteSpace(playerRoomCode)) return BadRequest();

        var getTeamByRoomCodeRequest = new GetTeamsByRoomCodeRequest
        {
            HostRoomCode = hostRoomCode,
            PlayerRoomCode = playerRoomCode
        };

        var teams = await _teamManager.GetTeamWithProducts(getTeamByRoomCodeRequest);

        if (teams == null)
        {
            return base.NotFound(new GetTeamsByRoomCodeResponse
            {
                Success = false,
                Message = null
            });
        }
        return base.Ok(new GetTeamsByRoomCodeResponse
        {
            Success = true,
            Message = teams
        });
    }

    [HttpGet("players/search")]
    public async Task<IActionResult> SearchPlayer([FromQuery] string? playerRoomCode, [FromQuery] string? playerName)
    {
        if (string.IsNullOrWhiteSpace(playerRoomCode) || string.IsNullOrWhiteSpace(playerName)) return BadRequest();
        var searchPlayerRequest = new SearchPlayerRequest
        {
            PlayerRoomCode = playerRoomCode,
            PlayerName = playerName
        };
        var player = await _teamManager.IsTeamHasPlayer(searchPlayerRequest);

        if (string.IsNullOrWhiteSpace(player))
        {
            return NotFound(new SearchPlayerResponse
            {
                Success = false,
                Message = "Player not found."
            });
        }
        return Ok(new SearchPlayerResponse
        {
            Success = true,
            Message = player
        });
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateTeamRequest updateTeamRequest)
    {
        if (updateTeamRequest == null) return BadRequest(new UpdateTeamResponse
        {
            Success = false,
            Message = "Incorrect request body format."
        });

        var updateMessage = await _teamService.Update(updateTeamRequest);

        if (string.IsNullOrWhiteSpace(updateMessage))
        {
            return NotFound(new UpdateTeamResponse
            {
                Success = false,
                Message = updateMessage
            });
        }

        return Ok(new UpdateTeamResponse
        {
            Success = true,
            Message = updateMessage
        });
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(DeleteTeamRequest deleteTeamRequest)
    {
        if (deleteTeamRequest is null) return BadRequest();

        var deleteMessage = await _teamService.Delete(deleteTeamRequest);

        if (string.IsNullOrWhiteSpace(deleteMessage)) return NotFound(new DeleteTeamResponse
        {
            Success = false,
            Message = deleteMessage
        });

        return Ok(new DeleteTeamResponse
        {
            Success = true,
            Message = deleteMessage
        });
    }
}