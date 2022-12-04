using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode;

public class Program
{
    // These are all solutions for
    // Advent of Code:
    // https://adventofcode.com/
    // my highest placing is 2022, day 1, part 1 at 493 place and a total time of 3:33

    [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH",
        MessageId = "type: System.Int64[]")]
    public static void Main()
    {
        Console.CursorVisible = false;
        Starter.Start();
    }
}