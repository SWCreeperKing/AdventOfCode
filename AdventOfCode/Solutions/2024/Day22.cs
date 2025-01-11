namespace AdventOfCode.Solutions._2024;

file class Day22() : Puzzle<int[]>(2024, 22, "Monkey Market")
{
    public const long Prune = 16777216;
    public override int[] ProcessInput(string input) => input.Split('\n').SelectArr(int.Parse);

    [Answer(19854248602)]
    public override object Part1(int[] inp)
    {
        return inp.Aggregate(0L, (sum, line) =>
        {
            long secret = line;
            for (var i = 0; i < 2000; i++)
            {
                secret = Secret(secret);
            }

            return sum + secret;
        });
    }

    [Answer(2223)]
    public override object Part2(int[] inp)
    {
        Dictionary<int, long> seen = [];
        foreach (var line in inp)
        {
            HashSet<int> localSeen = [];
            List<int> differences = [];
            long secret = line;
            var last = line % 10;
            for (var i = 0; i < 2000; i++)
            {
                secret = Secret(secret);
                var current = (int)(secret % 10);
                differences.Add(current - last);
                last = current;
                if (i < 3) continue;
                var key = HashCode.Combine(differences[i - 3], differences[i - 2], differences[i - 1], differences[i]);
                if (!localSeen.Add(key)) continue;
                seen[key] = seen.GetValueOrDefault(key, 0) + current;
            }
        }

        return seen.Values.Max();
    }

    public static long Secret(long secret)
    {
        secret = ((secret * 64) ^ secret) % Prune;
        secret = ((secret / 32) ^ secret) % Prune;
        return ((secret * 2048) ^ secret) % Prune;
    }
}