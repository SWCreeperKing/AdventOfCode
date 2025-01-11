namespace AdventOfCode.Solutions._2019;

file class Day1() : Puzzle<int[]>(2019, 1, "The Tyranny of the Rocket Equation")
{
    public override int[] ProcessInput(string input) { return input.Split('\n').Select(int.Parse).ToArray(); }

    [Answer(3465245)] public override object Part1(int[] inp) { return inp.Select(i => i / 3 - 2).Sum(); }

    [Answer(5194970)]
    public override object Part2(int[] inp)
    {
        return inp.Select(i =>
                   {
                       List<int> ints = [];
                       var hold = i;
                       while ((hold = hold / 3 - 2) > 0) ints.Add(hold);
                       return ints.Sum();
                   })
                  .Sum();
    }
}