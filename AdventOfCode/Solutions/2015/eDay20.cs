using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015;

public class eDay20 : Puzzle<long, long>
{
    public override (long part1, long part2) Result { get; } = (665280, 705600);
    public override (int year, int day) PuzzleSolution { get; } = (2015, 20);
    public override long ProcessInput(string input) => long.Parse(input);

    public override long Part1(long inp) => GetHouseFromPresents(inp, inp / 10);
    public override long Part2(long inp) => GetHouseFromPresents(inp, 50, 11);

    private static long GetHouseFromPresents(long presents, long maxHousesPerElf, int mul = 10)
    {
        var houses = new int[presents / 10];
        for (var elf = 1; elf < houses.Length; elf++)
        {
            int house = 0, visits = 0;
            while (house + elf < houses.Length && visits++ < maxHousesPerElf) houses[house += elf] += mul * elf;
        }

        return houses.FindIndexOf(houses.First(i => i >= presents));
    }
}