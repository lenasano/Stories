using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

using Stories.Shared;
using Stories.Shared.Models;
using Stories.Server.Helpers;

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

    [HttpPost]
    public async Task<ActionResult> SaveFileAsync([FromBody] StoryModel story)
    {
        try
        {
            await StoryFileWriter.CreateFilesFromText(story.StoryId, story.FullText);

            return Ok(new object());
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}