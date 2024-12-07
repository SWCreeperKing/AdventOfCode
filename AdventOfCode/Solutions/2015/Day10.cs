using System.Text;

namespace AdventOfCode.Solutions._2015;

[Day(2015, 10, "Elves Look, Elves Say")]
file class Day10
{
    [Answer(492982)] public static long Part1(string input) { return RunLook(input, 40); }

    [Answer(6989950)] public static long Part2(string input) { return RunLook(input, 50); }

    private static string LookSay(string look)
    {
        Span<char> span = look.ToCharArray();
        StringBuilder sb = new();
        var indexChar = 0;
        for (var i = 1; i < span.Length; i++)
        {
            if (span[indexChar] == span[i]) continue;
            sb.Append(i - indexChar).Append(span[indexChar]);
            indexChar = i;
        }

        sb.Append(span.Length - indexChar).Append(span[indexChar]);
        return sb.ToString();
    }

    private static int RunLook(string inp, int run)
    {
        var s = inp;
        for (var i = 0; i < run; i++) s = LookSay(s);
        return s.Length;
    }
}