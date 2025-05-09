namespace AdventOfCode.Solutions._2017;

file class Day14() : Puzzle<string>(2017, 14, "Disk Defragmentation")
{
    public override string ProcessInput(string input) { return input; }

    [Answer(8074)]
    public override object Part1(string inp)
    {
        var count = 0L;
        for (var i = 0; i < 128; i++)
            count = Day10.Part2($"{inp}-{i}")
                         .Select(hex => Convert.ToString(Convert.ToInt32(hex.ToString(), 16), 2))
                         .Aggregate(count, (current, binary) => current + binary.Count(c => c == '1'));

        return count;
    }

    [Answer(1212)]
    public override object Part2(string inp)
    {
        var map = new Matrix2d<char>(128).MatrixSelect(_ => '0');

        for (var i = 0; i < 128; i++)
        {
            var line = Day10.Part2($"{inp}-{i}")
                            .Select(hex => Convert.ToString(Convert.ToInt32(hex.ToString(), 16), 2))
                            .Aggregate("", (current, binary) => $"{current}{int.Parse(binary):0000}");

            for (var j = 0; j < line.Length; j++) map[j, i] = line[j];
        }

        var count = 0;
        while (map.Array.Contains('1'))
        {
            HashSet<Pos> seen = [];
            Queue<Pos> scan = [];
            var start = map.Find('1');
            scan.Enqueue(start);
            seen.Add(start);

            while (scan.Count != 0)
            {
                var pos = scan.Dequeue();
                map[pos] = '#';

                foreach (var newPos in Pos.Surround.Select(dxy => dxy + pos).Where(map.PositionExists))
                {
                    if (pos == newPos) continue;
                    if (!seen.Add(newPos)) continue;
                    if (map[newPos] != '1') continue;
                    scan.Enqueue(newPos);
                }
            }

            count++;
        }

        return count;
    }
}