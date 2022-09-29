using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Stories.Shared;

using System.Text;

[ApiController]
[Route("[controller]")]
public class SaveFileController : ControllerBase
{
    private readonly IWebHostEnvironment env;
    private readonly ILogger<SaveFileController> logger;

    private static string UPLOAD_FOLDERNAME = "unsafe_uploads";

    public SaveFileController(IWebHostEnvironment env,
        ILogger<SaveFileController> logger)
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

            string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), UPLOAD_FOLDERNAME);
            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);    // todo: make sure this dir has no execute permissions, and that files inherit this parent dir's permissions

            if (filecontent.Length > 0)
            {
                string fullPath = Path.Combine(uploadFolder, Path.GetRandomFileName());

                using (FileStream stream = new (fullPath, FileMode.Create))
                {
                    await stream.WriteAsync(Encoding.ASCII.GetBytes(filecontent));
                }
                return Ok(new object());
            }
            else
            {
                return BadRequest();
            }
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}