using System.ComponentModel.DataAnnotations;
using DrugScanner.Server.Models;
using DrugScanner.Server.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DrugScanner.Server.Controllers.v1.User;

public partial class DrugsController : ControllerBase
{

  [HttpPost("verify")]
  public async Task<IActionResult> Verify([FromBody] VerifyDrugBody body)
  {
    try
    {
      var drug = await mDbContext.Drugs.Where(e => e.Code == body.Code).FirstOrDefaultAsync();
      if (drug == null)
      {
        return BadRequest(new { Status = 400, Message = "The Drug is Fake or You scanned an Invalid QR Code." });
      }

      drug.IsScanned = true;
      drug.NoOfScans++;

      await mDbContext.SaveChangesAsync();

      return Ok(new { Status = 200, Message = "Ok", Data = GetDrug(drug) });
    }
    catch (Exception ex)
    {
      return StatusCode(500, new { Status = 500, ex.Message });
    }
  }

  #region Helpers

  public class VerifyDrugBody
  {
    [Required]
    public string? Code { get; set; }
  }
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