using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Http;
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

    [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH",
        MessageId = "type: System.Int64[]")]
    public static void Main()
    {
        Uri address = new("https://adventofcode.com");
        CookieContainer cookieContainer = new();
        cookieContainer.Add(address, new Cookie("session", Token.AoCToken));

        client = new HttpClient(new HttpClientHandler { CookieContainer = cookieContainer })
        {
            BaseAddress = address
        };
        
        Console.CursorVisible = false;
        Starter.Start();
    }

    public static string SaveInput(int year, int day)
    {
        var input = client.GetStringAsync($"/{year}/day/{day}/input").GetAwaiter().GetResult();
        if (!Directory.Exists($"Input/{year}")) Directory.CreateDirectory($"Input/{year}");
        File.WriteAllText($"Input/{year}/{day}.txt", input);
        return input;
    }

    // public static async Task StartFetcher()
    // {
    //     Uri address = new("https://adventofcode.com");
    //     CookieContainer cookieContainer = new();
    //     cookieContainer.Add(address, new Cookie("Cookie", Token.AoCToken));
    //     HttpClient client = new(new HttpClientHandler{CookieContainer = cookieContainer});
    //     var request = await client.GetAsync("/2023/day/1/input");
    //     ClrCnsl.Write(await request.Content.ReadAsStringAsync());
    // }
}