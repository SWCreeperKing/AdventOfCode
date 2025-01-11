namespace AdventOfCode.Solutions._2019;

file class Day2() : Puzzle<int[]>(2019, 2, "Program Alarm")
{
    public override int[] ProcessInput(string input) { return input.Split(',').Select(int.Parse).ToArray(); }

    [Answer(2692315)]
    public override object Part1(int[] inp)
    {
        inp[1] = 12;
        inp[2] = 2;
        for (var i = 0; i < inp.Length; i += 4)
        {
            if (inp[i] is not (1 or 2)) break;
            inp[inp[i + 3]] = inp[i] == 1
                ? inp[inp[i + 1]] + inp[inp[i + 2]]
                : inp[inp[i + 1]] * inp[inp[i + 2]];
        }

        return inp[0];
    }

    [Answer(9507)]
    public override object Part2(int[] inpRaw)
    {
        for (var j = 0; j < 100; j++)
        for (var k = 0; k < 100; k++)
        {
            var inp = inpRaw.ToArray();
            inp[1] = j;
            inp[2] = k;
            for (var i = 0; i < inp.Length; i += 4)
            {
                if (inp[i] is not (1 or 2)) break;
                inp[inp[i + 3]] = inp[i] == 1
                    ? inp[inp[i + 1]] + inp[inp[i + 2]]
                    : inp[inp[i + 1]] * inp[inp[i + 2]];
            }

            if (inp[0] == 19690720) return 100 * j + k;
        }

        return -1;
    }
}