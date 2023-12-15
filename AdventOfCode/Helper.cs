using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public static class Helper
{
    public static Regex NumberOnlyRegex = new(@"^\d+$", RegexOptions.Compiled);
    public static Regex SpaceRegex = new(@"\s+", RegexOptions.Compiled);

    public static char ToChar(this int i, int offset = 97) => (char) (i + offset);
    public static bool IsInRange(this int i, int min, int max) => min <= i && max >= i;
    public static bool IsInRange(this long l, int min, int max) => min <= l && max >= l;
    public static bool IsInRange(this long l, long min, long max) => min <= l && max >= l;
    public static int[] ToIntArr(this IEnumerable<string> texts) => texts.Select(int.Parse).ToArray();
    public static long[] ToLongArr(this IEnumerable<string> texts) => texts.Select(long.Parse).ToArray();
    public static string ReplaceWithSpace(this string text, string pattern) => text.Replace(pattern, " ");
    public static string Remove(this string text, string pattern) => text.Replace(pattern, string.Empty);
    public static string[] SplitSpace(this string text) => text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    public static bool IsAllNumbers(this string text) => NumberOnlyRegex.IsMatch(text);
    public static int FindIndexOf<T>(this IEnumerable<T> arr, T find) => arr.ToList().FindIndex(t => t.Equals(find));
    public static T Multi<T>(this IEnumerable<T> arr) where T : INumber<T> => arr.Aggregate((l1, l2) => l1 * l2);
    public static IEnumerable<T> EvenIndexes<T>(this IEnumerable<T> arr) => arr.Where((_, i) => i % 2 == 0);
    public static IEnumerable<T> OddIndexes<T>(this IEnumerable<T> arr) => arr.Where((_, i) => i % 2 == 1);
    public static int ToInt(this JsonNode node) => node.GetValue<int>();
    public static string ToStr<T>(this IEnumerable<T> obj) => $"[{string.Join(",", obj)}]";

    public static int FindLastIndexOf<T>(this IEnumerable<T> arr, T find)
        => arr.ToList().FindLastIndex(t => t.Equals(find));

    public static string Remove(this string text, params string[] pattern)
        => pattern.Aggregate(text, (s, p) => s.Remove(p));

    public static string[] Range(this GroupCollection gc, Range range)
    {
        var arr = new string[range.End.Value - (range.Start.Value - 1)];
        for (int i = range.Start.Value, j = 0; j < arr.Length; i++, j++) arr[j] = gc[i].Value;
        return arr;
    }

    public static string[] Range(this Match match, Range range) => match.Groups.Range(range);

    public static Dictionary<TK, TV> ToDictionary<TK, TV>(this IEnumerable<KeyValuePair<TK, TV>> pair)
        => pair.ToDictionary(kv => kv.Key, kv => kv.Value);

    public static void ForEach<T>(this IEnumerable<T> arr, Action<T> action)
    {
        foreach (var element in arr) action(element);
    }

    public static void ForEach<T>(this IEnumerable<T> arr, Action<T, int> action)
    {
        for (var i = 0; i < arr.Count(); i++)
        {
            action(arr.ElementAt(i), i);
        }
    }

    public static T AddReturn<T>(this IList<T> arr, T val)
    {
        arr.Add(val);
        return val;
    }

    public static bool IsSequential(params int[] arr)
    {
        for (var i = 1; i < arr.Length; i++)
        {
            if (arr[i - 1] + 1 != arr[i]) return false;
        }

        return true;
    }

    public static bool IsSequential(Span<int> arr)
    {
        for (var i = 1; i < arr.Length; i++)
        {
            if (arr[i - 1] + 1 != arr[i]) return false;
        }

        return true;
    }

    public static T[] SubArr<T>(this IEnumerable<T> arr, int start, int end)
        => arr.Skip(start).Take(end - start).ToArray();

    public static int[] FindAllIndexesOf<T>(this IEnumerable<T> arr, T search)
        => arr.Select((o, i) => Equals(search, o) ? i : -1).Where(i => i != -1).ToArray();

    public static T[] Append<T>(this IEnumerable<T> arr, IEnumerable<T> appender)
    {
        var l = arr.ToList();
        l.AddRange(appender);
        return l.ToArray();
    }

    public static T[] Combine<T>(this T[] core, T[] subCore, Func<T, T, T> condition)
    {
        var minLeng = Math.Min(core.Length, subCore.Length);
        var together = new T[minLeng];
        for (var i = 0; i < minLeng; i++)
        {
            together[i] = condition.Invoke(core[i], subCore[i]);
        }

        return together;
    }

    public static T[] Swap<T>(this IEnumerable<T> rawArr, int pos1, int pos2)
    {
        var arr = rawArr.ToArray();
        (arr[pos1], arr[pos2]) = (arr[pos2], arr[pos1]);
        return arr;
    }

    public static bool Contains<T>(this IEnumerable<T> arr, IEnumerable<T> arr2) => arr2.Any(arr.Contains);
    public static string Join<T>(this IEnumerable<T> carr, string join = "") => string.Join(join, carr);
    public static string Join<T>(this IEnumerable<T> carr, char join) => string.Join(join, carr);

    public static IEnumerable<T> Window<T>(this T[] arr, int grouping, Func<IEnumerable<T>, T> condense)
        => arr.Skip(grouping - 1).Select((_, i) => condense.Invoke(arr[i..(i + grouping)]));

    public static IEnumerable<IEnumerable<T>> Window<T>(this IEnumerable<T> arr, int grouping, bool before = false)
    {
        for (var i = 0; i < arr.Count(); i++)
        {
            if (!before && i < grouping) continue;
            yield return arr.Take(Math.Max(0, i - grouping)..i);
        }
    }

    public static int FirstIndexWhere<T>(this IEnumerable<T> arr, Func<T, bool> where)
        => arr.Select((t, i) => (t, i)).First(ti => where(ti.t)).i;

    public static int Unique<T>(this IEnumerable<T> arr) => arr.Distinct().Count();

    public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IEnumerable<T> list, int leng = -1)
        => leng == 1
            ? list.Select(t => new[] { t })
            : GetPermutations(list, (leng == -1 ? list.Count() : leng) - 1)
                .SelectMany(t => list.Where(o => !t.Contains(o)), (t1, t2) => t1.Concat(new[] { t2 }));

    public static T[][] GetPermutationsArr<T>(this IEnumerable<T> list, int leng = -1)
        => leng == 1
            ? list.Select(t => new[] { t }).ToArray()
            : GetPermutationsArr(list, (leng == -1 ? list.Count() : leng) - 1)
                .SelectMany(t => list.Where(o => !t.Contains(o)), (t1, t2) => t1.Concat(new[] { t2 }).ToArray())
                .ToArray();

    public static TValue GetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key,
        TValue def = default)
        => dict.GetValueOrDefault(key, def);

    public static IEnumerable<IReadOnlyList<bool>> SwitchingBool(int arrLeng, int maxAmount = -1, int minAmount = 0)
    {
        var arr = new List<bool>();
        for (var i = 0; i < arrLeng; i++) arr.Add(false);
        if (minAmount == 0) yield return arr;

        while (!arr.All(b => b))
        {
            arr.SwitchBool();
            var truCount = arr.Count(b => b);
            if (truCount >= minAmount && (maxAmount == -1 || truCount <= maxAmount)) yield return arr;
        }
    }

    public static void SwitchBool(this IList<bool> flags, int index = 0)
    {
        if (index >= flags.Count) throw new IndexOutOfRangeException("index is greater than array");
        if (flags[index]) SwitchBool(flags, index + 1);
        flags[index] = !flags[index];
    }

    public static IEnumerable<T> GetFrom<T>(this IEnumerable<T> arr, IReadOnlyList<bool> boolArr)
    {
        if (arr.Count() != boolArr.Count) throw new ArgumentException($"GetFrom error: {arr.Count()} != {boolArr}");
        return arr.Where((_, i) => boolArr.ElementAt(i));
    }

    public static string Time(this Stopwatch sw)
    {
        var elapsed = sw.Elapsed;
        StringBuilder sb = new();
        if (elapsed.Hours > 0) sb.Append(elapsed.Hours).Append("hr ");
        if (elapsed.Minutes > 0) sb.Append(elapsed.Minutes).Append("min ");
        if (elapsed.Seconds > 0) sb.Append(elapsed.Seconds).Append("sec ");
        if (elapsed.Milliseconds > 0) sb.Append(elapsed.Milliseconds).Append("ms ");

        var ns = (elapsed.Nanoseconds + elapsed.Microseconds * 1000f) / 100 / 10f;
        if (ns > 0) sb.Append($"{ns:###.#}µs");
        return sb.ToString().TrimEnd();
    }

    public static string String<T>(this IEnumerable<T> arr) => $"[{arr.Join(',')}]";

    public static string String<T, TK>(this IEnumerable<T> arr, Func<T, TK> select)
        => $"[{arr.Select(select).Join(',')}]";

    public static string InterceptSelf(this IEnumerable<string> arr)
        => arr.Aggregate((s1, s2) => s1.Intersect(s2).Join());

    public static IEnumerable<char> InterceptSelf(this IEnumerable<IEnumerable<char>> arr)
        => arr.Aggregate((carr1, carr2) => carr1.Intersect(carr2));

    public static bool IsInRange(this Range r1, Range r2)
        => r1.Start.Value >= r2.Start.Value && r1.End.Value <= r2.End.Value;

    public static bool IsOverlapping(this Range r1, Range r2)
        => (r1.Start.Value <= r2.Start.Value && r1.End.Value >= r2.Start.Value)
           || (r1.Start.Value <= r2.End.Value && r1.End.Value >= r2.End.Value);

    public static IEnumerable<IEnumerable<T>> GetCombinations<T>(this IEnumerable<T> arr, int k) where T : IComparable
    {
        if (k == 1) return arr.Select(t => new[] { t });
        return arr.GetCombinations(k - 1).SelectMany(t => arr.Where(o => o.CompareTo(t.Last()) > 0),
            (t1, t2) => t1.Concat(new[] { t2 }));
    }

    public static T[] SuperSplit<T>(this string str, string split1, string split2, Func<string[], T> select)
        => str.Split(split1).Select(s => s.Split(split2)).Select(select).ToArray();

    public static string[][] SuperSplit(this string str, string split1, string split2)
        => str.Split(split1).Select(s => s.Split(split2).ToArray()).ToArray();

    public static string[][] SuperSplit(this string str, char split1, char split2)
        => str.SuperSplit($"{split1}", $"{split2}");

    public static string[][] SuperSplit(this string str, string split1, char split2)
        => str.SuperSplit(split1, $"{split2}");

    public static string[][] SuperSplit(this string str, char split1, string split2)
        => str.SuperSplit($"{split1}", split2);

    public static int SizeOfPlane<T>(this Dictionary<(int x, int y), T> map)
        => map.Keys.Max(kv => Math.Max(Math.Abs(kv.x), Math.Abs(kv.y)));

    public static (int x, int y) GetOffsets<T>(this Dictionary<(int x, int y), T> map)
        => (Math.Abs(map.Min(kv => kv.Key.x)), Math.Abs(map.Min(kv => kv.Key.y)));

    public static (int x, int y) GetSizes<T>(this Dictionary<(int x, int y), T> map)
        => (map.Max(kv => kv.Key.x), map.Max(kv => kv.Key.y));

    public static T[,] GenerateMap<T>(this Dictionary<(int x, int y), T> map)
    {
        var (xSize, ySize) = map.GetSizes();
        var (xOff, yOff) = map.GetOffsets();
        var matrix = new T[xSize + xOff + 1, ySize + yOff + 1];

        foreach (var ((x, y), v) in map)
        {
            matrix[x + xOff, y + yOff] = v;
        }

        return matrix;
    }

    public static string String<T>(this T[,] map, Func<T, string> toStr)
    {
        StringBuilder sb = new();
        for (var y = map.GetLength(1) - 1; y >= 0; y--)
        {
            for (var x = 0; x < map.GetLength(0); x++)
            {
                sb.Append(toStr(map[x, y]));
            }

            sb.Append('\n');
        }

        return sb.ToString();
    }

    public static string LoopReplace(this string str, params (string search, string replace)[] arr)
        => arr.Aggregate(str, (current, replacement) => current.Replace(replacement.search, replacement.replace));

    public static string CleanSpaces(this string str) => SpaceRegex.Replace(str, " ");

    public static long GCD(this long a, long b)
    {
        while (b != 0)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }

    public static long LCM(this long a, long b) => a / a.GCD(b) * b;

    public static List<T> Rever<T>(this IEnumerable<T> arr)
    {
        var list = arr.ToList();
        list.Reverse();
        return list;
    }

    public static TR Flatten<T, TR>(this IEnumerable<T> list, Func<IEnumerable<T>, TR> flatten) => flatten(list);
    public static TR Flatten<T, TR>(this List<T> list, Func<List<T>, TR> flatten) => flatten(list);
    public static TR Flatten<T, TR>(this T[] list, Func<T[], TR> flatten) => flatten(list);
    public static int ParseInt(this char c, int def = 0) => int.TryParse($"{c}", out var i) ? i : def;
    public static TO Inline<T, TO>(this T t, Func<T, TO> func) => func(t);

    public static long ManhattanDistance(this (long x, long y) xy, (long x, long y) xy2)
        => Math.Abs(xy2.x - xy.x) + Math.Abs(xy2.y - xy.y);

    public static IEnumerable<(T, T)> CombinationsUnique<T>(this IEnumerable<T> arr) => CombinationsUnique(arr, arr);

    public static IEnumerable<(T1, T2)> CombinationsUnique<T1, T2>(this IEnumerable<T1> arr1, IEnumerable<T2> arr2)
    {
        for (var i = 0; i < arr1.Count(); i++)
        {
            for (var j = i + 1; j < arr2.Count(); j++)
            {
                yield return (arr1.ElementAt(i), arr2.ElementAt(j));
            }
        }
    }

    public static int[] GenRange(this int start, int count) => Enumerable.Range(start, count).ToArray();
    public static string Repeat(this char c, int amount) => Enumerable.Repeat(c, amount).Join();
    public static string Repeat(this string str, int amount) => Enumerable.Repeat(str, amount).Join();
    public static string Repeat(this string str, int amount, char join) => Enumerable.Repeat(str, amount).Join(join);

    public static void AddOrReplace<T>(this List<T> list, T item, Predicate<T> indexOf)
    {
        var index = list.FindIndex(indexOf);
        if (index == -1)
        {
            list.Add(item);
            return;
        }

        list[index] = item;
    }
}