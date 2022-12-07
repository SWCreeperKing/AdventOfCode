using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2016;

[Day(2016, 9, "Explosives in Cyberspace")]
public static partial class Day9
{
    public static Regex paraFind = new(@"\((\d+)x(\d+)\)", RegexOptions.Compiled);

    [Answer(120765)]
    public static long Part1(string inp)
    {
        var str = inp.AsSpan();
        StringBuilder sb = new();

        for (var i = 0; i < str.Length; i++)
        {
            if (str[i] != '(')
            {
                sb.Append(str[i]);
                continue;
            }

            i++;
            var end = str[i..].IndexOf(')') + i;
            var data = str[i..end];
            var count = int.Parse(data[..data.IndexOf('x')]);
            var repeat = int.Parse(data[(data.IndexOf('x') + 1)..]);
            i += data.Length + 1;
            var take = str[i..(i + count)];
            i += count - 1;
            sb.Append(Enumerable.Repeat(take.ToString(), repeat).Join());
        }

        return sb.ToString().Length;
    }

    // public static long Part2(string inp)
    // {
    //     inp = "(25x3)(3x3)ABC(2x3)XY(5x2)PQRSTX(18x9)(3x2)TWO(5x7)SEVEN";
    //     var str = inp;
    //     while (str.Contains('(')) str = ComputeString(str);
    //     return str.Length;
    // }

}