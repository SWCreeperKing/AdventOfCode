#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using CreepyUtil;
using static CreepyUtil.Direction;
using Range = System.Range;

namespace AdventOfCode;

public static class Helper
{
    // public static Regex NumberOnlyRegex = new(@"^\d+$", RegexOptions.Compiled);
    public static Regex SpaceRegex = new(@"\s+", RegexOptions.Compiled);

    public static char ToChar(this int i, int offset = 97) { return (char)(i + offset); }

    public static bool IsInRange(this int i, int min, int max) { return min <= i && max >= i; }

    public static bool IsInRange(this long l, int min, int max) { return min <= l && max >= l; }

    public static bool IsInRange(this long l, long min, long max) { return min <= l && max >= l; }

    public static int[] ToIntArr(this IEnumerable<string> texts) { return texts.Select(int.Parse).ToArray(); }

    public static long[] ToLongArr(this IEnumerable<string> texts) { return texts.Select(long.Parse).ToArray(); }

    public static string ReplaceWithSpace(this string text, string pattern) { return text.Replace(pattern, " "); }

    public static string Remove(this string text, string pattern) { return text.Replace(pattern, string.Empty); }

    public static string[] SplitSpace(this string text)
    {
        return text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    }

    public static bool IsAllNumbers(this string text) { return text.All(c => c is >= '0' and <= '9'); }

    public static int FindIndexOf<T>(this IEnumerable<T> arr, T find)
    {
        return arr.ToList().FindIndex(t => t.Equals(find));
    }

    public static T Multi<T>(this IEnumerable<T> arr) where T : INumber<T>
    {
        return arr.Aggregate((l1, l2) => l1 * l2);
    }

    public static IEnumerable<T> EvenIndexes<T>(this IEnumerable<T> arr) { return arr.Where((_, i) => i % 2 == 0); }

    public static IEnumerable<T> OddIndexes<T>(this IEnumerable<T> arr) { return arr.Where((_, i) => i % 2 == 1); }

    public static int ToInt(this JsonNode node) { return node.GetValue<int>(); }

    public static int FindLastIndexOf<T>(this IEnumerable<T> arr, T find)
    {
        return arr.ToList().FindLastIndex(t => t.Equals(find));
    }

    public static string Remove(this string text, params string[] pattern)
    {
        return pattern.Aggregate(text, (s, p) => s.Remove(p));
    }

    public static string[] Range(this GroupCollection gc, Range range)
    {
        var arr = new string[range.End.Value - (range.Start.Value - 1)];
        for (int i = range.Start.Value, j = 0; j < arr.Length; i++, j++) arr[j] = gc[i].Value;
        return arr;
    }

    public static string[] Range(this Match match, Range range) { return match.Groups.Range(range); }

    public static Dictionary<TK, TV> ToDictionary<TK, TV>(this IEnumerable<KeyValuePair<TK, TV>> pair)
    {
        return pair.ToDictionary(kv => kv.Key, kv => kv.Value);
    }

    public static void ForEach<T>(this IEnumerable<T> arr, Action<T> action)
    {
        foreach (var element in arr) action(element);
    }

    public static void ForEach<T>(this IEnumerable<T> arr, Action<T, int> action)
    {
        for (var i = 0; i < arr.Count(); i++) action(arr.ElementAt(i), i);
    }

    public static T AddReturn<T>(this IList<T> arr, T val)
    {
        arr.Add(val);
        return val;
    }

    public static bool IsSequential(params int[] arr)
    {
        for (var i = 1; i < arr.Length; i++)
            if (arr[i - 1] + 1 != arr[i])
                return false;

        return true;
    }

    public static bool IsSequential(Span<int> arr)
    {
        for (var i = 1; i < arr.Length; i++)
            if (arr[i - 1] + 1 != arr[i])
                return false;

        return true;
    }

    public static T[] SubArr<T>(this IEnumerable<T> arr, int start, int end)
    {
        return arr.Skip(start).Take(end - start).ToArray();
    }

