namespace AdventOfCode.Solutions._2023;

file class Day1() : Puzzle<string[]>(2023, 1, "Trebuchet?!")
{
    public override string[] ProcessInput(string input) { return input.Split('\n'); }
    [Answer(54916)] public override object Part1(string[] inp) { return inp.Select(ParseRunNums).Sum(); }

    [Answer(54728)]
    public override object Part2(string[] inp)
    {
        return inp.Select(str => str
                      .LoopReplace(("eight", "e8t"), ("nine", "n9"), ("seven", "7"),
                           ("six", "6"), ("five", "5"), ("four", "4"),
                           ("three", "3"), ("one", "o1"), ("two", "2")))
                  .Select(ParseRunNums)
                  .Sum();
    }

    public static int RunNums(int[] nums) { return nums.Length == 0 ? 0 : nums.First() * 10 + nums.Last(); }

    public static int ParseRunNums(string str)
    {
        return RunNums(str.Where(char.IsDigit).Select(c => c - 48).ToArray());
    }
}