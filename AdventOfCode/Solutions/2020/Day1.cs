namespace AdventOfCode.Solutions._2020;

file class Day1() : Puzzle<int[]>(2020, 1, "Report Repair")
{
    public override int[] ProcessInput(string input) { return input.Split('\n').Select(int.Parse).ToArray(); }

    [Answer(1016619)]
    public override object Part1(int[] inp)
    {
        return inp.Select(i => (i, n: 2020 - i))
                  .Where(t => inp.Contains(t.n))
                  .Select(t => t.i * t.n)
                  .First();
    }

    [Answer(218767230)]
    public override object Part2(int[] inp)
    {
        return inp.SelectMany(_ => inp, (i, j) => (i, j))
                  .Select(t => (t, n: 2020 - t.i - t.j))
                  .Where(t => inp.Contains(t.n))
                  .Select(t => t.t.i * t.t.j * t.n)
                  .First();
    }
}