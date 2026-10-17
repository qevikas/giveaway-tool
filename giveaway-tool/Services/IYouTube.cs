using giveaway_tool.Models;

namespace giveaway_tool.Services;
public interface IYouTube
{
    /// <summary>
    /// Fetch a single page of top-level comments from a YouTube video URL
    /// </summary>
    /// <param name="url">Full YouTube video url or YouTube video ID</param>
    /// <returns>An enumerable collection of mapped <see cref="Winner"/> objects containing user info and comment text.</returns>
    IEnumerable<Winner> GetComments(string url);
}
