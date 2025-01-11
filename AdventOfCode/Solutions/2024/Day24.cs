using System.IO;
using System.Text;

namespace AdventOfCode.Solutions._2024;

file class Day24() : Puzzle<(Dictionary<string, long> dict, string[][] list)>(2024, 24, "Crossed Wires")
{
    public override (Dictionary<string, long> dict, string[][] list) ProcessInput(string input)
    {
        var sep = input.Split("\n\n");
        var dict = sep[0]
                  .Split('\n')
                  .SelectArr(line => line.Split(": "))
                  .ToDictionary(arr => arr[0], arr => long.Parse(arr[1]));

        var list = sep[1].SuperSplit('\n', ' ');
        return (dict, list);
    }

    [Answer(51410244478064)]
    public override object Part1((Dictionary<string, long> dict, string[][] list) inp)
    {
        return Convert.ToInt64(GetNumber('z', Calc(inp.list, inp.dict, [])), 2);
    }

    [Answer("gst,khg,nhn,tvb,vdc,z12,z21,z33")]
    public override object Part2((Dictionary<string, long> dict, string[][] list) inp)
    {
        return inp.list.Where(line =>
                   {
                       if (line[4].StartsWith('z') && line[1] is not "XOR" && line[4] != "z45") return true;
                       if (!line[4].StartsWith('z')
                           && line[1] is "XOR"
                           && line[0][0] is not ('x' or 'y')
                           && line[2][0] is not ('x' or 'y'))
                       {
                           return true;
                       }

                       if (line[1] is "AND"
                           && line[0] != "x00"
                           && line[2] != "x00"
                           && inp.list.Any(l => (l[0] == line[4] || l[2] == line[4]) && l[1] is not "OR"))
                       {
                           return true;
                       }

                       return line[1] is "XOR" &&
                              inp.list.Any(l => (l[0] == line[4] || l[2] == line[4]) && l[1] is "OR");
                   })
                  .Select(line => line[4])
                  .Order()
                  .Join(',');
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