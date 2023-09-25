using CollabraDigital.Services.Models;
using System.Net.Http.Json;

namespace CollabraDigital.Services.Contracts
{
    public class StoryService : IStoryService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public StoryService(IHttpClientFactory httpClientFactory)
        {
                _httpClientFactory = httpClientFactory;
        }
        public async Task<IEnumerable<StoryResponseDto>> GetTBestStoriesAsync(int n)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://hacker-news.firebaseio.com/v0/beststories.json");
            var storyIds =  await response.Content.ReadFromJsonAsync<IEnumerable<int>>();

            if(!response.IsSuccessStatusCode)
            {
                return default;
            }
            var stories = new List<StoryResponseDto>();
            foreach (var storyId in storyIds.Take(n))
            {
                var storyResponse = await client.GetAsync($"https://hacker-news.firebaseio.com/v0/item/{storyId}.json");
                if (!storyResponse.IsSuccessStatusCode)
                {
                    return null;
                }
                var story = await storyResponse.Content.ReadFromJsonAsync<Story>();

                stories.Add(new StoryResponseDto
                {
                    Title = story.Title,
                    Uri = story.Url,
                    PostedBy = story.By,
                    Time = DateTimeOffset.FromUnixTimeSeconds(story.Time).DateTime,
                    Score = story.Score,
                    CommentCount = story.Kids.Count
                });
            }
           return stories.OrderByDescending(s => s.Score).ToList();

        }
    }
}
