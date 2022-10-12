using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stories.Shared.Models;
using Stories.Server.DataAccess;
using System.Text;
using System.Net;

namespace Stories.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoryDataController : Controller
    {
        StoryDataAccessLayer storyDataAccess = new();

        [HttpGet]
        public async Task<List<StoryModel>> GetAllStoriesAsync()
        {
            return await storyDataAccess.GetAllStoriesAsync();
        }

        [HttpGet("{storyId}")]
        public async Task<StoryModel> GetStoryAsync(string storyId)
        {
            return await storyDataAccess.GetStoryAsync(storyId);
        }

        [HttpGet("ViewsDownloads/{StoryId}")]
        public async Task<StoryViewsDownloadsInfo> GetStoryViewsDownloadsAsync(string storyId)
        {
            return await storyDataAccess.GetStoryStatisticsAsync(storyId);
        }

        [HttpPatch("Views/{storyId}")]
        public async Task IncrementStoryViewsAsync(string storyId)
        {
            await storyDataAccess.IncrementStoryStatisticsAsync(storyId, StoryIncrementValues.NumberOfViews);
        }

        [HttpPatch("Downloads/{storyId}")]
        public async Task IncrementStoryDownloadsAsync(string storyId)
        {
            await storyDataAccess.IncrementStoryStatisticsAsync(storyId, StoryIncrementValues.NumberOfDownloads);
        }

        [HttpPost]
        public async Task<ActionResult> PostStoryAsync([FromBody] StoryModel story)
        {
            try
            {
                string storyid = await storyDataAccess.AddStoryAsync(story) ?? string.Empty;

                return Ok(storyid);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
