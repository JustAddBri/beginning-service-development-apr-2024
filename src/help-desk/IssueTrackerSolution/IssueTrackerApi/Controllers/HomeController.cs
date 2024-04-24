using Microsoft.AspNetCore.Mvc;

namespace IssueTrackerApi.Controllers;

public class HomeController : ControllerBase
{
    [HttpGet("/")]
    public ActionResult GetHome()
    {
        return Ok("Welcome to my super Issues API!");
    }
}
