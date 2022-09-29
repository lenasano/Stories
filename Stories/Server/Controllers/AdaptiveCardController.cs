using Microsoft.AspNetCore.Mvc;
using System.IO;


[ApiController]
[Route("[controller]")]
public class AdaptiveCardController : ControllerBase
{


    [HttpGet]
    public IActionResult GetCardSchema(string cardtype)
    {
        // return the StoryCardSchema.json file as StreamContent
        // reference: https://stackoverflow.com/questions/9541351/returning-binary-file-from-controller-in-asp-net-web-api?rq=1

        string schemaPath = Path.Combine(
            Directory.GetCurrentDirectory(), $"{cardtype}CardSchema.json" // "WeatherCardSchema.json" //
        );

        FileStream stream = new FileStream(schemaPath, FileMode.Open, FileAccess.Read);

        return File(stream, "application/octet-stream");
    }
}