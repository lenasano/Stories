using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Stories.Shared;

using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Drawing.Drawing2D;

using System.Linq;

[ApiController]
[Route("[controller]")]
public class SaveFileController : ControllerBase
{
    private readonly IWebHostEnvironment env;
    private readonly ILogger<SaveFileController> logger;

    private static string UPLOAD_FOLDERNAME = "unsafe_uploads";

    private static int IMAGE_WIDTH = 120;

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

                using (FileStream stream = new(fullPath, FileMode.Create))
                {
                    await stream.WriteAsync(Encoding.ASCII.GetBytes(filecontent));
                }

                SaveTextAsImage(filecontent, ImageFormat.Png);

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

    public static void SaveTextAsImage(string text, ImageFormat f)
    {
        text = "The purpose of this challenge is to gather further evidence of the true abilities and qualifications of potential candidates for employment in Crawford Technologies software engineering division than would normally be possible via a simple interview. \nThe prospective employee is asked to take the enclosed project requirements and return to Crawford a functionally complete, working piece of software, for review by Crawford Technologies Management and Technical staff.Crawford’s staff will use this to evaluate the design decisions made by the prospective employee, as well as the craftsmanship and quality of the code and the project returned.This project will have a significant impact on the applicant selection process.Please take this opportunity to demonstrate for Crawford Technologies your skills in software engineering".Trim();


        Bitmap bitmap = new (1, 1);

        // Create a graphics object to measure the text's width and height.
        Graphics g = Graphics.FromImage(bitmap);


        // Set Background color
        g.Clear(Color.White);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = TextRenderingHint.AntiAlias;

        // Create the Font object for the image text drawing.
        Font font = new("Calibri Light", 20, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);

        // Add text wrapping to the string.
        string wrappedString = WrapTextWithGraphics( g, text + ' ', IMAGE_WIDTH, font);

        // Measure string.
        SizeF stringSize = g.MeasureString(wrappedString, font);

        // Create the bmpImage again with the correct size for the text and font.
        bitmap = new Bitmap(bitmap, Size.Round(stringSize) );
        g = Graphics.FromImage(bitmap);

        // Draw rectangle representing size of string.
        g.DrawRectangle(new Pen(Color.Azure, 1), 0.0F, 0.0F, stringSize.Width, stringSize.Height);

        // Draw string to screen.
        g.DrawString(wrappedString, font, Brushes.Black, new PointF(0, 0));

        g.Flush();
        // ? g.Dispose();

        string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".png";

        bitmap.Save( Path.Combine( Environment.CurrentDirectory, UPLOAD_FOLDERNAME, fileName), f);
    }

    /// <summary>
    /// Applies word wrapping to a string, respecing word boundaries, if possible. The width is in pixels (For a font size 20, each character is about 7 pixels)
    /// 
    /// Example,
    /// string result = SaveFileController.WrapTextWithGraphics(
    ///     Graphics.FromImage(new Bitmap(1,1)), 
    ///     "Hello this is a very long string, how long can it go? No one can say... okay, let's finish here. ", 
    ///     300, 
    ///     new Font("Calibri Light", 20, FontStyle.Regular, GraphicsUnit.Pixel)
    /// );
    ///
    /// result is "Hello this is a very long string, how \r\nlong can it go? No one can say... \r\nokay, let's finish here. "
    /// </summary>
    /// 
    /// <param name="g">A graphics object, used for measuring line lengths</param>
    /// <param name="original">The string to wrap. This string must end with a space (' ') or newline.</param>
    /// <param name="width">The desired line width in pixels</param>
    /// <param name="font">The font to be applied to the text</param>
    /// <param name="wrappedLines">Used for recursive steps. The incremental list of lines of wrapped text.</param>
    /// <param name="start">Used for recursive steps. The character position for the current iteration.</param>
    /// 
    /// <returns>The string with <see cref="Environment.NewLine"/> inserted.</returns>
    
    public static string WrapTextWithGraphics(in Graphics g, string original, in int width, in Font font, List<string> wrappedLines = null, int start = 0) 
    {
        if (wrappedLines is null) wrappedLines = new();                 // happens in the first iteration
        if (original is null)     original = string.Empty;              // this might happen in the first iteration, if a null string is passed in on the first call to this function

        //  _base case_:  empty string or reached the end

        if ( start + 1 >= original.Length ) return string.Join(Environment.NewLine, wrappedLines);



        // estimate the end position for the current line (for a size 20 font, divide by 7)
        int end = start + (width / 7);                                  // overestinate the number of chars in the next line of text

        end = end < original.Length ? end : original.Length - 1;        // in the last iteration, we need to adjust in case we are over the string length

        // adjust for word boundaries
        end = original.LastWordBoundaryBefore(end, start);

        // find the end position of a space that fits the width
        for
        (
            ; 
            g.MeasureString(original.Substring(start, end - start + 1), font).Width > width;
            end = original.LastWordBoundaryBefore( end - 1, start )
        );

        wrappedLines.Add( original.Substring(start, end - start + 1) );

        return WrapTextWithGraphics(g, original, width, font, wrappedLines, end + 1);
    }
}

public static class StringExtensions
{
    public static int LastWordBoundaryBefore(this string str, int end, int min)
    {
        int firstNewLine = str.Substring(min, end - min + 1).IndexOf(Environment.NewLine);
        if (firstNewLine != -1) return min + firstNewLine + Environment.NewLine.Length - 1;                      // there is a newline in the string segment (adds one to account for two characters in windows (\r\n) if needed)

        int lastSpace = str.Substring(min, end - min + 1).LastIndexOf(' ');
        if (lastSpace != -1) return min + lastSpace;                          // return the position of the last space character (we don't handle other types of whitespace)

        return end - 1;                                                 // there are no word breaks in the string segment, so reduce the range by one character (so that the width can be retested in the calling function)
    }
}