namespace AdventOfCode.Solutions._2024;

[Day(2024, 10, "Hoof It")]
file class Day10
{
    [ModifyInput]
    public static Matrix2d<int> ProcessInput(string input)
    {
        return new Matrix2d<int>(input.Split('\n')
                                      .SelectArr(line => line
                                          .SelectArr(c => c == '.' ? 20 : int.Parse(c.ToString()))));
    }

    [Answer(510)]
    public static long Part1(Matrix2d<int> inp)
    {
        var highes = inp.FindAll(i => i == 9);
        var lowes = inp.FindAll(i => i == 0);

        var sum = 0;
        Parallel.ForEach(lowes, low =>
        {
            foreach (var high in highes)
            {
                if (!Path(low, high)) continue;
                Increment(ref sum);
            }
        });

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
    public static long Part2(Matrix2d<int> inp)
    {
        var highes = inp.FindAll(i => i == 9);
        var lowes = inp.FindAll(i => i == 0);

        var sum = 0;

        Parallel.ForEach(lowes, low =>
        {
            foreach (var high in highes)
            {
                Add(ref sum, Path(low, high));
            }
        });

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