using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2021;

public class eDay10 : Puzzle<string, long>
{
    public override (long part1, long part2) Result { get; } = (271245, 1685293086);
    public override (int year, int day) PuzzleSolution { get; } = (2021, 10);
    public override string ProcessInput(string input) => input;

    public override long Part1(string inp)
    {
        var wrong = 0;
        foreach (var line in inp.Split('\n'))
        {
            Stack<char> groups = new();
            foreach (var c in line)
            {
                if (c is '(' or '[' or '{' or '<') groups.Push(c);
                else if (groups.Peek() switch
                         {
                             '(' => ')',
                             '{' => '}',
                             '<' => '>',
                             '[' => ']'
                         } != c)
                {
                    wrong += c switch
                    {
                        ')' => 3,
                        ']' => 57,
                        '}' => 1197,
                        '>' => 25137
                    };
                    break;
                }
                else groups.Pop();
            }
        }

        return wrong;
    }

    public override long Part2(string inp)
    {
        List<long> incomplete = new();
        foreach (var line in inp.Split('\n'))
        {
            Stack<char> groups = new();
            var cc = true;
            foreach (var c in line)
            {
                if (c is '(' or '[' or '{' or '<') groups.Push(c);
                else if (groups.Peek() switch
                         {
                             '(' => ')',
                             '{' => '}',
                             '<' => '>',
                             '[' => ']'
                         } != c)
                {
                    cc = false;
                    break;
                }
                else groups.Pop();
            }

            if (!cc) continue;
            if (groups.Count == 0) continue;

            var score = 0L;
            while (groups.Count != 0)
            {
                var c = groups.Pop();
                score *= 5;
                score += c switch
                {
                    '(' => 1,
                    '[' => 2,
                    '{' => 3,
                    '<' => 4
                };
            }

            incomplete.Add(score);
        }

        incomplete = incomplete.OrderBy(i => i).ToList();
        return incomplete[incomplete.Count / 2];
    }
}