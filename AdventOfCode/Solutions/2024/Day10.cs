namespace AdventOfCode.Solutions._2024;

file class Day10() : Puzzle<Matrix2d<int>>(2024, 10, "Hoof It")
{
    public override Matrix2d<int> ProcessInput(string input)
    {
        return new Matrix2d<int>(input.Split('\n').SelectArr(line => line.SelectArr(c => c - '0')));
    }

    [Answer(510)]
    public override object Part1(Matrix2d<int> inp)
    {
        var highes = inp.FindAll(i => i == 9);
        var lowes = inp.FindAll(i => i == 0);

        var sum = 0;
        Parallel.ForEach(lowes, low => { Add(ref sum, highes.Count(high => Path(low, high))); });

        return sum;

        bool Path(Pos start, Pos end)
        {
            if (start == end) return true;

            foreach (var dir in Pos.Surround)
            {
                var next = start + dir;
                if (!inp.PositionExists(next)) continue;
                if (inp[next] - inp[start] != 1) continue;
                if (!Path(next, end)) continue;
                return true;
            }

            return false;
        }
    }

    [Answer(1058)]
    public override object Part2(Matrix2d<int> inp)
    {
        var highes = inp.FindAll(i => i == 9);
        var lowes = inp.FindAll(i => i == 0);

        var sum = 0;

        Parallel.ForEach(lowes, low => { Add(ref sum, highes.Sum(high => Path(low, high))); });

        return sum;

        int Path(Pos start, Pos end)
        {
            if (start == end) return 1;

            var summ = 0;
            foreach (var dir in Pos.Surround)
            {
                var next = start + dir;
                if (!inp.PositionExists(next)) continue;
                if (inp[next] - inp[start] != 1) continue;
                summ += Path(next, end);
            }

            return summ;
        }
    }
}