namespace AdventOfCode.Solutions._2018;

file class Day3() : Puzzle<Day3.Rect[]>(2018, 3, "No Matter How You Slice It")
{
    public static readonly Regex Reg = new(@"#\d+ @ (\d+),(\d+): (\d+)x(\d+)", RegexOptions.Compiled);

    public override Rect[] ProcessInput(string input)
    {
        var split = input.Split('\n');
        var rects = new Rect[split.Length];

        for (var i = 0; i < split.Length; i++)
        {
            var matches = Reg.Match(split[i]).Groups.Range(1..4).Select(int.Parse).ToArray();
            rects[i] = new Rect(new Pos(matches[0], matches[1]), matches[2], matches[3]);
        }

        return rects;
    }

    [Answer(112418)] public override object Part1(Rect[] inp) { return MakeMap(inp).Array.Count(l => l?.Count > 1); }

    [Answer(560)]
    public override object Part2(Rect[] inp)
    {
        var map = MakeMap(inp);
        var singleClaims = map.Array.Where(l => l?.Count == 1).Select(l => l[0]).ToHashSet();
        var multiClaims = map.Array.Where(l => l?.Count > 1)
                             .Aggregate(new List<int>(), (h, l) =>
                              {
                                  h.AddRange(l);
                                  return h;
                              })
                             .ToHashSet();

        singleClaims.RemoveWhere(i => multiClaims.Contains(i));
        return singleClaims.First();
    }

    public static Matrix2d<List<int>> MakeMap(Rect[] inp)
    {
        var maxX = inp.Max(rect => rect.Pos.X + rect.W);
        var maxY = inp.Max(rect => rect.Pos.Y + rect.H);
        Matrix2d<List<int>> map = new(maxX, maxY);

        for (var i = 0; i < inp.Length; i++)
        {
            var (pos, w, h) = inp[i];
            for (var x = pos.X; x < pos.X + w; x++)
            for (var y = pos.Y; y < pos.Y + h; y++)
            {
                map[x, y] ??= [];
                map[x, y].Add(i + 1);
            }
        }

        return map;
    }

    public readonly struct Rect(Pos pos, int w, int h)
    {
        public readonly Pos Pos = pos;
        public readonly int W = w;
        public readonly int H = h;

        public void Deconstruct(out Pos pos, out int w, out int h)
        {
            pos = Pos;
            w = W;
            h = H;
        }
    }
}