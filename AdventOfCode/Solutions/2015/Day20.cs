namespace AdventOfCode.Solutions._2015;

file class Day20() : Puzzle<long>(2015, 20, "Infinite Elves and Infinite Houses")
{
    public override long ProcessInput(string input) { return long.Parse(input); }
    [Answer(665280)] public override object Part1(long inp) { return GetHouseFromPresents(inp, inp / 10); }
    [Answer(705600)] public override object Part2(long inp) { return GetHouseFromPresents(inp, 50, 11); }

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