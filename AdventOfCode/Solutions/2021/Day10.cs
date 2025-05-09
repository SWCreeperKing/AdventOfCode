namespace AdventOfCode.Solutions._2021;

file class Day10() : Puzzle<string>(2021, 10, "Syntax Scoring")
{
    public override string ProcessInput(string input) { return input; }

    [Answer(271245)]
    public override object Part1(string inp)
    {
        var wrong = 0;
        foreach (var line in inp.Split('\n'))
        {
            Stack<char> groups = new();
            foreach (var c in line)
                if (c is '(' or '[' or '{' or '<')
                {
                    groups.Push(c);
                }
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
                else
                {
                    groups.Pop();
                }
        }

        return wrong;
    }

    [Answer(1685293086)]
    public override object Part2(string inp)
    {
        List<long> incomplete = [];
        foreach (var line in inp.Split('\n'))
        {
            Stack<char> groups = new();
            var cc = true;
            foreach (var c in line)
                if (c is '(' or '[' or '{' or '<')
                {
                    groups.Push(c);
                }
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
                else
                {
                    groups.Pop();
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