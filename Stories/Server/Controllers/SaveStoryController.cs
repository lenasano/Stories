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
public class SaveStoryController : ControllerBase
{
    private readonly IWebHostEnvironment env;
    private readonly ILogger<SaveStoryController> logger;

    private static string UPLOAD_FOLDERNAME = "unsafe_uploads";

    public SaveStoryController(IWebHostEnvironment env,
        ILogger<SaveStoryController> logger)
    {
        this.env = env;
        this.logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<IList<UploadedStory>>> SaveFileAsync()
    {
        try
        {
            string filecontent;
            using (System.IO.StreamReader reader = new System.IO.StreamReader(Request.Body, Encoding.UTF8))
            {
                filecontent = await reader.ReadToEndAsync();
            }

            await TextImageFileWriter.CreateImageFileFromText(filecontent, ImageFormat.Png);

            return Ok(new object());
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}