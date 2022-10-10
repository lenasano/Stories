using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

using Stories.Shared.Models;
using Stories.Server.DataAccess;

[ApiController]
[Route("api/[controller]")]
public class StoryFileController : ControllerBase
{
    private readonly IWebHostEnvironment env;
    private readonly ILogger<StoryFileController> logger;

    public StoryFileController(IWebHostEnvironment env, ILogger<StoryFileController> logger)
    {
        this.env    = env;
        this.logger = logger;
    }


    [HttpGet("{storyId}")]
    public async Task<IActionResult> GetStoryTextAsync(string storyId)
    {
        try
        {
            if (string.IsNullOrEmpty(storyId)) throw new InvalidOperationException();

            string text = await StoryFileAccessLayer.GetStoryTextFromFileAsync(storyId);

            return Ok(text);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<ActionResult> SaveFileAsync([FromBody] StoryModel story)
    {
        try
        {
            await StoryFileAccessLayer.CreateStoryFilesFromText(story.StoryId, story.FullText);

            return Ok(new object());
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}