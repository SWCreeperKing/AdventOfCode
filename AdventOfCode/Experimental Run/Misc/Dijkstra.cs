using System;
using System.Collections.Generic;
using System.Linq;
using static AdventOfCode.Experimental_Run.Misc.NodeDirection;

namespace AdventOfCode.Experimental_Run.Misc;

// remember: a < b = -1 | a == b = 0 | a > b = 1
public class Dijkstra<T, TM, TCompare>(Matrix2d<TM> set, Func<TCompare, TCompare, int> comparer)
    where T : State<T, TM, TCompare>
{
    public readonly List<NodeDirection> MovingDirections = [Up, Right, Down, Left];

    private readonly HashSet<int> Seen = [];
    private readonly PriorityQueue<T, TCompare> Check = new(new Comparer<TCompare>(comparer));

    public T Eval(Pos dest, params T[] starters)
    {
        if (!set.PositionExists(dest)) throw new ArgumentException("Destination is not in Map");
        Check.EnqueueRange(starters.Select(state => (state, state.GetValue(set[state.Position]))));

        var p = ClrCnsl.GetCursor();
        
        while (Check.Count > 0)
        {
            ClrCnsl.SetCursor(p);
            Console.WriteLine($"{Check.Count}             ");
            var state = Check.Dequeue();
            if (!Seen.Add(state.GetHashCode())) continue;

            var pos = state.Position;
            if (!set.PositionExists(pos)) continue;
            if (state.IsFinal(dest, state, set[pos])) return state;

            foreach (var dir in MovingDirections)
            {
                var dxy = dir.Positional();
                var newPos = pos + dxy;
                if (!set.PositionExists(newPos)) continue;
                if (!state.ValidState(set, dir, dxy)) continue;

                state.MakeNewState(set, newPos, dir)
                    .InlineNoReturn(newState => Check.Enqueue(newState, newState.GetValue(set[newPos])));
            }
        }

        throw new Exception("Map is uncalculable");
    }
}

public abstract class State<T, TM, TCompare>(Pos position, NodeDirection direction)
    where T : State<T, TM, TCompare>
{
    public readonly NodeDirection Direction = direction;
    public readonly Pos Position = position;

    public abstract override int GetHashCode();
    public abstract TCompare GetValue(TM mapVal);
    public abstract T MakeNewState(Matrix2d<TM> map, Pos newPos, NodeDirection dir);
    public virtual bool ValidState(Matrix2d<TM> map, NodeDirection dir, Pos dxy) => true;
    public virtual bool IsFinal(Pos dest, State<T, TM, TCompare> state, TM val) => Position == dest;
}

file class Comparer<T>(Func<T, T, int> compare) : IComparer<T>
{
    public int Compare(T a, T b) => compare(a, b);
}