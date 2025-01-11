namespace AdventOfCode.Solutions._2024;

file class Day4() : Puzzle<Matrix2d<char>>(2024, 4, "Ceres Search")
{
    public override Matrix2d<char> ProcessInput(string input)
    {
        return new Matrix2d<char>(input.Split('\n').SelectArr(l => l.ToCharArray()));
    }

    [Answer(2642)]
    public override object Part1(Matrix2d<char> inp)
    {
        var count = 0;

        for (var i = 0; i < inp.Array.Length; i++)
        {
            if (inp[i] is not 'X') continue;
            var pos = inp.TranslatePosition(i);
            count += inp.WhereInCircle(pos, cc => cc == 'M')
                        .Count(p =>
                         {
                             var march = inp.MarchRangeArr(pos, 3, p - pos);
                             return march is not null && march[0] == 'M' && march[1] == 'A' && march[2] == 'S';
                         });
        }

        return count;
    }

    [Answer(1974)]
    public override object Part2(Matrix2d<char> inp)
    {
        var count = 0;

        for (var i = 0; i < inp.Array.Length; i++)
        {
            var (x, y) = inp.TranslatePosition(i);
            if (x == 0 || y == 0 || x == inp.Size.w - 1 || y == inp.Size.h - 1 || inp[i] is not 'A') continue;
            var chars = (char[]) [inp[x + 1, y + 1], inp[x - 1, y - 1], inp[x + 1, y - 1], inp[x - 1, y + 1]];
            if (chars[0] is 'X' or 'A' || chars[1] is 'X' or 'A' || chars[2] is 'X' or 'A' ||
                chars[3] is 'X' or 'A' || chars[0] == chars[1] || chars[2] == chars[3]) continue;
            count++;
        }

        return count;
    }
}