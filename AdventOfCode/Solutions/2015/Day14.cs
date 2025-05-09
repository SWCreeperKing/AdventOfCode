namespace AdventOfCode.Solutions._2015;

file class Day14() : Puzzle<Dictionary<string, Day14.Flight>>(2015, 14, "Reindeer Olympics")
{
    private static readonly Regex InputRegex =
        new(@"(\w+) can fly (\d+) km/s for (\d+) seconds, but then must rest for (\d+) seconds\.",
            RegexOptions.Compiled);

    public override Dictionary<string, Flight> ProcessInput(string input)
    {
        return input.Split('\n')
                    .Select(l => InputRegex.Match(l).Range(1..4))
                    .ToDictionary(match => match[0],
                         match => new Flight(int.Parse(match[1]), int.Parse(match[2]), int.Parse(match[3])));
    }

    [Answer(2660)]
    public override object Part1(Dictionary<string, Flight> inp)
    {
        var distance = 0;
        foreach (var (speed, sec, rest) in inp.Values)
        {
            var dist = (int)Math.Floor(2503f / (sec + rest)) * speed * sec;
            if (2503f % (sec + rest) > sec) dist += speed * sec;
            distance = Math.Max(distance, dist);
        }

        return distance;
    }

    [Answer(1256)]
    public override object Part2(Dictionary<string, Flight> inp)
    {
        Dictionary<string, int> canMove = new();
        Dictionary<string, int> distance = new();
        Dictionary<string, int> points = new();

        for (var time = 1; time <= 2503; time++)
        {
            foreach (var deer in inp.Keys)
            {
                canMove.TryAdd(deer, 0);
                if (canMove[deer] >= time) continue;
                var (speed, sec, rest) = inp[deer];
                distance.TryAdd(deer, 0);
                distance[deer] += speed;
                if (canMove[deer] + sec == time) canMove[deer] += sec + rest;
            }

            var max = distance.Values.Max();
            var winners = distance.Where(kv => kv.Value == max).Select(kv => kv.Key);

            foreach (var winner in winners)
                if (!points.TryGetValue(winner, out var value)) points[winner] = 1;
                else points[winner] = ++value;
        }

        return points.Values.Max();
    }

    public record Flight(int Speed, int Sec, int Rest);
}