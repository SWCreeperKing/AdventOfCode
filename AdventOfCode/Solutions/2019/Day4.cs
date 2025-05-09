namespace AdventOfCode.Solutions._2019;

file class Day4() : Puzzle<int[]>(2019, 4, "Secure Container")
{
    public override int[] ProcessInput(string input) { return input.Split('-').Select(int.Parse).ToArray(); }

    [Answer(1019)]
    public override object Part1(int[] inp)
    {
        var count = 0;
        for (var i = inp[0]; i < inp[1]; i++)
        {
            var str = $"{i}";
            var isDouble = str.GroupBy(c => c).Any(group => group.Count() >= 2);
            var isDecrease = false;
            for (var j = 1; j < str.Length; j++)
            {
                if (int.Parse(str[j].ToString()) >= int.Parse(str[j - 1].ToString())) continue;
                isDecrease = true;
                break;
            }

            if (!isDouble || isDecrease) continue;
            count++;
        }

        return count;
    }

    [Answer(660)]
    public override object Part2(int[] inp)
    {
        var count = 0;
        for (var i = inp[0]; i < inp[1]; i++)
        {
            var str = $"{i}";
            var isDouble = str.GroupBy(c => c).Any(group => group.Count() == 2);
            var isDecrease = false;
            for (var j = 1; j < str.Length; j++)
            {
                if (int.Parse(str[j].ToString()) >= int.Parse(str[j - 1].ToString())) continue;
                isDecrease = true;
                break;
            }

            if (!isDouble || isDecrease) continue;
            count++;
        }

        return count;
    }
}