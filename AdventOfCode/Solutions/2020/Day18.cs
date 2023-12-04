using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2020;

[Day(2020, 18, "Operation Order")]
public class Day18
{
    [ModifyInput] public static string[] ProcessInput(string input) => input.Split('\n');
    [Answer(12956356593940)] public static long Part1(string[] inp) => inp.Sum(s => CalculateStart(s));
    [Answer(94240043727614)] public static long Part2(string[] inp) => inp.Sum(s => CalculateStart(s, false));

    private static (int, int) GetPera(IList<string> equation)
    {
        var start = equation.IndexOf("(");
        if (equation.Count(s => s[0] == ')') == 1) return (start, equation.IndexOf(")") - start);
        var layered = 0;
        for (var i = 0; i < equation.Count; i++)
            switch (equation[i][0])
            {
                case ')':
                    layered--;
                    if (layered == 0) return (start, i - start);
                    break;
                case '(':
                    layered++;
                    break;
                default:
                    continue;
            }

        return (start, equation.Count - 1);
    }

    private static long Calculate(List<string> equation, bool isPart1 = true)
    {
        while (equation.Count > 1)
        {
            if (equation.Contains("("))
            {
                var (start, count) = GetPera(equation);
                var snip = equation.GetRange(start + 1, count - 1);
                equation.RemoveRange(start, count + 1);
                equation.Insert(start, $"{Calculate(snip, isPart1)}");
            }
            else if ((equation.Contains("*") || equation.Contains("+")) && isPart1)
            {
                var isMulti = equation.First(s => s is "*" or "+") == "*";
                var indx = equation.IndexOf(isMulti ? "*" : "+");
                var result = isMulti
                    ? long.Parse(equation[indx - 1]) * long.Parse(equation[indx + 1])
                    : long.Parse(equation[indx - 1]) + long.Parse(equation[indx + 1]);
                equation.RemoveRange(indx - 1, 3);
                equation.Insert(indx - 1, $"{result}");
            }
            else if (equation.Contains("+") && !isPart1)
            {
                var indx = equation.IndexOf("+");
                var result = long.Parse(equation[indx - 1]) + long.Parse(equation[indx + 1]);
                equation.RemoveRange(indx - 1, 3);
                equation.Insert(indx - 1, $"{result}");
            }
            else if (equation.Contains("*") && !isPart1)
            {
                var indx = equation.IndexOf("*");
                var result = long.Parse(equation[indx - 1]) * long.Parse(equation[indx + 1]);
                equation.RemoveRange(indx - 1, 3);
                equation.Insert(indx - 1, $"{result}");
            }
        }

        return long.Parse(equation[0]);
    }

    private static long CalculateStart(string rawEquation, bool isPart1 = true)
    {
        return Calculate(Regex
            .Replace(
                Regex.Replace(
                    new[] { '+', '*', '(', ')' }.Aggregate(rawEquation,
                        (current, c) => current.Replace($"{c}", $" {c} ")), @"([ ]{2,})", " "), @"(^ | $)",
                string.Empty)
            .Split(" ").ToList(), isPart1);
    }
}