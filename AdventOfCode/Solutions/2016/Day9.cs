namespace AdventOfCode.Solutions._2016;

file class Day9() : Puzzle<string>(2016, 9, "Explosives in Cyberspace")
{
    public override string ProcessInput(string input) { return input; }
    public static readonly Regex ParaFind = new(@"\((\d+)x(\d+)\)", RegexOptions.Compiled);
    [Answer(120765)] public override object Part1(string inp) { return CountData(inp); }
    [Answer(11658395076)] public override object Part2(string inp) { return CountData(inp, true); }

    public static long CountData(string inp, bool recurse = false)
    {
        var str = inp.AsSpan();
        long counter = 0;

        for (var i = 0; i < str.Length; i++)
        {
            if (str[i] != '(')
            {
                counter++;
                continue;
            }

            var end = str[i..].IndexOf(')') + i + 1;
            var data = ParaFind.Match(str[i..end].ToString()).Groups.Range(1..2).ToIntArr();
            i = end + data[0] - 1;
            if (recurse)
            {
                var subString = str[end..(end + data[0])].ToString();
                counter += CountData(subString, true) * data[1];
            }
            else
            {
                counter += data.Multi();
            }
        }

        return counter;
    }
}