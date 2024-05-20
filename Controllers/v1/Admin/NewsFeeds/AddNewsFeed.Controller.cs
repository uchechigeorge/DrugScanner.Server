using System.ComponentModel.DataAnnotations;
using DrugScanner.Server.Models;
using DrugScanner.Server.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DrugScanner.Server.Controllers.v1.Admin;

public partial class NewsFeedsController : ControllerBase
{

  [HttpPost]
  public async Task<IActionResult> Add([FromForm] AddNewsFeedBody body)
  {
    try
    {
      string imageUrl = await body.Image.WriteFile(new FileHelper.FileWriteOptions
      {
        DirectoryPath = ["newsfeeds"],
        ModFileName = $"newsfeed_{DateTime.UtcNow.ToFileTime()}",
      });

      var newsFeed = new NewsFeed
      {
        Description = body.Description,
        Title = body.Title,
        ImageUrl = imageUrl,
      };
      await dbContext.NewsFeeds.AddAsync(newsFeed);
      await dbContext.SaveChangesAsync();

      return CreatedAtAction(nameof(GetOne), new { newsFeed.Id }, new { Status = 201, Message = "Created", Data = GetNewsFeed(newsFeed) });
    }
    catch (Exception ex)
    {
      return StatusCode(500, new { Status = 500, ex.Message });
    }
  }

  public class AddNewsFeedBody
  {
    [Required]
    public string? Title { get; set; }
    [Required]
    public string? Description { get; set; }
    public string? Address { get; set; }
    public IFormFile? Image { get; set; }
  }

}