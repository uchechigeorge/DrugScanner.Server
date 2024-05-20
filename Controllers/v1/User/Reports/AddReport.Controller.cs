using System.ComponentModel.DataAnnotations;
using DrugScanner.Server.Models;
using DrugScanner.Server.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DrugScanner.Server.Controllers.v1.User;

public partial class ReportsController : ControllerBase
{

  [HttpPost]
  public async Task<IActionResult> Add([FromForm] AddReportBody body)
  {
    try
    {
      string imageUrl = await body.Image.WriteFile(new FileHelper.FileWriteOptions
      {
        DirectoryPath = ["reports"],
        ModFileName = $"report_{DateTime.UtcNow.ToFileTime()}",
      });

      var report = new Report
      {
        Address = body.Address,
        Description = body.Description,
        Title = body.Title,
        ImageUrl = imageUrl,
      };
      await mDbContext.Reports.AddAsync(report);
      await mDbContext.SaveChangesAsync();

      // Send mail

      return StatusCode(201, new { Status = 201, Message = "Created" });
    }
    catch (Exception ex)
    {
      return StatusCode(500, new { Status = 500, ex.Message });
    }
  }

  public class AddReportBody
  {
    [Required]
    public string? Title { get; set; }
    [Required]
    public string? Description { get; set; }
    public string? Address { get; set; }
    public IFormFile? Image { get; set; }
  }

}