﻿using System;
using System.Collections.Generic;
using System.Linq;

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
        public static string[] SplitSpace(this string text) => text.Split(" ");

        public static string[] Split(this string text, string pattern) =>
            text.Split(pattern, StringSplitOptions.RemoveEmptyEntries);
    }
}