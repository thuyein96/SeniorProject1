using Microsoft.AspNetCore.Mvc;
using SnowFlake.Dtos.APIs.Playground;
using SnowFlake.Dtos.APIs.Playground.GetPlayground;
using SnowFlake.Services;

namespace SnowFlake.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaygroundController : ControllerBase
    {
        private readonly IPlaygroundService _playgroundService;

        public PlaygroundController(IPlaygroundService playgroundService)
        {
            _playgroundService = playgroundService;
        }

        [HttpPost]
        public async Task<IActionResult> Entry(CreatePlaygroundRequest request)
        {
            try
            {
                var playgroundResponse = await _playgroundService.Create(request);

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

        [HttpGet]
        public async Task<IActionResult> GetByRoomCode(string user, string roomCode)
        {
            try
            {
                var playgroundResponse = await _playgroundService.GetPlayground(user, roomCode);
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
}
