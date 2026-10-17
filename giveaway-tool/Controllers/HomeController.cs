using giveaway_tool.Library;
using giveaway_tool.Models;
using giveaway_tool.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace giveaway_tool.Controllers;

[Route("giveaway-tools")]
public class HomeController(IYouTube _yt, ICommon common) : Controller
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
        var comments = _yt.GetComments(picker.Input).DistinctBy(c => c.UserId);
        if (picker.WinnerCount > comments.Count()) picker.WinnerCount = (short)comments.Count();
        picker.Winners = common.PickWinners(comments.ToList(), picker.WinnerCount)?.ToList();
        return View(picker);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}