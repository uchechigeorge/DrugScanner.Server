using Microsoft.AspNetCore.Mvc;
using DrugScanner.Server.Models;
using DrugScanner.Server.Utils;

namespace DrugScanner.Server.Controllers.v1.Admin;

public partial class DrugsController : ControllerBase
{

  #region Methods

  [HttpGet]
  public async Task<ActionResult> Get([FromQuery] GetDrugParams parameters)
  {
    try
    {
      parameters.Page ??= 1;
      parameters.PageSize ??= 50;

      var result = await drugsService.GetDrugs(parameters);

      var data = result.Data
      ?.Select(GetDrug)
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
      var result = await drugsService.GetDrugs(new GetDrugParams { Id = id });

      if (!result.Data.Any())
      {
        return NotFound(new { Status = 404, Message = "Not found" });
      }

      var data = GetDrug(result.Data.First());

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

  private static GetDrugResponse? GetDrug(Drug e)
  {
    var current = e.MutateObject<GetDrugResponse>();

    return current;
  }

  public class GetDrugResponse
  {
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? BatchNo { get; set; }
    public bool IsScanned { get; set; }
    public int NoOfScans { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public string? ManufacturedBy { get; set; }
    public DateTime? ManufacturingDate { get; set; }
    public DateTime DateModified { get; set; }
    public DateTime DateCreated { get; set; }
  }

  #endregion

}