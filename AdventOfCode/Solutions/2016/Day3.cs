namespace AdventOfCode.Solutions._2016;

file class Day3() : Puzzle<int[][]>(2016, 3, "Squares With Three Sides")
{
    public static readonly Regex InputRegex = new(@"\s+(\d+)\s+(\d+)\s+(\d+)", RegexOptions.Compiled);

    public override int[][] ProcessInput(string inp)
    {
        return inp.Split('\n')
                  .Select(s => InputRegex.Match(s).Groups.Range(1..3).Select(int.Parse).ToArray())
                  .ToArray();
    }

    [Answer(1032)]
    public override object Part1(int[][] inp)
    {
        return inp.Count(i =>
        {
            for (var j = 0; j < 3; j++)
                if (i[j] + i[(j + 1) % 3] <= i[(j + 2) % 3])
                    return false;

            return true;
        });
    }

    [Answer(1838)]
    public override object Part2(int[][] inp)
    {
        List<int[]> triangles = [];

        foreach (var arr in inp.Chunk(3))
        {
            // NOTE TO SELF: Enumerable.Range() COPIES REFERENCE NOT THE OBJECT
            triangles.AddRange(new[] { new int[3], new int[3], new int[3] });
            for (var i = 0; i < 3; i++)
            {
                var line = arr[i];
                triangles[^1][i] = line[0];
                triangles[^2][i] = line[1];
                triangles[^3][i] = line[2];
            }
        }

        return Part1(triangles.ToArray());
    }
}