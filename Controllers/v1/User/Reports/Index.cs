using Microsoft.AspNetCore.Mvc;
using DrugScanner.Server.Services;
using DrugScanner.Server.Models;

namespace DrugScanner.Server.Controllers.v1.User;

[Route("api/v1/reports")]
[ApiController]
public partial class ReportsController : ControllerBase
{

  private readonly ApplicationDbContext mDbContext;
  // private readonly IReportsService mReportsService;

  public ReportsController(
    ApplicationDbContext dbContext
    // , IReportsService reportsService
    )
  {
    mDbContext = dbContext;
    // mReportsService = reportsService;
  }

}