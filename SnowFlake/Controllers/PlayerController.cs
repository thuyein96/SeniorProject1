using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using SnowFlake.Dtos.APIs;
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
    public IActionResult Entry(CreatePlayerRequest request)
    {
        _playerService.Create(request);
        return NoContent();
    }
    
    [HttpGet]
    public IActionResult GetAll()
    {
        var players = _playerService.GetAll();
        return players == null ? (IActionResult)NotFound() : Ok(players);
    }
    
    [HttpGet("search")]
    public IActionResult GetTeamPlayers([FromQuery] string? teamId)
    {
        var players = _playerService.GetPlayersByTeamId(teamId);
        return players == null ? (IActionResult)NotFound() : Ok(players);
    }

    [HttpGet("{playerId}")]
    public IActionResult Edit(string playerId)
    {
        var player = _playerService.GetByPlayerId(playerId);
        return player == null ? (IActionResult) NotFound() : Ok(player);
    }

    [HttpPut]
    public IActionResult Update(UpdatePlayerRequest request)
    {
        _playerService.Update(request);
        return NoContent();
    }

    [HttpDelete("{playerId}")]
    public IActionResult Delete(string playerId)
    {
        var isDeleted = _playerService.Delete(playerId);
        return isDeleted ? (IActionResult) Ok() : NotFound();
    }
}