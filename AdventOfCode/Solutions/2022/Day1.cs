namespace AdventOfCode.Solutions._2022;

file class Day1() : Puzzle<int[]>(2022, 1, "Calorie Counting")
{
    public override int[] ProcessInput(string input)
    {
        return input.Split("\n\n")
                    .Select(s =>
                         s.Split('\n').Select(int.Parse).Sum())
                    .ToArray();
    }

    [Answer(68923)] public override object Part1(int[] inp) { return inp.Max(); }
    [Answer(200044)] public override object Part2(int[] inp) { return inp.OrderDescending().Take(3).Sum(); }
}