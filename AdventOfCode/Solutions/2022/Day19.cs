namespace AdventOfCode.Solutions._2022;

file class Day19()
    : Puzzle<(int blueprint, int orebot, int claybot, int obibot1, int obibot2, int geobot1, int geobot2)[]>(2022, 19,
        "Not Enough Minerals")
{
    public static readonly Regex InputRegex =
        new(
            @"Blueprint (\d+): Each ore robot costs (\d+) ore. Each clay robot costs (\d+) ore. Each obsidian robot costs (\d+) ore and (\d+) clay. Each geode robot costs (\d+) ore and (\d+) obsidian.",
            RegexOptions.Compiled);

    public override (int blueprint, int orebot, int claybot, int obibot1, int obibot2, int geobot1, int geobot2)[]
        ProcessInput(string inp)
    {
        return inp.Split('\n')
                  .Select(s =>
                   {
                       var reg = InputRegex.Match(s).Groups.Range(1..7).ToIntArr();
                       return (blueprint: reg[0], orebot: reg[1], claybot: reg[2], obibot1: reg[3], obibot2: reg[4],
                           geobot1: reg[5],
                           geobot2: reg[6]);
                   })
                  .ToArray();
    }

    public override object Part1(
        (int blueprint, int orebot, int claybot, int obibot1, int obibot2, int geobot1, int geobot2)[] inp)
    {
        var maxCollect = 0;

        foreach (var (blueprint, orebot, claybot, obibot1, obibot2, geobot1, geobot2) in inp)
        {
            var resources = new[] { 0, 0, 0, 0 };
            var botbuilding = new[] { 0, 0, 0, 0 };
            var bots = new[] { 1, 0, 0, 0 };

            for (var i = 0; i < 24; i++)
            {
                while (geobot1 <= resources[0] && geobot2 <= resources[2])
                {
                    resources[0] -= geobot1;
                    resources[2] -= geobot2;
                    botbuilding[4]++;
                }

                // while (obibot1 <= )

                for (var bot = 0; bot < bots.Length; bot++)
                {
                    resources[bot] += bots[bot];
                    bots[bot] += botbuilding[bot];
                    botbuilding[bot] = 0;
                }
            }

            maxCollect = Math.Max(maxCollect, blueprint * resources[^1]);
        }

        return 0;
    }

    public override object Part2(
        (int blueprint, int orebot, int claybot, int obibot1, int obibot2, int geobot1, int geobot2)[] inp)
    {
        return 0;
    }
}

public record Material(int Ore, int Clay, int Obi, int Geo);

public record Bluerint(int Id, Bot Ore, Bot Clay, Bot Obi, Bot Geo);

public record Bot(Material Cost, Material Make);