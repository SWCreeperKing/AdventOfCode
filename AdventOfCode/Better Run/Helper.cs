using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace AdventOfCode.Better_Run
{
    public static class Helper
    {
        public static bool IsInRange(this int i, int min, int max) => min <= i && max >= i;
        public static bool IsInRange(this long l, int min, int max) => min <= l && max >= l;
        public static int[] ToIntArr(this IEnumerable<string> texts) => texts.Select(int.Parse).ToArray();
        public static long[] ToLongArr(this IEnumerable<string> texts) => texts.Select(long.Parse).ToArray();
        public static string ReplaceWithSpace(this string text, string pattern) => text.Replace(pattern, " ");
        public static string Remove(this string text, string pattern) => text.Replace(pattern, "");
        public static string[] SplitSpace(this string text) => text.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        public static bool IsAllNumbers(this string text) => text.All(c => Regex.Match($"{c}", @"\d").Success);
        public static string Repeat(this string pattern, int i) => string.Join("", Enumerable.Repeat(pattern, i));
        public static string Rever(this string s) => string.Join("", s.Reverse().ToArray());
        public static bool IsSequential(this int[] arr) => arr.Zip(arr.Skip(1), (a, b) => (a + 1) == b).All(x => x);
        public static int FindIndexOf<T>(this T[] arr, T find) => arr.ToList().FindIndex(t => t.Equals(find));
        public static int FindLastIndexOf<T>(this T[] arr, T find) => arr.ToList().FindLastIndex(t => t.Equals(find));

        public static string[] Split(this string text, string pattern) =>
            text.Split(pattern, StringSplitOptions.RemoveEmptyEntries);

        public static T[] SubArr<T>(this IEnumerable<T> arr, int start, int end) =>
            arr.Skip(start).Take(end - start).ToArray();

        public static int[] FindAllIndexesOf<T>(this IEnumerable<T> arr, T search) =>
            arr.Select((o, i) => object.Equals(search, o) ? i : -1).Where(i => i != -1).ToArray();

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

        public static IEnumerable<T> Window<T>(this T[] arr, int grouping, Func<IEnumerable<T>, T> condense) =>
            arr.Skip(grouping - 1).Select((n, i) => condense.Invoke(arr[i..(i + grouping)]));
    }
}