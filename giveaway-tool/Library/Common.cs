using System.Security.Cryptography;

namespace giveaway_tool.Library;
public class Common : ICommon
{

    public IEnumerable<T> PickWinners<T>(IList<T> winners, short winnerCount)
    {
        if (winners is null || winners.Count == 0) return null;
        Shuffle(winners);
        return winners.Take(winnerCount);
    }

    private static void Shuffle<T>(IList<T> list)
    {
        if (list is null || list.Count <= 1) return;
        int n = list.Count;
        while (n > 1)
        {
            int i = RandomNumberGenerator.GetInt32(n);
            n--;
            (list[n], list[i]) = (list[i], list[n]);
        }
    }

}