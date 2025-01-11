namespace AdventOfCode.Solutions._2021;

file class Day3() : Puzzle<string[]>(2021, 3, "Binary Diagnostic")
{
    public override string[] ProcessInput(string input) { return input.Split('\n'); }

    [Answer(1082324)]
    public override object Part1(string[] inp)
    {
        var gamma = Enumerable.Range(0, inp[0].Length)
                              .Select(i => inp.Select(s => s[i]).Join())
                              .Select(s => s.Count(c => c is '0') > inp.Length / 2 ? '0' : '1')
                              .Join();
        return Convert.ToInt32(gamma, 2) * Convert.ToInt32(gamma.Select(c => c == '0' ? '1' : '0').Join(), 2);
    }

    [Answer(1353024)]
    public override object Part2(string[] inp)
    {
        int FindMostReplace(List<string> ar, char first = '0', char sec = '1')
        {
            for (var i = 0; i < ar[0].Length && ar.Count > 1; i++)
                ar = ar.Where(s => s[i] == (ar.Count(s => s[i] == '0') > ar.Count / 2 ? first : sec)).ToList();
            return Convert.ToInt32(ar[0], 2);
        }

        return FindMostReplace(inp.ToList()) * FindMostReplace(inp.ToList(), '1', '0');
    }
}