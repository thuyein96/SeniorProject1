using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Team.CreateTeam;
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
    public IActionResult Entry(CreateTeamRequest createTeamRequest)
    {
        _teamService.Create(createTeamRequest);
        return NoContent();
    }
    
    [HttpGet]
    public IActionResult GetAll()
    {
        var teams = _teamService.GetAll();
        if (teams == null) return null;
        if (teams.Teams == null) return null;
        
        return teams.Teams.Count() == 0 ? (IActionResult) NotFound() : Ok(teams);
    }
    
    [HttpGet("{teamId}")]
    public IActionResult Edit(string teamId)
    {
        var team = _teamService.GetById(teamId);
        return team == null ? (IActionResult) NotFound() : Ok(team);
    }
    
    [HttpPut]
    public IActionResult Update(UpdateTeamRequest updateTeamRequest)
    {
        _teamService.Update(updateTeamRequest);
        return NoContent();
    }
    
    [HttpDelete("{teamId}")]
    public IActionResult Delete(string teamId)
    {
        var isDeleted = _teamService.Delete(teamId);
        return isDeleted ? (IActionResult) Ok() : NotFound();
    }
}