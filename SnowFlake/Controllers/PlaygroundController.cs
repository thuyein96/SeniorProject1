using Microsoft.AspNetCore.Mvc;
using SnowFlake.Dtos.APIs;
using SnowFlake.Dtos.APIs.Playground;
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
        public IActionResult Entry(CreatePlaygroundRequest request)
        {
            _playgroundService.Create(request);
            return NoContent();
        }

        [HttpPost("start-timer")]
        public IActionResult StartTimer(int durationInSeconds)
        {
            _playgroundService.StartTimer(durationInSeconds);
            return Ok("Timer completed");
        }
    }
}
