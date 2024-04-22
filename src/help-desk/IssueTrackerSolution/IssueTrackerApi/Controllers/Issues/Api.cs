using Microsoft.AspNetCore.Mvc;

namespace IssueTrackerApi.Controllers.Issues;

public class Api : ControllerBase
{
    // GET /issues
    [HttpGet("/issues")]
    public async Task<ActionResult> GetTheIssuesAsync()
    {
        return Ok();
    }
}
