using giveaway_tool.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace giveaway_tool.Controllers;

[Route("giveaway-tools")]
public class HomeController : Controller
{
    [Route("home")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet, Route("youtube-comment-picker")]
    public IActionResult YouTubeCommentPicker()
    {
        return View(new CommentPicker());
    }

    [HttpPost, Route("youtube-comment-picker")]
    public IActionResult YouTubeCommentPicker(CommentPicker picker)
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
