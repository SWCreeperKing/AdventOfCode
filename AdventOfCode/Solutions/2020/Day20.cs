using System.Linq;

namespace AdventOfCode.Solutions._2020;

file class Day20
{
    // file class Tile
    // {
    //     public int id;
    //     public string[] map;
    //     public string[] sides = new string[4];
    //     public string[] sidesRev;
    //     public bool[] matched = { false, false, false, false };
    //     public int rotation;
    //     public bool[] flip = { false, false }; // {v:h}
    //
    //     public Tile(string[] map)
    //     {
    //         id = int.Parse(Regex.Match(map[0], @"Tile (\d*?):").Groups[1].Value);
    //         map = map[1..];
    //         sides = new[] { map[0], map.Select(s => s[^1]).ToS(), map[^1], map.Select(s => s[0]).ToS() };
    //         sidesRev = sides.Select(s => string.Join("", s.Reverse())).ToArray();
    //         this.map = map;
    //     }
    //
    //     public Tile()
    //     {
    //     }
    //
    //     public void Match(Tile t)
    //     {
    //         if (id == t.id) return;
    //         var rawMatch = sides.Concat(sidesRev).Intersect(t.sides.Concat(t.sidesRev));
    //         if (!rawMatch.Any()) return;
    //         var match = rawMatch.First();
    //         SetMatch(match);
    //         t.SetMatch(match);
    //     }
    //
    //     public void SetMatch(string match)
    //     {
    //         if (sides.Contains(match)) matched[sides.FindIndexOf(match)] = true;
    //         else matched[sidesRev.FindIndexOf(match)] = true;
    //     }
    //
    //     public void Rotate()
    //     {
    //         void Rotate<T>(ref T[] t) => t = new[] { t[3], t[0], t[1], t[2] };
    //         Rotate(ref sides);
    //         Rotate(ref sidesRev);
    //         Rotate(ref matched);
    //         rotation++;
    //         rotation %= 4;
    //         (flip[0], flip[1]) = (flip[1], flip[0]);
    //     }
    //
    //     public void Flip(bool isVertical)
    //     {
    //         flip[isVertical ? 0 : 1] = !flip[isVertical ? 0 : 1];
    //     }
    //
    //     public Tile Copy() =>
    //         new()
    //         {
    //             flip = flip.ToArray(), id = id, map = map.ToArray(), matched = matched.ToArray(),
    //             rotation = rotation, sides = sides.ToArray(), sidesRev = sidesRev.ToArray()
    //         };
    // }

    public class Tile
    {
        public int Id;
        public string RawData;
        public string[] Sides;
        public string[] SidesFlipped;

        public Tile(int id, string rawData)
        {
            (Id, RawData) = (id, rawData);
            var data = rawData.Split('\n');
            Sides = new[] { data[0], data.Select(s => s[^1]).Join(), data[^1], data.Select(s => s[0]).Join() };
            SidesFlipped = Sides.Select(s => s.Reverse().Join()).ToArray();
        }
    }

    // [Run(2020, 20, 1, 7492183537913)]
    public static long Part1(string input)
    {
        // var tiles = input.Split("\n\n").Select(t => new Tile(t.Split('\n', StringSplitOptions.RemoveEmptyEntries)))
        //     .ToList();
        // for (var i = 0; i < tiles.Count; i++)
        // for (var j = i + 1; j < tiles.Count; j++)
        //     tiles[i].Match(tiles[j]);
        // return tiles.Where(t => t.matched.Count(b => b) == 2).Aggregate(1L, (l, t) => l * t.id);
        return -1;
    }

    // [Run(2020, 20, 2, 2323)]
    public static long Part2(string input)
    {
        // var seaMonster = new[] { "                  # ", "#    ##    ##    ###", "#  #  #  #  #  #   " };
        // var tiles = input.Split("\n\n")
        //     .Select(t => new Tile(t.Split('\n', StringSplitOptions.RemoveEmptyEntries)))
        //     .ToList();
        // for (var i = 0; i < tiles.Count; i++)
        // for (var j = i + 1; j < tiles.Count; j++)
        //     tiles[i].Match(tiles[j]);
        // var corners = tiles.Where(t => t.matched.Count(b => b) == 2);
        // string[] build = null;
        // foreach (var chosen in corners)
        // {
        //     while (chosen.matched[0] || chosen.matched[3]) chosen.Rotate();
        //     build = Part2Build(chosen.Copy(), tiles.ToArray(), 0, 0, (int)Math.Sqrt(tiles.Count),
        //         Array.Empty<int>(), tiles.ToDictionary(t => t.id, t => t), Array.Empty<int>());
        //     Console.WriteLine($"Failed at Testing Corner: {chosen.id}");
        // }
        //
        // Console.WriteLine(string.Join("\n", build!));
        //
        return -1;
    }

    // public static string[] Part2Build(Tile start, Tile[] tiles, int x, int y, int size, int[] last,
    //     Dictionary<int, Tile> lookup, int[] chain)
    // {
    //     Console.WriteLine($"Testing: {start.id} at {x},{y}");
    //     List<string> retur = new() { $"{start.id}," };
    //     if (x == size - 1 && y == size - 1) return new[] { $"{start.id}" };
    //     var ts = tiles.ToList();
    //     ts.RemoveAll(t => t.id == start.id);
    //     var nChain = chain.ToList();
    //     nChain.Add(start.id);
    //
    //     var arrRight = new[] { start.sides[1], start.sidesRev[1] };
    //     string[] arrTop = null;
    //     if (last.Any())
    //     {
    //         var lastT = lookup[last[x]];
    //         arrTop = new[] { lastT.sides[2], lastT.sidesRev[2] };
    //     }
    //
    //     string Match(Tile t, string[] c1, string[] cTop = null)
    //     {
    //         var mainC = t.sides.Concat(t.sidesRev);
    //         var match = c1.Intersect(mainC);
    //         if (!match.Any()) return "";
    //         if (cTop is null) return match.First();
    //         var sMatch = mainC.Intersect(cTop);
    //         return !sMatch.Any() ? "" : match.First();
    //     }
    //
    //     void MatchToS(Tile t, string match, int side = 3)
    //     {
    //         if (t.sides.Contains(match))
    //             while (t.sides[side] != match)
    //                 t.Rotate();
    //         else
    //         {
    //             while (t.sidesRev[side] != match) t.Rotate();
    //             t.Flip(false);
    //         }
    //     }
    //
    //     foreach (var t in ts) // check continue line
    //     {
    //         var theMatch = Match(t, arrRight, arrTop);
    //         if (theMatch == "") continue;
    //         var newT = t.Copy();
    //         MatchToS(newT, theMatch);
    //
    //         Console.WriteLine($"potential match: {newT.id}");
    //
    //         var found = Part2Build(newT, ts.ToArray(), x + 1, y, size, last, lookup, nChain.ToArray());
    //         if (found is null) continue;
    //         retur[0] += found[0];
    //         retur.AddRange(found[1..]);
    //         return retur.ToArray();
    //     }
    //
    //     if (x != size - 1) return null;
    //     Console.WriteLine("Moving to Next Row");
    //     var leftT = lookup[chain[0]];
    //     var arrBottom = new[] { leftT.sides[2], leftT.sidesRev[2] };
    //     foreach (var t in ts) // check new line
    //     {
    //         var theMatch = Match(t, arrBottom);
    //         if (theMatch == "") continue;
    //         var newT = t.Copy();
    //         MatchToS(newT, theMatch, 0);
    //
    //         var found = Part2Build(newT, ts.ToArray(), 0, y + 1, size, chain, lookup,
    //             Array.Empty<int>());
    //         if (found is null) continue;
    //         retur.AddRange(found);
    //         return retur.ToArray();
    //     }
    //
    //     return null;
    // }
}