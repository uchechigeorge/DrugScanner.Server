using Microsoft.AspNetCore.Mvc;
using DrugScanner.Server.Services;

namespace DrugScanner.Server.Controllers.v1.User;

[Route("api/v1/news-feeds")]
[ApiController]
public partial class NewsFeedsController(INewsFeedsService newsFeedsService) : ControllerBase
{
}