namespace CollabraDigital.Services.Contracts
{
    public interface IStoryService
    {
        Task<IEnumerable<StoryResponseDto>> GetTBestStoriesAsync(int n);
    }
}
