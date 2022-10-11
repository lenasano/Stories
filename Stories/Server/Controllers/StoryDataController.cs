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
        StoryDataAccessLayer storyDataAccess = new ();

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

        [HttpPatch("Views/{storyId}")]
        public async Task<int> IncrementStoryViewsAsync(string storyId)
        {
            return await storyDataAccess.IncrementStoryDataAsync(storyId, StoryIncrementValues.NumberOfViews);
        }

        [HttpPatch("Downloads/{storyId}")]
        public async Task<int> IncrementStoryDownloadsAsync(string storyId)
        {
            return await storyDataAccess.IncrementStoryDataAsync(storyId, StoryIncrementValues.NumberOfDownloads);
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
        

        /*
        [HttpPost]
        public async Task<HttpResponseMessage> PostStoryAsync([FromBody] StoryModel story)
        {
            try
            {
                string storyid = await storyDataAccess.AddStoryAsync(story) ?? string.Empty;

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        storyid,
                        Encoding.UTF8,
                        "text/plain"
                    )
                };
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }*/

        /*  not supported
        [HttpPut]
        public void Put([FromBody] Story story)
        {
            storyDataAccess.UpdateStory(story);
        }
        [HttpDelete("{storyId}")]
        public void Delete(string storyId)
        {
            storyDataAccess.DeleteEmployee(storyId);
        }*/


        /* generated
         
        // GET: StoryController
        public ActionResult Index()
        {
            return View();
        }

        // GET: StoryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StoryController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StoryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }*/
    }
}
