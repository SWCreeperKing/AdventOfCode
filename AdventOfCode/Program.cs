﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AdventOfCode.Experimental_Run;
using static CreepyUtil.ClrCnsl.ClrCnsl;

namespace AdventOfCode;

public class Program
{
    // These are all solutions for
    // Advent of Code:
    // https://adventofcode.com/
    // my highest placing is 2022, day 1, part 1 at 493 place and a total time of 3:33
    public static HttpClient client;
    public static long LastDownload;

    [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH",
        MessageId = "type: System.Int64[]")]
    public static void Main()
    {
        InitHttpClient();
        Console.CursorVisible = false;
        Starter.Start();
    }

    public static void InitHttpClient()
    {
        Uri address = new("https://adventofcode.com");
        CookieContainer cookieContainer = new();
        cookieContainer.Add(address, new Cookie("session", Token.AoCToken));

        client = new HttpClient(new HttpClientHandler { CookieContainer = cookieContainer })
        {
            BaseAddress = address
        };
    }

    public static string SaveInput(YearDayInfo info)
    {
        Write($"[#yellow]Downloading Input for [{info}]... ");
        var time = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();

        if (time - LastDownload <= 3e4) // 30s
            Task.Delay((int)(3e4 - (time - LastDownload))).GetAwaiter().GetResult();

        Console.WriteLine();
        var input = client.GetStringAsync(info.Url).GetAwaiter().GetResult();
        if (!Directory.Exists($"Input/{info.Year}")) Directory.CreateDirectory($"Input/{info.Year}");

        File.WriteAllText(info.File, input);
        WriteLine("[#darkyellow][Done]");
        LastDownload = time;
        return input;
    }

    public static string[][] GetLeaderBoard(int year)
    {
        Write($"[#yellow]Downloading leaderboard for [{year}]... ");
        var time = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();

        if (time - LastDownload <= 3e4) // 30s
            Task.Delay((int)(3e4 - (time - LastDownload))).GetAwaiter().GetResult();

        var content = client.GetStringAsync($"/{year}/leaderboard/self")
                            .GetAwaiter()
                            .GetResult();
        var leaderboardRaw = content.Remove("\r").Split('\n');
        WriteLine("[#darkyellow][Done]");
        LastDownload = time;

        var leaderboardStartText = leaderboardRaw.First(s => s.Contains("Day "));
        var leaderboardIndex = leaderboardRaw.FindIndexOf(leaderboardStartText);
        var endingIndex = leaderboardRaw.FindIndexOf("</pre>");
        return leaderboardRaw[(leaderboardIndex + 1)..endingIndex]
              .Select(s => s.Trim().CleanSpaces().Replace(">", "\\>").Split(' '))
              .ToArray();
    }
}