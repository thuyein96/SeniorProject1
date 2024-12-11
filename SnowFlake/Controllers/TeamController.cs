using Microsoft.AspNetCore.Mvc;
using SnowFlake.Dtos.APIs.Team.CreateTeam;
using SnowFlake.Dtos.APIs.Team.DeleteTeam;
using SnowFlake.Dtos.APIs.Team.GetTeam;
using SnowFlake.Dtos.APIs.Team.GetTeams;
using SnowFlake.Dtos.APIs.Team.UpdateTeam;
using SnowFlake.Services;

namespace SnowFlake.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TeamController : ControllerBase
{
    private readonly ITeamService _teamService;

    public TeamController(ITeamService teamService)
    {
        _teamService = teamService;
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
            return NotFound(new GetTeamsResponse
            {
                Success = false,
                Message = null
            });
        }

        return Ok(new GetTeamsResponse
        {
            Success = true,
            Message = teams
        });
    }

    [HttpGet("{teamid}")]
    public async Task<IActionResult> Edit(string teamid)
    {
        if (string.IsNullOrWhiteSpace(teamid)) return BadRequest();

        var player = await _teamService.GetById(teamid);

        if (player == null)
        {
            return NotFound(new GetTeamResponse
            {
                Success = false,
                Message = null
            });
        }
        return Ok(new GetTeamResponse
        {
            Success = true,
            Message = player
        });
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateTeamRequest updateTeamRequest)
    {
        if (updateTeamRequest == null) return BadRequest();

        var updateMessage = await _teamService.Update(updateTeamRequest);

        if (updateMessage == string.Empty)
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

    [HttpDelete("{teamid}")]
    public async Task<IActionResult> Delete(string teamid)
    {
        if (string.IsNullOrWhiteSpace(teamid)) return BadRequest();

        var deleteMessage = await _teamService.Delete(teamid);

        if (deleteMessage == string.Empty) return NotFound(new DeleteTeamResponse
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