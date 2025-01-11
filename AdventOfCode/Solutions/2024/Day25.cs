namespace AdventOfCode.Solutions._2024;

file class Day25() : Puzzle<string>(2024, 25, "Code Chronicle")
{
    public override string ProcessInput(string input) => input;

    [Answer(2815)]
    public override object Part1(string inp)
    {
        List<int[]> locks = [];
        List<int[]> keys = [];

        foreach (var rawMap in inp.Split("\n\n"))
        {
            Matrix2d<char> map = new(rawMap.Split('\n').SelectArr(line => line.ToCharArray()));

            var isLock = map.Array[..5].All(c => c is '#') && map.Array[^5..].All(c => c is '.');
            var heights = new int[5];
            for (var x = 0; x < map.Size.w; x++)
            {
                var depth = -1;
                for (var y = 0; y < map.Size.h; y++)
                {
                    if (map[x, y] is not '#') continue;
                    depth++;
                }

                heights[x] = depth;
            }

            if (isLock)
            {
                locks.Add(heights);
            }
            else
            {
                keys.Add(heights);
            }
        }

        HashSet<string> seen = [];
        foreach (var key in keys)
        {
            foreach (var @lock in locks)
            {
                var isMatch = true;
                for (var i = 0; i < 5; i++)
                {
                    if (key[i] + @lock[i] <= 5) continue;
                    isMatch = false;
                }
                
                if (!isMatch) continue;
                seen.Add($"{key.String()}|>{@lock.String()}");
            }
        }

        return seen.Count;
    }
}