using Microsoft.AspNetCore.Mvc;
using DrugScanner.Server.Services;
using DrugScanner.Server.Models;

namespace DrugScanner.Server.Controllers.v1.Admin;

[Route("api/v1/admin/news-feeds")]
[ApiController]
public partial class NewsFeedsController(
  ApplicationDbContext dbContext, INewsFeedsService newsFeedsService) : ControllerBase
{
}