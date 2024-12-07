using System.Threading;
using AdventOfCode.Experimental_Run.Misc;

namespace AdventOfCode.Solutions._2024;

[Day(2024, 7, "wip"), Run]
file class Day7
{
    [ModifyInput] public static string ProcessInput(string input) => input;

    [Answer(1582598718861)]
    public static long Part1(string inp)
    {
        var nlInp = inp.Split('\n');
        var total = 0L;
        foreach (var line in nlInp)
        {
            var split = line.Split(": ");
            var num = long.Parse(split[0]);
            var list = split[1].Split(' ').SelectArr(int.Parse);

            var res = 0L;
            var state = new int[list.Length - 1];
            var loop = true;
            while (res != num && loop)
            {
                res = list[0];
                for (var i = 1; i < list.Length; i++)
                {
                    if (res > num) break;
                    switch (state[i - 1])
                    {
                        case 0:
                            res += list[i];
                            break;
                        case 1:
                            res *= list[i];
                            break;
                    }
                }

                state[0]++;
                for (var i = 0; i < state.Length; i++)
                {
                    if (state[i] < 2) break;
                    if (i == state.Length - 1)
                    {
                        loop = false;
                        break;
                    }

                    state[i] = 0;
                    state[i + 1]++;
                }

                if (res != num) continue;
                total += res;
            }
        }

        return total;
    }

    [Answer(165278151522644)]
    public static long Part2(string inp)
    {
        var nlInp = inp.Split('\n');
        var total = 0L;
        Parallel.ForEach(nlInp, line =>
        {
            var split = line.Split(": ");
            var num = long.Parse(split[0]);
            var list = split[1].Split(' ').SelectArr(int.Parse);
            var res = 0L;
            var state = new int[list.Length - 1];
            var loop = true;
            while (res != num && loop)
            {
                res = list[0];
                for (var i = 1; i < list.Length; i++)
                {
                    if (res > num) break;
                    switch (state[i - 1])
                    {
                        case 0:
                            res += list[i];
                            break;
                        case 1:
                            res *= list[i];
                            break;
                        case 2:
                            res = res * (long)Math.Pow(10, (long)Math.Floor(Math.Log10(list[i])) + 1) + list[i];
                            break;
                    }
                }

                state[0]++;
                for (var i = 0; i < state.Length; i++)
                {
                    if (state[i] < 3) break;
                    if (i == state.Length - 1)
                    {
                        loop = false;
                        break;
                    }

                    state[i] = 0;
                    state[i + 1]++;
                }

                if (res != num) continue;
                Interlocked.Add(ref total, res);
            }
        });

        return total;
    }
}