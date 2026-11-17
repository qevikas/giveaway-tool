namespace giveaway_tool.Contracts;
public interface ICommon
{
    IEnumerable<T> PickWinners<T>(IList<T> winners, short winnerCount);
}
