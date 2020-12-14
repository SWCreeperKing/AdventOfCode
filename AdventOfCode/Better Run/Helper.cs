﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

        public static string[] Split(this string text, string pattern) =>
            text.Split(pattern, StringSplitOptions.RemoveEmptyEntries);

        public static T[] SubArr<T>(this IEnumerable<T> arr, int start, int end) =>
            arr.Skip(start).Take(end - start).ToArray();
        
    }
}