using System.ComponentModel.DataAnnotations;
using DrugScanner.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DrugScanner.Server.Controllers.v1.Admin;

public partial class DrugsController : ControllerBase
{

  [HttpPost]
  public async Task<IActionResult> Add([FromBody] AddDrugBody body)
  {
    try
    {
      var codeExists = await dbContext.Drugs.Where(e => e.Code == body.Code).AnyAsync();
      if (codeExists)
      {
        return BadRequest(new { Status = 400, Message = "Duplicate codes" });
      }

      var drug = new Drug
      {
        Name = body.Name,
        Code = body.Code,
        BatchNo = body.BatchNo,
        ExpirationDate = body.ExpirationDate,
        ManufacturedBy = body.ManufacturedBy,
        ManufacturingDate = body.ManufacturingDate,
        StatusId = body.StatusId,
      };
      await dbContext.Drugs.AddAsync(drug);
      await dbContext.SaveChangesAsync();

      return CreatedAtAction(nameof(GetOne), new { drug.Id }, new { Status = 201, Message = "Created", Data = GetDrug(drug) });
    }
    catch (Exception ex)
    {
      return StatusCode(500, new { Status = 500, ex.Message });
    }
  }

  public class AddDrugBody
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