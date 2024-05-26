using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DrugScanner.Server.Controllers.v1.Admin;

public partial class DrugsController : ControllerBase
{

  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(int id)
  {
    try
    {
      var drug = await dbContext.Drugs.Where(e => e.Id == id).FirstOrDefaultAsync();
      if (drug == null)
      {
        return BadRequest(new { Status = 400, Message = "Invalid drug" });
      }

      drug.DateDeleted = DateTime.UtcNow;

      await dbContext.SaveChangesAsync();

      return Ok(new { Status = 200, Message = "Ok" });
    }
    catch (Exception ex)
    {
      return StatusCode(500, new { Status = 500, ex.Message });
    }
  }
}