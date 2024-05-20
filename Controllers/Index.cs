using Microsoft.AspNetCore.Mvc;

namespace DrugScanner.Server.Controllers;

[ApiController]
[Route("")]
public class RootController : ControllerBase
{
  [HttpGet]
  public ActionResult Get()
  {

    #region Json

    string json = @"
{
    ""status"": true,
    ""message"": ""You probably shouldn't be here, but..."",
    ""data"": {
        ""service"": ""DrugScanner.Server"",
        ""version"": ""1.0""
    }
}";

    #endregion

    return Content("<pre style='word-wrap: break-word; white-space: pre-wrap;'>" + json + "</pre>", "text/html");
  }
}