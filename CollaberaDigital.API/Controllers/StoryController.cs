using CollabraDigital.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CollabraDigital.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoryController : ControllerBase
    {
        private readonly IStoryService _storyService;
        private readonly ILogger<StoryController> _logger;
        public StoryController(IStoryService storyService, ILogger<StoryController> logger)
        {
                _storyService = storyService;
            _logger = logger;
        }



        [HttpGet("best-stories")]
        public async Task<IActionResult> GetBestStories(int n)
        {
            try
            {
                _logger.LogInformation($"Returning {n} best stories");
                if (n == 0)
                {
                    return BadRequest("n must be greater than 0");
                }
                var bestStories = await _storyService.GetTBestStoriesAsync(n);

                if (bestStories == null)
                {
                    return NotFound();
                }
                return Ok(bestStories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting best stories");
                return StatusCode(500);
            }

        }
    }
}