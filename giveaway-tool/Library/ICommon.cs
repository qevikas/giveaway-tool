namespace giveaway_tool.Library;
public interface ICommon
{
    IEnumerable<T> PickWinners<T>(IList<T> winners, short winnerCount);
}