    public static int[] FindAllIndexesOf<T>(this IEnumerable<T> arr, T search)
    {
        return arr.Select((o, i) => Equals(search, o) ? i : -1).Where(i => i != -1).ToArray();
    }

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
        for (var i = 0; i < minLeng; i++) together[i] = condition.Invoke(core[i], subCore[i]);

        return together;
    }

    public static T[] Swap<T>(this IEnumerable<T> rawArr, int pos1, int pos2)
    {
        var arr = rawArr.ToArray();
        (arr[pos1], arr[pos2]) = (arr[pos2], arr[pos1]);
        return arr;
    }

    public static bool Contains<T>(this IEnumerable<T> arr, IEnumerable<T> arr2) { return arr2.Any(arr.Contains); }

    public static string Join<T>(this IEnumerable<T> carr, string join = "") { return string.Join(join, carr); }

    public static string Join<T>(this IEnumerable<T> carr, char join) { return string.Join(join, carr); }

    public static IEnumerable<T> Window<T>(this T[] arr, int grouping, Func<IEnumerable<T>, T> condense)
    {
        return arr.Skip(grouping - 1).Select((_, i) => condense.Invoke(arr[i..(i + grouping)]));
    }

    public static IEnumerable<IEnumerable<T>> Window<T>(this IEnumerable<T> arr, int grouping, bool before = false)
    {
        for (var i = 0; i < arr.Count(); i++)
        {
            if (!before && i < grouping) continue;
            yield return arr.Take(Math.Max(0, i - grouping)..i);
        }
    }

    public static int FirstIndexWhere<T>(this IEnumerable<T> arr, Func<T, bool> where)
    {
        return arr.Select((t, i) => (t, i)).First(ti => where(ti.t)).i;
    }

    public static int Unique<T>(this IEnumerable<T> arr) { return arr.Distinct().Count(); }

    public static bool IsAllUnique<T>(this IEnumerable<T> arr) { return arr.Distinct().Count() == arr.Count(); }

    public static bool DoAllMatch<T, TKey>(this IEnumerable<T> arr, Func<T, TKey> by)
    {
        var groups = arr.GroupBy(by);
        return groups.Count() == 1;
    }

    public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IEnumerable<T> list, int leng = -1)
    {
        return leng == 1
            ? list.Select(t => new[] { t })
            : GetPermutations(list, (leng == -1 ? list.Count() : leng) - 1)
               .SelectMany(t => list.Where(o => !t.Contains(o)), (t1, t2) => t1.Concat(new[] { t2 }));
    }

    public static T[][] GetPermutationsArr<T>(this IEnumerable<T> list, int leng = -1)
    {
        return leng == 1
            ? list.Select(t => new[] { t }).ToArray()
            : GetPermutationsArr(list, (leng == -1 ? list.Count() : leng) - 1)
             .SelectMany(t => list.Where(o => !t.Contains(o)), (t1, t2) => t1.Concat(new[] { t2 }).ToArray())
             .ToArray();
    }

    public static TValue GetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key,
        TValue def = default)
    {
        return dict.GetValueOrDefault(key, def);
    }

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

    public static string Time(this Stopwatch sw) { return sw.Elapsed.Time(); }

    public static string Time(this TimeSpan elapsed)
    {
        StringBuilder sb = new();
        if (elapsed.Hours > 0) sb.Append(elapsed.Hours).Append("hr ");
        if (elapsed.Minutes > 0) sb.Append(elapsed.Minutes).Append("min ");
        if (elapsed.Seconds > 0) sb.Append(elapsed.Seconds).Append("sec ");
        if (elapsed.Milliseconds > 0) sb.Append(elapsed.Milliseconds).Append("ms ");

        var ns = (elapsed.Nanoseconds + elapsed.Microseconds * 1000f) / 100 / 10f;
        if (ns > 0) sb.Append($"{ns:###.#}µs");
        return sb.ToString().TrimEnd();
    }

    public static string String<T>(this IEnumerable<T> arr) { return $"[{arr.Join(',')}]"; }

    public static string String<T, TK>(this IEnumerable<T> arr, Func<T, TK> select)
    {
        return $"[{arr.Select(select).Join(',')}]";
    }

    public static string InterceptSelf(this IEnumerable<string> arr)
    {
        return arr.Aggregate((s1, s2) => s1.Intersect(s2).Join());
    }

    public static IEnumerable<char> InterceptSelf(this IEnumerable<IEnumerable<char>> arr)
    {
        return arr.Aggregate((carr1, carr2) => carr1.Intersect(carr2));
    }

    public static bool IsInRange(this Range r1, Range r2)
    {
        return r1.Start.Value >= r2.Start.Value && r1.End.Value <= r2.End.Value;
    }

    public static bool IsOverlapping(this Range r1, Range r2)
    {
        return (r1.Start.Value <= r2.Start.Value && r1.End.Value >= r2.Start.Value)
               || (r1.Start.Value <= r2.End.Value && r1.End.Value >= r2.End.Value);
    }

    public static IEnumerable<IEnumerable<T>> GetCombinations<T>(this IEnumerable<T> arr, int k) where T : IComparable
    {
        if (k == 1) return arr.Select(t => new[] { t });
        return arr.GetCombinations(k - 1)
                  .SelectMany(t => arr.Where(o => o.CompareTo(t.Last()) > 0),
                       (t1, t2) => t1.Concat(new[] { t2 }));
    }

    public static T[] SuperSplit<T>(this string str, string split1, string split2, Func<string[], T> select)
    {
        return str.Split(split1).Select(s => s.Split(split2)).Select(select).ToArray();
    }

    public static string[][] SuperSplit(this string str, string split1, string split2)
    {
        return str.Split(split1).Select(s => s.Split(split2).ToArray()).ToArray();
    }

    public static string[][] SuperSplit(this string str, char split1, char split2)
    {
        return str.SuperSplit($"{split1}", $"{split2}");
    }

    public static string[][] SuperSplit(this string str, string split1, char split2)
    {
        return str.SuperSplit(split1, $"{split2}");
    }

    public static string[][] SuperSplit(this string str, char split1, string split2)
    {
        return str.SuperSplit($"{split1}", split2);
    }

    public static int SizeOfPlane<T>(this Dictionary<(int x, int y), T> map)
    {
        return map.Keys.Max(kv => Math.Max(Math.Abs(kv.x), Math.Abs(kv.y)));
    }

    public static (int x, int y) GetOffsets<T>(this Dictionary<(int x, int y), T> map)
    {
        return (Math.Abs(map.Min(kv => kv.Key.x)), Math.Abs(map.Min(kv => kv.Key.y)));
    }

    public static (int x, int y) GetSizes<T>(this Dictionary<(int x, int y), T> map)
    {
        return (map.Max(kv => kv.Key.x), map.Max(kv => kv.Key.y));
    }

    public static T[,] GenerateMap<T>(this Dictionary<(int x, int y), T> map)
    {
        var (xSize, ySize) = map.GetSizes();
        var (xOff, yOff) = map.GetOffsets();
        var matrix = new T[xSize + xOff + 1, ySize + yOff + 1];

        foreach (var ((x, y), v) in map) matrix[x + xOff, y + yOff] = v;

        return matrix;
    }

    public static string String<T>(this T[,] map, Func<T, string> toStr)
    {
        StringBuilder sb = new();
        for (var y = map.GetLength(1) - 1; y >= 0; y--)
        {
            for (var x = 0; x < map.GetLength(0); x++) sb.Append(toStr(map[x, y]));

            sb.Append('\n');
        }

        return sb.ToString();
    }

    public static string LoopReplace(this string str, params (string search, string replace)[] arr)
    {
        return arr.Aggregate(str, (current, replacement) => current.Replace(replacement.search, replacement.replace));
    }

    public static string CleanSpaces(this string str) { return SpaceRegex.Replace(str, " "); }

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

    public static long LCM(this long a, long b) { return a / a.GCD(b) * b; }

    public static List<T> Rever<T>(this IEnumerable<T> arr)
    {
        var list = arr.ToList();
        list.Reverse();
        return list;
    }

    public static TR Flatten<T, TR>(this IEnumerable<T> list, Func<IEnumerable<T>, TR> flatten)
    {
        return flatten(list);
    }

    public static TR Flatten<T, TR>(this List<T> list, Func<List<T>, TR> flatten) { return flatten(list); }

    public static TR Flatten<T, TR>(this T[] list, Func<T[], TR> flatten) { return flatten(list); }

    public static int ParseInt(this char c, int def = 0) { return int.TryParse($"{c}", out var i) ? i : def; }

    public static TO Inline<T, TO>(this T t, Func<T, TO> func) { return func(t); }

    public static void InlineNoReturn<T>(this T t, Action<T> func) { func(t); }

    public static T InlineAndReturnSelf<T>(this T t, Action<T> func)
    {
        func(t);
        return t;
    }

    public static long ManhattanDistance(this (long x, long y) xy, (long x, long y) xy2)
    {
        return Math.Abs(xy2.x - xy.x) + Math.Abs(xy2.y - xy.y);
    }

    public static IEnumerable<(T, T)> CombinationsUnique<T>(this IEnumerable<T> arr)
    {
        return CombinationsUnique(arr, arr);
    }

    public static IEnumerable<(T1, T2)> CombinationsUnique<T1, T2>(this IEnumerable<T1> arr1, IEnumerable<T2> arr2)
    {
        for (var i = 0; i < arr1.Count(); i++)
        for (var j = i + 1; j < arr2.Count(); j++)
            yield return (arr1.ElementAt(i), arr2.ElementAt(j));
    }

    public static int[] GenRange(this int start, int count) { return Enumerable.Range(start, count).ToArray(); }

    public static string Repeat(this char c, int amount) { return Enumerable.Repeat(c, amount).Join(); }

    public static string Repeat(this string str, int amount) { return Enumerable.Repeat(str, amount).Join(); }

    public static string Repeat(this string str, int amount, char join)
    {
        return Enumerable.Repeat(str, amount).Join(join);
    }

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

    //https://www.wikihow.com/Calculate-the-Area-of-a-Polygon
    //https://en.wikipedia.org/wiki/Shoelace_formula
    public static long Shoelace(this IEnumerable<(int amount, Direction dir)> list)
    {
        long x = 0, y = 0, area = 0, perimeter = 0;
        foreach (var (amount, dir) in list)
        {
            long lx = x, ly = y;
            if (dir is Up or Down)
                y += amount * (dir is Up ? -1 : 1);
            else
                x += amount * (dir is Left ? -1 : 1);

            perimeter += amount;
            area += lx * y - ly * x;
        }

        return Math.Abs(area / 2) + perimeter / 2 + 1;
    }

    public static object? SInvoke(this MethodInfo info, params object?[]? objs) { return info.Invoke(null, objs); }

    public static MethodInfo? FirstOrNull<T>(this IEnumerable<MethodInfo> list)
        where T : Attribute
    {
        return list.FirstOrDefault(m => m.GetCustomAttributes<T>().Any(), null);
    }

    public static MethodInfo? FirstOrNull(this IEnumerable<MethodInfo> list, string name)
    {
        return list.FirstOrDefault(m => m.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase), null);
    }

    public static T? Attribute<T>(this MethodInfo info) where T : Attribute { return info.GetCustomAttribute<T>(); }

    public static IEnumerable<T> Attributes<T>(this MethodInfo info) where T : Attribute
    {
        return info.GetCustomAttributes<T>();
    }

    public static TimeSpan Sum(this TimeSpan?[] times)
    {
        if (times.Length > 2) throw new ArgumentException("Only 2 times in the array are allowed");
        if (times[0] is null && times[1] is null) return TimeSpan.Zero;
        if (times[0] is null) return times[1]!.Value;
        if (times[1] is null) return times[0]!.Value;
        return times[0]!.Value + times[0]!.Value;
    }

    public static bool IsDigit(this char c) { return c is >= '0' and <= '9'; }

    public static void Rotate<T>(this T[] arr, int steps, bool isLeft)
    {
        var copy = arr.ToArray();
        var len = arr.Length;
        steps = Math.Abs(steps) % len;
        var step = isLeft ? steps : len - steps;
        for (var i = 0; i < len; i++) arr[i] = copy[(i + step) % len];
    }

    public static (T t, TKey tKey, TKey otherKey) SingleOut<T, TKey>(this IEnumerable<T> arr, Func<T, TKey> by)
    {
        var groups = arr.GroupBy(by);
        if (groups.Count() != 2) throw new ArgumentException("Input list has more than 1 unique element");
        var single = groups.First(g => g.Count() == 1);

        return (single.ElementAt(0), single.Key, groups.First(g => g.Count() != 1).Key);
    }

    public static string RemoveWhile(this string s, char what, int removeLength)
    {
        int i;
        while ((i = s.IndexOf(what)) != -1)
        {
            s = s.Remove(i, removeLength);
        }

        return s;
    }

    public static string RemoveWhile(this string s, char what, Func<string, int, int> whereEnd, bool includeEnd = true)
    {
        int i;
        while ((i = s.IndexOf(what)) != -1)
        {
            var end = whereEnd(s, i);
            s = s.Remove(i, end - i + (includeEnd ? 1 : 0));
        }

        return s;
    }

    public static IEnumerable<string> RemoveWhileIterator(this string s, char what, Func<string, int, int> whereEnd,
        bool includeEnd = true)
    {
        int i;
        while ((i = s.IndexOf(what)) != -1)
        {
            var end = whereEnd(s, i);
            yield return s.Substring(i, end - i + (includeEnd ? 1 : 0));
            s = s.Remove(i, end - i + (includeEnd ? 1 : 0));
        }
    }

    public static T[] ReverseWithWrapping<T>(this T[] arr, int start, int length)
    {
        if (length > arr.Length) throw new ArgumentException("Length is greater than input array length");
        if (start + length <= arr.Length)
        {
            return [..arr[..start], ..arr[start..(start + length)].Reverse(), ..arr[(start + length)..]];
        }

        var reverseArr = ((T[]) [..arr[start..arr.Length], ..arr[..((start + length) % arr.Length)]])
                        .Reverse()
                        .ToArray(); // reverse wrapped array

        return
        [
            ..reverseArr[(arr.Length - start)..], // beginning
            ..arr[((start + length) % arr.Length)..start], // unchanged section
            ..reverseArr[..(arr.Length - start)] // end
        ];
    }

    public static IEnumerable<(int i, List<T>)> Cycle<T>(this IEnumerable<T> arr, Action<List<T>> cycle)
    {
        List<T> list = [..arr];
        var i = 0;

        while (true)
        {
            cycle(list);
            yield return (++i, list);
        }
    }

    public static IEnumerable<(int i, List<T>)> GetRepeatInCycle<T>(this IEnumerable<T> arr, Action<List<T>> cycle)
    {
        HashSet<string> seen = [];

        foreach (var (i, item) in arr.Cycle(cycle))
        {
            if (seen.Add(item.String())) continue;
            yield return (i, item);
        }
    }

    public static (List<T[]>, List<T>, int i) GenerateCycleTillRepeat<T>(this IEnumerable<T> arr, Action<List<T>> cycle)
    {
        HashSet<string> seen = [];
        List<T[]> cycles = [];

        foreach (var (i, item) in arr.Cycle(cycle))
        {
            var str = item.String();
            if (!seen.Add(str)) return (cycles, item, seen.FindIndexOf(str));
            cycles.Add([..item]);
        }

        return (cycles, [], -1);
    }

    public static TR[] SelectArr<T, TR>(this IEnumerable<T> arr, Func<T, int, TR> selector)
    {
        return arr.Select(selector).ToArray();
    }
    
    public static TR[] SelectArr<T, TR>(this IEnumerable<T> arr, Func<T, TR> selector)
    {
        return arr.Select(selector).ToArray();
    }
}