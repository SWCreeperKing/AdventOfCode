using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;
using AdventOfCode.Experimental_Run.Misc;
using static AdventOfCode.Experimental_Run.Misc.NodeDirection;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 21, "Step Counter")]
file class Day21
{
    [ModifyInput]
    public static Matrix2d<char> ProcessInput(string input)
        => new(input.Split('\n').Select(s => s.ToCharArray()).ToArray());

    [Answer(3847)]
    public static long Part1(Matrix2d<char> inp)
    {
        HashSet<Pos> locations = [inp.Find('S')];

        for (var i = 0; i < 64; i++)
        {
            HashSet<Pos> newLocations = [];
            foreach (var pos in locations)
            {
                AddPos(pos.Move(Up), newLocations);
                AddPos(pos.Move(Down), newLocations);
                AddPos(pos.Move(Left), newLocations);
                AddPos(pos.Move(Right), newLocations);
            }

            locations = newLocations;
        }

        return locations.Count;

        void AddPos(Pos nextPos, HashSet<Pos> list)
        {
            if (list.Contains(nextPos)) return;
            if (!inp.PositionExists(nextPos)) return;
            if (inp[nextPos] is '#') return;
            list.Add(nextPos);
        }
    }

    [Answer(637537341306357)]
    public static long Part2(Matrix2d<char> inp)
    {
        List<long> counter = [];
        HashSet<Pos> locations = [inp.Find('S')];

        var size = inp.Size.w;
        var half = (size - 1) / 2;

        for (var i = 1; i < size * 2 + half + 1; i++)
        {
            HashSet<Pos> newLocations = [];
            foreach (var pos in locations)
            {
                AddPos(pos.Move(Up), newLocations);
                AddPos(pos.Move(Down), newLocations);
                AddPos(pos.Move(Left), newLocations);
                AddPos(pos.Move(Right), newLocations);
            }

            locations = newLocations;

            if (i % size != half) continue;
            counter.Add(newLocations.Count);
        }

        var differences = MakeDifferenceList(counter.ToArray()).Select(arr => arr[^1]).ToArray();
        for (var i = 2; i < (26501365 - half) / size; i++)
        {
            differences[1] += differences[2];
            differences[0] += differences[1];
        }

        return differences[0];

        void AddPos(Pos nextPos, HashSet<Pos> list)
        {
            if (list.Contains(nextPos)) return;
            Pos pos = new((nextPos.X % size + size) % size, (nextPos.Y % size + size) % size);
            if (inp[pos] is '#') return;
            list.Add(nextPos);
        }

        List<List<long>> MakeDifferenceList(long[] arr)
        {
            List<List<long>> history = [arr.ToList()];
            while (history[^1].GroupBy(i => i).Count() > 1)
            {
                history.Add(Differences(history[^1]));
            }

            return history;
        }

        List<long> Differences(List<long> arr)
        {
            List<long> diffArr = [];
            for (var i = 1; i < arr.Count; i++)
            {
                diffArr.Add(arr[i] - arr[i - 1]);
            }

            return diffArr;
        }
    }
}