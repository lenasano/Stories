using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Stories.Shared;
using Stories.Server.Helpers;

using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Drawing.Drawing2D;

using System.Linq;
using System.Text.RegularExpressions;

[ApiController]
[Route("[controller]")]
public class StoryFileController : ControllerBase
{
    private readonly IWebHostEnvironment env;
    private readonly ILogger<StoryFileController> logger;

    public StoryFileController(IWebHostEnvironment env,
        ILogger<StoryFileController> logger)
    {
        this.env = env;
        this.logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<IList<UploadedStory>>> SaveFileAsync()
    {
        try
        {
            await StoryFileWriter.CreateFilesFromText(Request.Body, Encoding.UTF8);
            return Ok(new object());
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}