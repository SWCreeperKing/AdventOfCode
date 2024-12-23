using static CreepyUtil.Direction;

namespace AdventOfCode.Experimental_Run.Misc;

// remember: a < b = -1 | a == b = 0 | a > b = 1
public class Dijkstra<T, TM, TCompare>(Matrix2d<TM> set, Func<TCompare, TCompare, int> comparer)
    where T : State<T, TM, TCompare>
{
    private readonly PriorityQueue<T, TCompare> Check = new(new Comparer<TCompare>(comparer));
    public readonly List<Direction> MovingDirections = [Up, Right, Down, Left];

    private readonly HashSet<int> Seen = [];

    public T Eval(Pos dest, params T[] starters)
    {
        if (!set.PositionExists(dest)) throw new ArgumentException("Destination is not in Map");
        Check.EnqueueRange(starters.Select(state => (state, state.GetValue(set[state.Position]))));

        while (Check.Count > 0)
        {
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

                var newState = state.MakeNewState(set, newPos, dir);
                newState?.InlineNoReturn(newState => Check.Enqueue(newState, newState.GetValue(set[newPos])));
            }
        }

        throw new Exception("Map is uncalculable");
    }
}

public abstract class State<T, TM, TCompare>(Pos position, Direction direction)
    where T : State<T, TM, TCompare>
{
    public readonly Direction Direction = direction;
    public readonly Pos Position = position;

    public override int GetHashCode() { return Position.GetHashCode(); }
    public abstract TCompare GetValue(TM mapVal);
    public abstract T MakeNewState(Matrix2d<TM> map, Pos newPos, Direction dir);

    public virtual bool ValidState(Matrix2d<TM> map, Direction dir, Pos dxy) { return true; }

    public virtual bool IsFinal(Pos dest, State<T, TM, TCompare> state, TM val) { return Position == dest; }
}

public class Comparer<T>(Func<T, T, int> compare) : IComparer<T>
{
    public int Compare(T a, T b) { return compare(a, b); }
}