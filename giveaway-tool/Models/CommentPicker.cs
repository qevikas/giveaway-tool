using System.ComponentModel.DataAnnotations;

namespace giveaway_tool.Models;
public class CommentPicker
{
    [Required, Display(Name = "URL")]
    public string Input { get; set; }
    [Required, Display(Name = "Winner Count")]
    public short WinnerCount { get; set; } = 5;
    public List<Winner> Winners { get; set; }
}
