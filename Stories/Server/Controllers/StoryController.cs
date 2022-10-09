using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stories.Shared.Models;
using Stories.Server.DataAccess;

namespace Stories.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoryController : Controller
    {
        StoryDataAccessLayer storyDataAccess = new ();

        [HttpGet]
        public Task<List<StoryModel>> Get()
        {
            return storyDataAccess.GetAllStories();
        }

        [HttpGet("{storyId}")]
        public Task<StoryModel> Get(string storyId)
        {
            return storyDataAccess.GetStoryData(storyId);
        }

        [HttpPost]
        public void Post([FromBody] StoryModel story)
        {
            storyDataAccess.AddStory(story);
        }

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
