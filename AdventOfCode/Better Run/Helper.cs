using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Better_Run;

public static class Helper
{
    public static Regex NumberOnlyRegex = new(@"^\d+$", RegexOptions.Compiled);

    public static char ToChar(this int i, int offset = 97) => (char) (i + offset);
    public static bool IsInRange(this int i, int min, int max) => min <= i && max >= i;
    public static bool IsInRange(this long l, int min, int max) => min <= l && max >= l;
    public static int[] ToIntArr(this IEnumerable<string> texts) => texts.Select(int.Parse).ToArray();
    public static long[] ToLongArr(this IEnumerable<string> texts) => texts.Select(long.Parse).ToArray();
    public static string ReplaceWithSpace(this string text, string pattern) => text.Replace(pattern, " ");
    public static string Remove(this string text, string pattern) => text.Replace(pattern, "");
    public static string[] SplitSpace(this string text) => text.Split(" ", StringSplitOptions.RemoveEmptyEntries);
    public static bool IsAllNumbers(this string text) => NumberOnlyRegex.IsMatch(text);
    public static string Repeat(this string pattern, int i) => string.Join("", Enumerable.Repeat(pattern, i));
    public static string Rever(this string s) => string.Join("", s.Reverse().ToArray());
    public static int FindIndexOf<T>(this T[] arr, T find) => arr.ToList().FindIndex(t => t.Equals(find));
    public static int FindLastIndexOf<T>(this T[] arr, T find) => arr.ToList().FindLastIndex(t => t.Equals(find));
    public static T Multi<T>(this IEnumerable<T> arr) where T : INumber<T> => arr.Aggregate((l1, l2) => l1 * l2);
    public static IEnumerable<T> EvenIndexes<T>(this IEnumerable<T> arr) => arr.Where((_, i) => i % 2 == 0);
    public static IEnumerable<T> OddIndexes<T>(this IEnumerable<T> arr) => arr.Where((_, i) => i % 2 == 1);

    public static string[] Range(this GroupCollection gc, Range range)
    {
        var arr = new string[range.End.Value - (range.Start.Value - 1)];
        for (int i = range.Start.Value, j = 0; i < arr.Length + 1; i++, j++) arr[j] = gc[i].Value;
        return arr;
    }

    public static Dictionary<K, V> ToDictionary<K, V>(this IEnumerable<KeyValuePair<K, V>> pair)
    {
        return pair.ToDictionary(kv => kv.Key, kv => kv.Value);
    }

    public static void Each<T>(this IEnumerable<T> arr, Action<T> action)
    {
        foreach (var element in arr) action(element);
    }

    public static void Each<T>(this IEnumerable<T> arr, Action<T, int> action)
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
        return arr.Reverse().Select((n, i) => n + i).Distinct().Count() == 1;
    }

    public static int ToInt(this char c, int offset = 97) => (c - offset) % 27;

    public static string[] Split(this string text, string pattern)
    {
        return text.Split(pattern, StringSplitOptions.RemoveEmptyEntries);
    }

    public static T[] SubArr<T>(this IEnumerable<T> arr, int start, int end)
    {
        return arr.Skip(start).Take(end - start).ToArray();
    }

    public static int[] FindAllIndexesOf<T>(this IEnumerable<T> arr, T search)
    {
        return arr.Select((o, i) => Equals(search, o) ? i : -1).Where(i => i != -1).ToArray();
    }

    public static T[] Append<T>(this T[] arr, T[] appender)
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

    public static bool AnyContainsAny<T>(this IEnumerable<T> arr, IEnumerable<T> arr2) => arr2.Any(arr.Contains);

    public static T[] Copy<T>(this T[] t)
    {
        var n = new T[t.Length];
        for (var i = 0; i < t.Length; i++) n[i] = t[i];
        return n;
    }

    public static string ToS(this IEnumerable<char> carr) => string.Join("", carr);

    public static IEnumerable<T> Window<T>(this T[] arr, int grouping, Func<IEnumerable<T>, T> condense)
    {
        return arr.Skip(grouping - 1).Select((n, i) => condense.Invoke(arr[i..(i + grouping)]));
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
        return dict.ContainsKey(key) ? dict[key] : def;
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
}