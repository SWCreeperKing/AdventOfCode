using AdventOfCode.Experimental_Run.Misc;

namespace AdventOfCode.Solutions._2024;

[Day(2024, 13, "Claw Contraption"), Run]
file class Day13
{
    [ModifyInput] public static string ProcessInput(string input) => input;

    [Answer(31065)]
    public static long Part1(string inp)
    {
        var nlInp = inp.Split("\n\n").SelectArr(line => line.Split('\n'));
        var total = 0L;
        foreach (var machine in nlInp)
        {
            var minMoves = int.MaxValue;
            var a = machine[0].Split(": ")[1].Replace("X+", "").Replace("Y+", "").Split(", ").SelectArr(int.Parse);
            var b = machine[1].Split(": ")[1].Replace("X+", "").Replace("Y+", "").Split(", ").SelectArr(int.Parse);
            var dest = machine[2].Split(": ")[1].Replace("X=", "").Replace("Y=", "").Split(", ").SelectArr(int.Parse);

            for (var i = 0; i <= 100; i++)
            {
                for (var j = 0; j <= 100; j++)
                {
                    int[] aNormal = [a[0] * i, a[1] * i];
                    int[] bNormal = [b[0] * j, b[1] * j];

                    if (aNormal[0] + bNormal[0] != dest[0] || aNormal[1] + bNormal[1] != dest[1]) continue;
                    minMoves = Math.Min(minMoves, j + i * 3);
                }
            }

            if (minMoves == int.MaxValue) continue;
            total += minMoves;
        }

        return total;
    }
    
    [Answer(93866170395343)]
    public static long Part2(string inp)
    {
        var nlInp = inp.Split("\n\n").SelectArr(line => line.Split('\n'));
        var total = 0L;
        foreach (var machine in nlInp)
        {
            var a = machine[0].Split(": ")[1].Replace("X+", "").Replace("Y+", "").Split(", ").SelectArr(double.Parse);
            var b = machine[1].Split(": ")[1].Replace("X+", "").Replace("Y+", "").Split(", ").SelectArr(double.Parse);
            var dest = machine[2].Split(": ")[1]
                                 .Replace("X=", "")
                                 .Replace("Y=", "")
                                 .Split(", ")
                                 .SelectArr(double.Parse);

            dest[0] += 10000000000000;
            dest[1] += 10000000000000;

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