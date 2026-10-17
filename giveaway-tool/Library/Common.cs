using System.Security.Cryptography;

namespace giveaway_tool.Library;
public class Common
{

    public static void Shuffle<T>(IList<T> list)
    {
        if (list is null || list.Count <= 1) return;
        int n = list.Count;
        while (n > 1)
        {
            int i = RandomNumberGenerator.GetInt32(n);
            n--;
            (list[n], list[i]) = (list[n], list[i]);
        }
    }

}