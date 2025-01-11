namespace AdventOfCode.Solutions._2022;

file class Day3() : Puzzle<string[]>(2022, 3, "Rucksack Reorganization")
{
    public override string[] ProcessInput(string inp) { return inp.Split('\n'); }

    [Answer(7903)]
    public override object Part1(string[] inp)
    {
        return inp.Select(s => s.Chunk(s.Length / 2))
                  .Select(carr => carr.InterceptSelf().First())
                  .Select(Value)
                  .Sum();
    }

    [Answer(2548)]
    public override object Part2(string[] inp) { return inp.Chunk(3).Select(arr => Value(arr.InterceptSelf()[0])).Sum(); }

    private static int Value(char c) { return c is >= 'a' and <= 'z' ? c - 'a' + 1 : c - 'A' + 27; }
}