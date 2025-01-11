using CreepyUtil.TreeNode;

namespace AdventOfCode.Solutions._2017;

file class Day7() : Puzzle<Weight>(2017, 7, "Recursive Circus")
{
    public override Weight ProcessInput(string input)
    {
        Weight.ParseMaps(input.Split('\n'), out var childMap,
            out var dataMap,
            line =>
            {
                var split = line.Remove(",").Split(' ');
                return (split[0], split.Length == 2 ? [] : split[3..], int.Parse(split[1][1..^1]));
            });

        return Weight.CreateTree(childMap, k => new Weight(k, dataMap[k]));
    }

    [Answer("cyrupz")] public override object Part1(Weight inp) { return inp.Id; }

    [Answer(193)]
    public override object Part2(Weight inp)
    {
        var isEnd = true;
        foreach (var node in inp.IterateDeepestFirst().Where(w => w.Children is not null))
            node.TotalVal += node.Children.Sum(c => c.TotalVal);

        var w = inp.Climb(
            children => children.SingleOut(w => w.TotalVal).t,
            children => children.DoAllMatch(w => w.TotalVal));

        return w.Parent.Children.SingleOut(n => n.TotalVal).otherKey - (w.TotalVal - w.Val);
    }
}

public class Weight(string id, int val) : TreeNode<Weight>(id)
{
    public readonly int Val = val;
    public int TotalVal = val;
}