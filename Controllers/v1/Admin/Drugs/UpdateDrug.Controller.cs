using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DrugScanner.Server.Controllers.v1.Admin;

public partial class DrugsController : ControllerBase
{

  [HttpPatch("{id}")]
  public async Task<IActionResult> Update(int id, [FromBody] UpdateDrugBody body)
  {
    try
    {
      var drug = await dbContext.Drugs.Where(e => e.Id == id).FirstOrDefaultAsync();
      if (drug == null)
      {
        return BadRequest(new { Status = 400, Message = "Invalid drug" });
      }

      drug.Name = body.Name;
      drug.Code = body.Code;
      drug.BatchNo = body.BatchNo;
      drug.ExpirationDate = body.ExpirationDate;
      drug.ManufacturedBy = body.ManufacturedBy;
      drug.ManufacturingDate = body.ManufacturingDate;
      drug.StatusId = body.StatusId;

      await dbContext.SaveChangesAsync();

      return Ok(new { Status = 200, Message = "Ok" });
    }
    catch (Exception ex)
    {
      return StatusCode(500, new { Status = 500, ex.Message });
    }
  }


  public class UpdateDrugBody
  {
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Code { get; set; }
    public string? BatchNo { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public string? ManufacturedBy { get; set; }
    public DateTime? ManufacturingDate { get; set; }
    public int StatusId { get; set; }
  }
}