namespace AdventOfCode.Solutions._2024;

[Day(2024, 10, "Hoof It")]
file class Day10
{
    [ModifyInput] public static string ProcessInput(string input) => input;

    [Answer(510)]
    public static long Part1(string inp)
    {
        var nlInp = inp.Split('\n');
        Matrix2d<int> map = new(nlInp.SelectArr(line => line.SelectArr(c => c == '.' ? 20 : int.Parse(c.ToString()))));
        var highes = map.FindAll(i => i == 9);
        var lowes = map.FindAll(i => i == 0);

        var sum = 0;
        Parallel.ForEach(lowes, low =>
        {

            foreach (var high in highes)
            {
                if (!Path([low], low, high, 0)) continue;
                Increment(ref sum);
            }
        });

        return sum;

        bool Path(Pos[] path, Pos start, Pos end, int step)
        {
            if (start == end && step == 9) return true;
            if (step > 9) return false;

            foreach (var dir in Pos.Surround)
            {
                var next = start + dir;
                if (!map.PositionExists(next)) continue;
                if (path.Contains(next)) continue;
                if (Math.Abs(map[start] - map[next]) > 1) continue;
                if (!Path([..path, next], next, end, step + 1)) continue;
                return true;
            }

            return false;
        }
    }

    [Answer(1058)]
    public static long Part2(string inp)
    {
        var nlInp = inp.Split('\n');
        Matrix2d<int> map = new(nlInp.SelectArr(line => line.SelectArr(c => c == '.' ? 20 : int.Parse(c.ToString()))));
        var highes = map.FindAll(i => i == 9);
        var lowes = map.FindAll(i => i == 0);

        var sum = 0;
        
        Parallel.ForEach(lowes, low =>
        {
            foreach (var high in highes)
            {
                Add(ref sum, Path([low], low, high, 0));
            }
        });

        return sum;

        int Path(Pos[] path, Pos start, Pos end, int step)
        {
            if (start == end && step == 9) return 1;
            if (step > 9) return 0;

            var summ = 0;
            foreach (var dir in Pos.Surround)
            {
                var next = start + dir;
                if (!map.PositionExists(next)) continue;
                if (path.Contains(next)) continue;
                if (Math.Abs(map[start] - map[next]) > 1) continue;
                summ += Path([..path, next], next, end, step + 1);
            }

            return summ;
        }
    }
}