namespace giveaway_tool.Models;
public class Winner
{
    public string UserName { get; set; }
    public string UserId { get; set; }
    public string ProfileImageUrl { get; set; }
    public string ProfileUrl { get; set; }
    public string CommentText { get; set; }
    public string Platform { get; set; }
    public DateTime? PublishedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
}
