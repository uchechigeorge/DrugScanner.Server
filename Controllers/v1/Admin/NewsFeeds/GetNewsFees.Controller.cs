using Microsoft.AspNetCore.Mvc;
using DrugScanner.Server.Models;
using DrugScanner.Server.Utils;

namespace DrugScanner.Server.Controllers.v1.Admin;

public partial class NewsFeedsController : ControllerBase
{

  #region Methods

  [HttpGet]
  public async Task<ActionResult> Get([FromQuery] GetNewsFeedParams parameters)
  {
    try
    {
      parameters.Page ??= 1;
      parameters.PageSize ??= 50;

      var result = await newsFeedsService.GetNewsFeeds(parameters);

      var data = result.Data
      ?.Select(GetNewsFeed)
      .ToList()
      ;

      return Ok(new
      {
        Status = 200,
        Message = "Ok",
        Data = data ?? [],
        Meta = new
        {
          result.Total,
        }
      });
    }
    catch (Exception ex)
    {
      return StatusCode(500, new { Status = 500, ex.Message });
    }
  }

  [HttpGet("{id}")]
  public async Task<ActionResult> GetOne(int id)
  {
    try
    {
      var result = await newsFeedsService.GetNewsFeeds(new GetNewsFeedParams { Id = id });

      if (!result.Data.Any())
      {
        return NotFound(new { Status = 404, Message = "Not found" });
      }

      var data = GetNewsFeed(result.Data.First());

      return Ok(new
      {
        Status = 200,
        Message = "Ok",
        Data = data,
      });
    }
    catch (Exception ex)
    {
      return StatusCode(500, new { Status = 500, ex.Message });
    }
  }

  #endregion

  #region Helpers

  private static GetNewsFeedResponse? GetNewsFeed(NewsFeed e)
  {
    var current = e.MutateObject<GetNewsFeedResponse>();

    return current;
  }

  public class GetNewsFeedResponse
  {
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime DateModified { get; set; }
    public DateTime DateCreated { get; set; }
  }

  #endregion

}