namespace AdventOfCode.Solutions._2024;

[Day(2024, 2, "Red-Nosed Reports")]
file class Day2
{
    [ModifyInput]
    public static int[][] ProcessInput(string input)
    {
        return input.Split('\n').SelectArr(l => l.Split(' ').SelectArr(int.Parse));
    }

    [Answer(442)] public static long Part1(int[][] inp) { return inp.Count(Check); }

    [Answer(493)]
    public static long Part2(int[][] inp)
    {
        return inp
           .Count(ints => (..ints.Length)
               .LoopSelect(l => Check(ints.SkipIndexArr((int)l)), _ => true));
    }

    public static bool Check(int[] ints)
    {
        return ints.WindowArr(2, arr => arr[0] - arr[1])
                   .Inline(deltas
                        => (deltas.All(i => i < 0) || deltas.All(i => i > 0)) &&
                           deltas.All(i => Math.Abs(i) is >= 1 and <= 3));
    }
}