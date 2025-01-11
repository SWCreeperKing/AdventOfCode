namespace AdventOfCode.Solutions._2017;

internal class Day2() : Puzzle<int[][]>(2017, 2, "Corruption Checksum")
{
    public override int[][] ProcessInput(string input)
    {
        return input.Split('\n').Select(s => s.CleanSpaces().Split(' ').Select(int.Parse).ToArray()).ToArray();
    }

    [Answer(46402)] public override object Part1(int[][] input) { return input.Sum(row => row.Max() - row.Min()); }

    [Answer(265)]
    public override object Part2(int[][] inp)
    {
        return inp.Select(arr =>
                   {
                       for (var i = 0; i < arr.Length; i++)
                       for (var j = 0; j < arr.Length; j++)
                       {
                           if (i == j) continue;
                           if (arr[i] % arr[j] == 0) return arr[i] / arr[j];
                       }

                       return 0;
                   })
                  .Sum();
    }
}