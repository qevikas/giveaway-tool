using giveaway_tool.Models;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;

namespace giveaway_tool.Services;
public class YouTube : IYouTube
{
    private readonly string apiKey = "AIzaSyDrmjoQvAvD3avpMHs77eVZ9zxiizZ8E9k";

    public IEnumerable<Winner> GetComments(string url)
    {
        var ytService = new YouTubeService(new BaseClientService.Initializer
        {
            ApiKey = apiKey,
            ApplicationName = "YouTube Comment Picker"
        });
        var request = ytService.CommentThreads.List("snippet");
        request.VideoId = ExtractVideoID(url);
        request.MaxResults = 100;

        var response = request.Execute();
        var winners = response.Items.Select(s =>
        {
            var snippet = s.Snippet.TopLevelComment.Snippet;
            return new Winner
            {
                UserId = snippet.AuthorChannelId?.Value,
                UserName = snippet.AuthorDisplayName,
                ProfileImageUrl = snippet.AuthorProfileImageUrl,
                CommentText = snippet.TextDisplay,
                PublishedOn = snippet.PublishedAtDateTimeOffset,
                UpdatedOn = snippet.UpdatedAtDateTimeOffset,
                Platform = "YouTube"
            };
        });
        return winners;
    }

    private static string ExtractVideoID(string url)
    {
        if (url.Contains("v="))
            return url.Split(new[] { "v=" }, StringSplitOptions.None)[1].Split('&')[0];
        else if (url.Contains("youtu.be/"))
            return url.Split(new[] { "youtu.be/" }, StringSplitOptions.None)[1].Split('?')[0];
        return url; // assume user enters video id directly
    }
}
