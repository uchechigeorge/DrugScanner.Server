using System.ComponentModel.DataAnnotations;
using DrugScanner.Server.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DrugScanner.Server.Controllers.v1.Admin;

public partial class NewsFeedsController : ControllerBase
{

  [HttpPatch("{id}")]
  public async Task<IActionResult> Update(int id, [FromBody] UpdateNewsFeedBody body)
  {
    try
    {
      var newsFeed = await dbContext.NewsFeeds.Where(e => e.Id == id).FirstOrDefaultAsync();
      if (newsFeed == null)
      {
        return BadRequest(new { Status = 400, Message = "Invalid news feed" });
      }

      newsFeed.Description = body.Description;
      newsFeed.Title = body.Title;

      await dbContext.SaveChangesAsync();

      return Ok(new { Status = 200, Message = "Ok" });
    }
    catch (Exception ex)
    {
      return StatusCode(500, new { Status = 500, ex.Message });
    }
  }

  [HttpPatch("{id}/image")]
  public async Task<IActionResult> UpdateImage(int id, [FromForm] UpdateNewsFeedImageBody body)
  {
    try
    {
      var newsFeed = await dbContext.NewsFeeds.Where(e => e.Id == id).FirstOrDefaultAsync();
      if (newsFeed == null)
      {
        return BadRequest(new { Status = 400, Message = "Invalid news feed" });
      }

      string imageUrl = await body.Image.WriteFile(new FileHelper.FileWriteOptions
      {
        DirectoryPath = ["newsfeeds"],
        ModFileName = $"newsfeed_{DateTime.UtcNow.ToFileTime()}",
      });

      newsFeed.ImageUrl = imageUrl;

      await dbContext.SaveChangesAsync();

      return Ok(new { Status = 200, Message = "Ok" });
    }
    catch (Exception ex)
    {
      return StatusCode(500, new { Status = 500, ex.Message });
    }
  }

  public class UpdateNewsFeedBody
  {
    [Required]
    public string? Title { get; set; }
    [Required]
    public string? Description { get; set; }
  }

  public class UpdateNewsFeedImageBody
  {
    public IFormFile? Image { get; set; }
  }

}