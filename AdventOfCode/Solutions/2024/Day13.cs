namespace AdventOfCode.Solutions._2024;

file class Day13() : Puzzle<(double[] a, double[] b, double[] dest)[]>(2024, 13, "Claw Contraption")
{
    public static readonly Regex Reg = new(@".*: X(?:\+|=)(\d+), Y(?:\+|=)(\d+)");

    public override (double[] a, double[] b, double[] dest)[] ProcessInput(string input)
    {
        return input.Split("\n\n")
                    .Select(line => line.Split('\n'))
                    .Select(machines =>
                         (Reg.Match(machines[0]).Groups.Range(1..2).SelectArr(double.Parse),
                             Reg.Match(machines[1]).Groups.Range(1..2).SelectArr(double.Parse),
                             Reg.Match(machines[2]).Groups.Range(1..2).SelectArr(double.Parse)))
                    .ToArray();
    }

    [Answer(31065)]
    public override object Part1((double[] a, double[] b, double[] dest)[] inp)
    {
        return Equate(inp);
    }

    [Answer(93866170395343)]
    public override object Part2((double[] a, double[] b, double[] dest)[] inp)
    {
        return Equate(inp, true);
    }

    public static long Equate((double[] a, double[] b, double[] dest)[] inp, bool part2 = false)
    {
        var total = 0L;
        foreach (var machine in inp)
        {
            var (a, b, dest) = machine;

            if (part2)
            {
                dest[0] += 10000000000000;
                dest[1] += 10000000000000;
            }

            var determinant = a[0] * b[1] - a[1] * b[0];
            if (determinant == 0)
            {
                var isASmaller = a[0] < b[0];
                var smaller = Grab(a, b, isASmaller);
                if (dest[0] % smaller[0] != 0 || dest[1] % smaller[1] != 0) continue;
                var aCount = dest[0] / a[0];
                var bCount = dest[0] / b[0];

                if (!isASmaller && a[0] / b[0] >= 3)
                {
                    if (aCount % 1 == 0)
                    {
                        total += (long)aCount * 3;
                        continue;
                    }

                    var aUsed = (long)Math.Floor(aCount) - 1;
                    total += aUsed * 3;
                    total += (long)((dest[0] - aUsed * a[0]) / b[0]);
                    continue;
                }

                if (bCount % 1 == 0 || (!isASmaller && a[0] / b[0] < 3))
                {
                    total += (long)bCount;
                }

                continue;
            }

            var A = (b[1] * dest[0] - b[0] * dest[1]) / determinant;
            var B = (a[0] * dest[1] - a[1] * dest[0]) / determinant;
            if (A % 1 != 0 || A < 0) continue;
            if (B % 1 != 0 || B < 0) continue;
            total += (long)(A * 3 + B);
        }

        return total;

        double[] Grab(double[] a, double[] b, bool choose) { return choose ? a : b; }
    }
}