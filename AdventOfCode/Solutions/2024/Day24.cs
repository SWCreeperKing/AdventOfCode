using System.IO;
using System.Text;

namespace AdventOfCode.Solutions._2024;

[Day(2024, 24, "Crossed Wires")]
file class Day24
{
    [ModifyInput] public static string ProcessInput(string input) => input;

    [Copy]
    [Answer(51410244478064)]
    public static long Part1(string inp)
    {
        var sep = inp.Split("\n\n");
        var dict = sep[0]
                  .Split('\n')
                  .SelectArr(line => line.Split(": "))
                  .ToDictionary(arr => arr[0], arr => long.Parse(arr[1]));

        var list = sep[1].SuperSplit('\n', ' ');
        Queue<(string a, string op, string b, string output)> queue = [];

        foreach (var line in list)
        {
            if (line is not [var a, var op, var b, "->", var output]) throw new Exception("E");
            queue.Enqueue((a, op, b, output));
        }

        Dictionary<string, (string a, string op, string b, string output)> waitForA = [];
        Dictionary<string, (string a, string op, string b, string output)> waitForB = [];

        while (queue.Count > 0)
        {
            var item = queue.Dequeue();
            var (a, op, b, output) = item;

            if (!dict.ContainsKey(a) || !dict.ContainsKey(b))
            {
                queue.Enqueue(item);
                continue;
            }

            dict[output] = op switch
            {
                "OR" => dict[a] | dict[b],
                "AND" => dict[a] & dict[b],
                "XOR" => dict[a] ^ dict[b]
            };
        }

        var zs = dict.Where(kv => kv.Key.StartsWith('z'))
                     .Select(kv => (int.Parse(kv.Key[1..]), kv.Value))
                     .OrderBy(t => t.Item1)
                     .Select(t => $"{t.Value}")
                     .Join();

        return Convert.ToInt64(zs.Rever().Join(), 2);
    }

    [Copy]
    [Answer("gst,khg,nhn,tvb,vdc,z12,z21,z33")]
    public static string Part2(string inp)
    {
        var sep = inp.Split("\n\n");
        var list = sep[1].SuperSplit('\n', ' ');
        var rawDict = sep[0]
                     .Split('\n')
                     .SelectArr(line => line.Split(": "))
                     .ToDictionary(arr => arr[0], arr => long.Parse(arr[1]));

        string[][] swapPairs = // did manually
        [ 
            ["z12", "vdc"],
            ["z21", "nhn"],
            ["z33", "gst"],
            ["tvb", "khg"]
        ];
        
        Dictionary<string, string> swap = [];

        foreach (var arr in swapPairs)
        {
            swap[arr[0]] = arr[1];
            swap[arr[1]] = arr[0];
        }

        // HashSet<string> set = [];
        //
        // foreach (var line in list)
        // {
        //     var key = swap.TryGetValue(line[4], out var value) ? value : line[4];
        //
        //     set.Add($"{{{line[0]}}} -> {{{key}}} [label={line[1]}]");
        //     set.Add($"{{{line[2]}}} -> {{{key}}} [label={line[1]}]");
        // }
        //
        // File.WriteAllText("out.dot", $"strict digraph {{\n  {set.Join("\n  ")}\n}}");
        // dot -Tpng out.dot > output.png
        // THANKS GRAPHVIS
        
        var dict = Calc(list, rawDict.ToDictionary(kv => kv.Key, kv => kv.Value), swap);

        var xs = GetNumber('x', dict);
        var ys = GetNumber('y', dict);
        var zs = GetNumber('z', dict);
        var num = Convert.ToInt64(xs.Join(), 2) + Convert.ToInt64(ys.Join(), 2);
        var s1 = Convert.ToString(num, 2).Join();
        var s2 = zs.Join();

        StringBuilder s3 = new();
        StringBuilder s4 = new();
        for (var i = 0; i < s1.Length; i++)
        {
            if (s1[i] == s2[i])
            {
                s3.Append($"[#green]{s1[i]}");
                s4.Append($"[#green]{s2[i]}");
                continue;
            }
        
            s3.Append($"[#red]{s1[i]}");
            s4.Append($"[#red]{s2[i]}");
        }
        
        WriteLine(s3.ToString());
        WriteLine(s4.ToString());

        return ((string[])["z12", "vdc", "z21", "nhn", "z33", "gst", "tvb", "khg"]).Order().String()[1..^1];
    }

    public static string GetNumber(char num, Dictionary<string, long> dict)
    {
        return dict.Where(kv => kv.Key.StartsWith(num))
                   .Select(kv => (int.Parse(kv.Key[1..]), kv.Value))
                   .OrderByDescending(t => t.Item1)
                   .Select(t => $"{t.Value}")
                   .Join();
    }

    public static Dictionary<string, long> Calc(string[][] list, Dictionary<string, long> dict,
        Dictionary<string, string> swap
    )
    {
        Queue<(string a, string op, string b, string output)> queue = [];

        foreach (var line in list)
        {
            if (line is not [var a, var op, var b, "->", var output]) throw new Exception("E");
            if (swap.TryGetValue(output, out var value))
            {
                output = value;
            }

            queue.Enqueue((a, op, b, output));
        }

        while (queue.Count > 0)
        {
            var item = queue.Dequeue();
            var (a, op, b, output) = item;

            if (!dict.ContainsKey(a) || !dict.ContainsKey(b))
            {
                queue.Enqueue(item);
                continue;
            }

            dict[output] = op switch
            {
                "OR" => dict[a] | dict[b],
                "AND" => dict[a] & dict[b],
                "XOR" => dict[a] ^ dict[b]
            };
        }

        return dict;
    }
}