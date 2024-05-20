using Microsoft.AspNetCore.Mvc;
using DrugScanner.Server.Services;
using DrugScanner.Server.Models;

namespace DrugScanner.Server.Controllers.v1.User;

[Route("api/v1/drugs")]
[ApiController]
public partial class DrugsController : ControllerBase
{

  private readonly ApplicationDbContext mDbContext;
  private readonly IDrugsService mDrugsService;

  public DrugsController(
    ApplicationDbContext dbContext, IDrugsService drugsService)
  {
    mDrugsService = drugsService;
    mDbContext = dbContext;
  }

}