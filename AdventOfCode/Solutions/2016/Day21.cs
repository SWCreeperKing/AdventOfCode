namespace AdventOfCode.Solutions._2016;

file class Day21() : Puzzle<string[][]>(2016, 21, "Scrambled Letters and Hash")
{
    public override string[][] ProcessInput(string input) { return input.SuperSplit('\n', ' '); }

    [Answer("baecdfgh")]
    public override object Part1(string[][] inp)
    {
        var abc = "abcdefgh".ToCharArray();

        foreach (var line in inp)
            switch (line)
            {
                case ["swap", "position", var rX, "with", "position", var rY]:
                    Swap(abc, rX, rY);
                    break;
                case ["swap", "letter", var l1, "with", "letter", var l2]:
                    SwapLetter(abc, l1, l2);
                    break;
                case ["rotate", var lOrR, var rawSteps, "step" or "steps"]:
                    Rotate(abc, lOrR, rawSteps);
                    break;
                case ["rotate", "based", "on", "position", "of", "letter", var l]:
                    RotateOnBase(abc, l);
                    break;
                case ["reverse", "positions", var rX, "through", var rY]:
                    Reverse(abc, rX, rY);
                    break;
                case ["move", "position", var rX, "to", "position", var rY]:
                    abc = Move(abc, rX, rY);
                    break;
            }

        return abc.Join();
    }

    [Answer("cegdahbf")]
    public override object Part2(string[][] inp)
    {
        var abc = "fbgdceah".ToCharArray();

        foreach (var line in inp.Reverse())
            switch (line)
            {
                case ["swap", "position", var rX, "with", "position", var rY]:
                    Swap(abc, rX, rY);
                    break;
                case ["swap", "letter", var l1, "with", "letter", var l2]:
                    SwapLetter(abc, l1, l2);
                    break;
                case ["rotate", var lOrR, var rawSteps, "step" or "steps"]:
                    Rotate(abc, lOrR, rawSteps, true);
                    break;
                case ["rotate", "based", "on", "position", "of", "letter", var l]:
                    RotateOnBase(abc, l, true);
                    break;
                case ["reverse", "positions", var rX, "through", var rY]:
                    Reverse(abc, rX, rY);
                    break;
                case ["move", "position", var rX, "to", "position", var rY]:
                    abc = Move(abc, rY, rX);
                    break;
            }

        return abc.Join();
    }

    public static void Swap(char[] abc, string rX, string rY)
    {
        var x = int.Parse(rX);
        var y = int.Parse(rY);
        (abc[x], abc[y]) = (abc[y], abc[x]);
    }

    public static void SwapLetter(char[] abc, string l1, string l2)
    {
        var x = abc.FindIndexOf(l1[0]);
        var y = abc.FindIndexOf(l2[0]);
        (abc[x], abc[y]) = (abc[y], abc[x]);
    }

    public static void Rotate(char[] abc, string lOrR, string rawSteps, bool flip = false)
    {
        var steps = int.Parse(rawSteps) % abc.Length;
        if (steps == 0) return;
        abc.Rotate(steps, flip ? lOrR == "right" : lOrR == "left");
    }

    public static void RotateOnBase(char[] abc, string l, bool isLeft = false)
    {
        var index = abc.FindIndexOf(l[0]);
        var steps = index;
        if (steps >= 4) steps++;

        steps++;

        if (isLeft)
            switch (index) // i don't wanna talk about it
            {
                case 1 or 4:
                    steps--;
                    break;
                case 3:
                    steps -= 2;
                    break;
                case 5:
                    steps -= 4;
                    break;
                case 7 or 2:
                    steps += 3;
                    break;
            }

        abc.Rotate(steps, isLeft);
    }

    public static void Reverse(char[] abc, string rX, string rY)
    {
        var x = int.Parse(rX);
        var y = int.Parse(rY) + 1;
        var reversed = abc[x..y].Reverse();

        for (var i = x; i < y; i++) abc[i] = reversed.ElementAt(i - x);
    }

    public static char[] Move(char[] abc, string rX, string rY)
    {
        var x = int.Parse(rX);
        var list = abc.ToList();
        list.RemoveAt(x);
        list.Insert(int.Parse(rY), abc[x]);
        return list.ToArray();
    }
}