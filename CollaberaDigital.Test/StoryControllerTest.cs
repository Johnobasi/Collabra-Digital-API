using CollabraDigital.API.Controllers;
using CollabraDigital.Services;
using CollabraDigital.Services.Contracts;
using CollabraDigital.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace CollabraDigital.Test
{
    public class StoryControllerTest
    {
        private readonly Mock<IStoryService> _storyServiceMock;
        private readonly StoryController _storyController;
        private readonly Mock<ILogger<StoryController>> _loggerMock;
        public StoryControllerTest()
        {
             _loggerMock = new Mock<ILogger<StoryController>>();
            _storyServiceMock = new Mock<IStoryService>();
            _storyController = new StoryController(_storyServiceMock.Object, _loggerMock.Object);
        }
        [Fact]
        public void GetBestStories_ValidRequest_ReturnsOkResult()
        {
          
            _storyServiceMock.Setup(s => s.GetTBestStoriesAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<IEnumerable<StoryResponseDto>>(new List<StoryResponseDto>()));


            var result =  _storyController.GetBestStories(2);

 
            var okResult = Assert.IsType<OkObjectResult>(result);
            var stories = Assert.IsAssignableFrom<IEnumerable<Story>>(okResult.Value);
            Assert.NotNull(stories);
        }

        [Fact]
        public async Task GetBestStories_InvalidRequest_ReturnsBadRequest()
        {
            var result = await _storyController.GetBestStories(0);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetBestStories_ServiceReturnsNull_ReturnsNotFound()
        {
            _storyServiceMock.Setup(service => service.GetTBestStoriesAsync(It.IsAny<int>()))
                  .Returns(Task.FromResult<IEnumerable<StoryResponseDto>>(new List<StoryResponseDto>()));

            var result = await _storyController.GetBestStories(5);


            Assert.IsType<NotFoundResult>(result);
        }
    }


}


