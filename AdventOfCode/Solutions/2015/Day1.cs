namespace AdventOfCode.Solutions._2015;

file class Day1() : Puzzle<string>(2015, 1, "Not Quite Lisp")
{
    public override string ProcessInput(string input) { return input; }

    [Answer(280)] public override object Part1(string inp) { return inp.Sum(c => c is '(' ? 1 : -1); }

    [Answer(1797)]
    public override object Part2(string inp)
    {
        return 0.Inline(floor => (..inp.Length)
           .LoopSelect(i => (floor += inp[(int)i] is '(' ? 1 : -1) == -1, i => i + 1, floor));
    }
}