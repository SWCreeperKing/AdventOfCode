namespace AdventOfCode.Solutions._2017;

file class Day6() : Puzzle<int[]>(2017, 6, "Memory Reallocation")
{
    public override int[] ProcessInput(string input)
    {
        return input.Replace('\t', ' ').Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
    }

    [Answer(5042)]
    public override object Part1(int[] inp)
    {
        HashSet<string> hash = [inp.String()];

        for (var i = 1;; i++)
        {
            var index = inp.FindIndexOf(inp.Max());
            var val = inp[index];
            inp[index] = 0;

            var j = index;
            while (val > 0)
            {
                j = (j + 1) % inp.Length;
                inp[j]++;
                val--;
            }

            if (!hash.Add(inp.String())) return i;
        }
    }

    [Answer(1086)]
    public override object Part2(int[] inp)
    {
        HashSet<string> hash = [inp.String()];
        var recorded = "";

        for (var i = 1;; i++)
        {
            var index = inp.FindIndexOf(inp.Max());
            var val = inp[index];
            inp[index] = 0;

            var j = index;
            while (val > 0)
            {
                j = (j + 1) % inp.Length;
                inp[j]++;
                val--;
            }

            if (hash.Add(inp.String()) || (recorded != "" && recorded != inp.String())) continue;
            if (recorded != "") return i;
            i = 0;
            recorded = inp.String();
        }
    }
}