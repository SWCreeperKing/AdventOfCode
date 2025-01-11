namespace AdventOfCode.Solutions._2017;

file class Day9() : Puzzle<string>(2017, 9, "Stream Processing")
{
    public override string ProcessInput(string input) { return input; }

    [Answer(7616)]
    public override object Part1(string inp)
    {
        inp = inp.Replace("!!", "")
                 .RemoveWhile('!', 2)
                 .RemoveWhile('<', (s, i) => s.IndexOf('>', i))
                 .Replace(",", "");

        var layer = 0;
        var count = 0;
        foreach (var c in inp)
        {
            if (c == '{')
            {
                layer++;
                continue;
            }

            count += layer;
            layer--;
        }

        return count;
    }

    [Answer(3838)]
    public override object Part2(string inp)
    {
        return inp.Replace("!!", "")
                  .RemoveWhile('!', 2)
                  .RemoveWhileIterator('<', (s, i) => s.IndexOf('>', i))
                  .Sum(substr => substr.Length - 2);
    }
}