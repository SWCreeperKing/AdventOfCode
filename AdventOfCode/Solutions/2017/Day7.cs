using System.Linq;
using AdventOfCode.Experimental_Run;
using CreepyUtil.TreeNode;

namespace AdventOfCode.Solutions._2017;

[Day(2017, 7, "Recursive Circus")]
file class Day7
{
    [ModifyInput]
    public static Weight ProcessInput(string input)
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

    [Answer("cyrupz")] public static string Part1(Weight inp) { return inp.Id; }

    [Answer(193)]
    public static long Part2(Weight inp)
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