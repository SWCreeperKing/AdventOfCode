using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Experimental_Run;

public static class MdFormatter
{
    private const char WrongAnswer = '\u274c';
    private const char PossibleAnswer = '\u2754';
    private const char CorrectAnswer = '\u2705';
    private const char NoAnswer = '\u2796';

    private const string LinkStarter =
        "https://github.com/SWCreeperKing/AdventOfCode/blob/master/AdventOfCode/Solutions";

    public static string MakeFile(this Dictionary<int, (bool?[], TimeSpan?[])> stats, TimeSpan totalTime, int year, string[][] leaderboard)
    {
        StringBuilder sb = new();
        sb.Append("> [!NOTE]\n> Timings are only ran once, this is not an average\n\n");
        sb.Append($"> [!NOTE]\n> Total runtime: {totalTime.Time()}\n\n");
        sb.Append("|Day - Part|Time|Rank|Score|Passing|Time Completed|\n");
        sb.Append("|:-:|:-:|:-:|:-:|:-:|-:|\n");
        foreach (var (day, part, dayPart, time, rank, score) in leaderboard.DatIter(year))
        {
            if (day == 25 && part == 2) continue;
            if (!stats.TryGetValue(day, out var dayStat)) continue;
            var partStatTimeRaw = dayStat.Item2[part - 1];
            var partStatCompletion = partStatTimeRaw is null ? NoAnswer : dayStat.Item1[part - 1].ToEmote();
            var partStatTime = partStatTimeRaw is null ? "-" : partStatTimeRaw!.Value.Time();

            sb.Append($"|{dayPart}|{time}|{rank}|{score}|{partStatCompletion}|{partStatTime}|\n");
        }

        return sb.ToString();
    }

    private static IEnumerable<(int, int, string, string, string, string)> DatIter(this string[][] leaderboard,
        int year)
    {
        foreach (var arr in leaderboard)
        {
            var day = int.Parse(arr[0]);
            var link = $"{LinkStarter}/{year}/Day{day}.cs";
            var part1 = arr[1..4];
            var part2 = arr[4..];
            yield return (day, 2, $"[{day} - 2]({link})", part2[0], part2[1], part2[2]);
            yield return (day, 1, $"[{day} - 1]({link})", part1[0], part1[1], part1[2]);
        }
    }

    private static char ToEmote(this bool? b)
        => b switch
        {
            null => PossibleAnswer,
            true => CorrectAnswer,
            _ => WrongAnswer
        };
}