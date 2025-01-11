namespace AdventOfCode.Solutions._2016;

// https://www.reddit.com/r/adventofcode/comments/5hoia9/comment/db1v0hi/?utm_source=share&utm_medium=web3x&utm_name=web3xcss&utm_term=1&utm_content=share_button
// from u/bovard on reddit:
//     I also discovered to move n items up 1 floor,
// it requires 2 * (n - 1) - 1 moves

// i didn't look at their solution i saw the equation and made my own solution 
file class Day11() : Puzzle<int[]>(2016, 11, "Radioisotope Thermoelectric Generators")
{
    public override int[] ProcessInput(string input)
    {
        return input.Split('\n').Select(s => s.Split(' ').Count(c => c == "a")).ToArray();
    }

    [Answer(37)]
    public override object Part1(int[] inp)
    {
        var steps = 0;
        for (var i = 0; i < inp.Length - 1; i++)
        {
            steps += 2 * (inp[i] - 1) - 1;
            inp[i + 1] += inp[i];
        }

        return steps;
    }

    [Answer(61)] public override object Part2(int[] inp) { return Part1([inp[0] + 4, inp[1], inp[2], inp[3]]); }
}