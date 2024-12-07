using static System.Linq.Enumerable;

namespace AdventOfCode.Solutions._2017;

[Day(2017, 16, "Permutation Promenade")]
file class Day16
{
    [ModifyInput] public static string[] ProcessInput(string input) { return input.Split(','); }

    [Answer("ionlbkfeajgdmphc")]
    public static string Part1(string[] inp)
    {
        var arr = Range(0, 16).ToList();
        Dance(arr, inp);
        return arr.Select(i => (char)(i + 'a')).Join();
    }

    [Answer("fdnphiegakolcmjb")]
    public static string Part2(string[] inp)
    {
        var (cycles, _, _) = Range(0, 16).GenerateCycleTillRepeat(l => Dance(l, inp));
        var mod = (int)((1e9 - 1) % cycles.Count);
        return cycles[mod].Select(i => (char)(i + 'a')).Join();
    }

    public static void Dance(List<int> arr, string[] inp)
    {
        foreach (var inst in inp)
        {
            int slash, a, b;
            switch (inst[0])
            {
                case 's':
                    var size = int.Parse(inst[1..]);

                    for (var i = 0; i < size; i++)
                    {
                        var n = arr[^1];
                        arr.Remove(n);
                        arr.Insert(0, n);
                    }

                    break;
                case 'x':
                    slash = inst.IndexOf('/');
                    a = int.Parse(inst[1..slash]);
                    b = int.Parse(inst[(slash + 1)..]);
                    (arr[a], arr[b]) = (arr[b], arr[a]);
                    break;
                case 'p':
                    slash = inst.IndexOf('/');
                    a = arr.IndexOf(inst[1..slash][0] - 'a');
                    b = arr.IndexOf(inst[(slash + 1)..][0] - 'a');
                    (arr[a], arr[b]) = (arr[b], arr[a]);
                    break;
            }
        }
    }
}