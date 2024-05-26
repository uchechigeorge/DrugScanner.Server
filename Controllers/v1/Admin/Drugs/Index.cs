using Microsoft.AspNetCore.Mvc;
using DrugScanner.Server.Services;
using DrugScanner.Server.Models;

namespace DrugScanner.Server.Controllers.v1.Admin;

[Route("api/v1/admin/drugs")]
[ApiController]
public partial class DrugsController(
  ApplicationDbContext dbContext, IDrugsService drugsService) : ControllerBase
{
}