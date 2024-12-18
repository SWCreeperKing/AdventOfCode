using AdventOfCode.Experimental_Run.Misc;
using static CreepyUtil.Direction;

namespace AdventOfCode.Solutions._2024;

[Day(2024, 16, "Reindeer Maze")]
file class Day16
{
    [ModifyInput] public static string ProcessInput(string input) => input;

    [Answer(90460)]
    public static long Part1(string inp)
    {
        Matrix2d<char> map = new(inp.Split('\n').SelectArr(line => line.ToCharArray()));
        var start = map.Find(c => c is 'S');
        var end = map.Find(c => c is 'E');
        var dijkstra = new Dijkstra<State, char, int>(map, (a, b) => a.CompareTo(b));
        var finish = dijkstra.Eval(end, new State(start, Center, []));
        return finish.Value;
    }

    [Answer(575)]
    public static long Part2(string inp)
    {
        Matrix2d<char> map = new(inp.Split('\n').SelectArr(line => line.ToCharArray()));
        var start = map.Find(c => c is 'S');
        var end = map.Find(c => c is 'E');
        var dijkstra = new Dijkstra<State, char, int>(map, (a, b) => a.CompareTo(b));
        var finish = dijkstra.Eval(end, new State(start, Direction.Center, [start]));
        var paths = finish.Path.ToList();
        List<Pos> deadEnds = [];
        List<(Pos, Pos)> deadEnds2 = [];
        State[] next;

        while ((next = FindPathWithIntersections(paths)).Length != 0)
        {
            foreach (var set in next)
            {
                paths.AddRange(set.Path);
            }

            paths = paths.Distinct().ToList();
        }

        return paths.Count;

        State[] FindPathWithIntersections(List<Pos> paths)
        {
            List<Pos> intersections = [];
            List<Pos> processed = [];

            foreach (var pos in paths)
            {
                processed.Add(pos);
                var surround = Pos.Surround.Select(dxy => dxy + pos)
                                  .Where(next => map.PositionExists(next) && !processed.Contains(next))
                                  .ToArray();

                var neigbors = surround.Select(p => map[p]).ToArray();

                if (neigbors.Count(n => n == '.') > 1)
                {
                    intersections.AddRange(surround.Where(p => map[p] == '.' && paths.Contains(p)));
                }
            }

            intersections = intersections.Distinct().ToList();

            List<State> candidates = [];
            Parallel.For(0, intersections.Count, i =>
            {
                Matrix2d<char> copy = new(map.Array, map.Size);
                if (deadEnds.Contains(intersections[i])) return;
                copy[intersections[i]] = '#';
                for (var j = i; j < intersections.Count; j++)
                {
                    if (deadEnds.Contains(intersections[j])) continue;
                    var set = (intersections[i], intersections[j]);
                    if (deadEnds2.Contains(set)) continue;
                    
                    copy[intersections[j]] = '#';
                    try
                    {
                        var dijkstra = new Dijkstra<State, char, int>(copy, (a, b) => a.CompareTo(b));
                        var possible = dijkstra.Eval(end, new State(start, Center, [start]));
                        if (possible.Value > finish.Value && i == j)
                        {
                            deadEnds.Add(intersections[i]);
                        }
                        else if (possible.Value == finish.Value)
                        {
                            deadEnds2.Add(set);
                        }

                        lock (candidates)
                        {
                            candidates.Add(possible);
                        }
                    }
                    catch
                    {
                        if (i == j)
                        {
                            deadEnds.Add(intersections[i]);
                        }
                        else
                        {
                            deadEnds2.Add(set);
                        }
                    }

                    copy[intersections[j]] = '.';
                }

                copy[intersections[i]] = '.';
            });

            return candidates.Where(state => state.Path.Any(p => !paths.Contains(p)) && state.Value == finish.Value)
                             .ToArray();
        }
    }
}

file class State(Pos position, Direction direction, Pos[] path, int value = 0)
    : State<State, char, int>(position, direction)
{
    public Pos[] Path = path;
    public int Value = value;

    public override int GetValue(char mapVal) { return Value; }

    public override State MakeNewState(Matrix2d<char> map, Pos newPos, Direction dir)
    {
        return new State(newPos, dir, [..Path, newPos], Value + 1 + (dir != Direction ? 1000 : 0));
    }

    public override bool ValidState(Matrix2d<char> map, Direction dir, Pos dxy)
    {
        return dir != Direction.Rotate180() && map[Position + dxy] != '#';
    }
}