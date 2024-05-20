using Microsoft.AspNetCore.Mvc;
using DrugScanner.Server.Models;
using DrugScanner.Server.Utils;

namespace DrugScanner.Server.Controllers.v1.Admin;

public partial class ReportsController : ControllerBase
{

  #region Methods

  [HttpGet]
  public async Task<ActionResult> Get([FromQuery] GetReportParams parameters)
  {
    try
    {
      parameters.Page ??= 1;
      parameters.PageSize ??= 50;

      var result = await reportsService.GetReports(parameters);

      var data = result.Data
      ?.Select(GetReport)
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
      var result = await reportsService.GetReports(new GetReportParams { Id = id });

      if (!result.Data.Any())
      {
        return NotFound(new { Status = 404, Message = "Not found" });
      }

      var data = GetReport(result.Data.First());

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

  private static GetReportResponse? GetReport(Report e)
  {
    var current = e.MutateObject<GetReportResponse>();

    return current;
  }

  public class GetReportResponse
  {
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? Address { get; set; }
    public DateTime DateModified { get; set; }
    public DateTime DateCreated { get; set; }
  }

  #endregion

}