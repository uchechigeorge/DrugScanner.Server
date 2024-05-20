using Microsoft.AspNetCore.Mvc;
using DrugScanner.Server.Services;

namespace DrugScanner.Server.Controllers.v1.Admin;

[Route("api/v1/admin/reports")]
[ApiController]
public partial class ReportsController(
  IReportsService reportsService
    ) : ControllerBase
{
}