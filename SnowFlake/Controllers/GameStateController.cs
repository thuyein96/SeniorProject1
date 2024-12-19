using Microsoft.AspNetCore.Mvc;
using SnowFlake.Dtos.APIs.GameState.CreateGameState;
using SnowFlake.Dtos.APIs.GameState.GetGameState;
using SnowFlake.Dtos.APIs.GameState.UpdateGameState;
using SnowFlake.Services;

namespace SnowFlake.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameStateController : ControllerBase
    {
        private readonly IGameStateService _gameStateService;

        public GameStateController(IGameStateService gameStateService)
        {
            _gameStateService = gameStateService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGameStateAsync(CreateGameStateRequest createGameStateRequest)
        {
            try
            {
                if (createGameStateRequest is null) return BadRequest(new CreateGameStateResponse
                {
                    Success = false,
                    Message = null
                });

                var gameState = await _gameStateService.AddGameState(createGameStateRequest);

                if (gameState is null)
                {
                    return NotFound(new CreateGameStateResponse
                    {
                        Success = true,
                        Message = null
                    });
                }

                return Ok(new CreateGameStateResponse
                {
                    Success = true,
                    Message = gameState
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpGet("{gamestateid}")]
        public async Task<IActionResult> GetGameStateAsync(GetGameStateRequest getGameStateRequest)
        {
            try
            {
                if (getGameStateRequest is null) return BadRequest(new GetGameStateResponse
                {
                    Success = false,
                    Message = null
                });

                var gameState = await _gameStateService.GetGameState(getGameStateRequest);

                if (gameState is null)
                {
                    return NotFound(new GetGameStateResponse
                    {
                        Success = false,
                        Message = null
                    });
                }
                return Ok(new GetGameStateResponse
                {
                    Success = true,
                    Message = gameState
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateGameStateAsync(UpdateGameStateRequest updateGameStateRequest)
        {
            try
            {
                if (updateGameStateRequest is null) return BadRequest(new UpdateGameStateResponse
                {
                    Success = false,
                    Message = null
                });

                var gameState = await _gameStateService.UpdateGameState(updateGameStateRequest);

                if (gameState is null)
                {
                    return NotFound(new UpdateGameStateResponse
                    {
                        Success = false,
                        Message = null,
                    });
                }

                return Ok(new UpdateGameStateResponse
                {
                    Success = true,
                    Message = gameState
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}
