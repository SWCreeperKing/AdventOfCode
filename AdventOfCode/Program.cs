using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;
using AdventOfCode.Experimental_Run;

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

    public static string SaveInput(int year, int day)
    {
        ClrCnsl.Write($"[#yellow]Downloading Input for [{year}, {day}]... ");
        var time = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();

        if (time - LastDownload <= 3e4) // 30s
        {
            Task.Delay((int) (3e4 - (time - LastDownload))).GetAwaiter().GetResult();
        }

        var input = client.GetStringAsync($"/{year}/day/{day}/input")
            .GetAwaiter().GetResult();
        if (!Directory.Exists($"Input/{year}")) Directory.CreateDirectory($"Input/{year}");
        File.WriteAllText($"Input/{year}/{day}.txt", input);
        ClrCnsl.WriteLine("[#darkyellow][Done]");
        LastDownload = time;
        return input;
    }
}