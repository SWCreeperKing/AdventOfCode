using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2016;

[Day(2016, 10, "Balance Bots")]
file class Day10
{
    private static readonly Regex ValueRegex = new(@"value (\d+?) goes to (.+)", RegexOptions.Compiled);
    private static readonly Regex GiveRegex = new("(.+) gives low to (.+) and high to (.+)", RegexOptions.Compiled);

    [ModifyInput]
    public static string[][][] ProcessInput(string inp)
    {
        var rawInp = inp.Split('\n');
        return
        [
            rawInp.Where(s => s.StartsWith("value")).Select(s => ValueRegex.Match(s).Range(1..2)).ToArray(),
            rawInp.Where(s => !s.StartsWith("value")).Select(s => GiveRegex.Match(s).Range(1..3)).ToArray()
        ];
    }

    [Answer(157)]
    public static long Part1(string[][][] inp)
    {
        int[] compareDesire = { 61, 17 };
        Dictionary<int, List<int>> outputs = new();
        Dictionary<string, Bot> bots = new();

        foreach (var botInstruction in inp[1])
        {
            var hander = GetBot(bots, botInstruction[0]);

            if (botInstruction[1].Contains("output"))
                hander.GiveLowerOutput = int.Parse(botInstruction[1].Split(' ')[^1]);
            else
                hander.GiveLowerBot = GetBot(bots, botInstruction[1]);

            if (botInstruction[2].Contains("output"))
                hander.GiveHigherOutput = int.Parse(botInstruction[2].Split(' ')[^1]);
            else
                hander.GiveHigherBot = GetBot(bots, botInstruction[2]);
        }

        var botList = bots.Values.ToList();
        foreach (var giveInstruction in inp[0])
        {
            var value = int.Parse(giveInstruction[0]);
            var bot = bots[giveInstruction[1]];
            bot.Inventory.Add(value);

            while (botList.Any(b => b.IsInventoryFull()))
            {
                var fullInvBot = botList.First(b => b.IsInventoryFull());
                var output = fullInvBot.GiveOutput(outputs, compareDesire);
                if (output != -1) return output;
            }
        }

        return -1;
    }

    [Answer(1085)]
    public static long Part2(string[][][] inp)
    {
        Dictionary<int, List<int>> outputs = new();
        Dictionary<string, Bot> bots = new();

        foreach (var botInstruction in inp[1])
        {
            var hander = GetBot(bots, botInstruction[0]);

            if (botInstruction[1].Contains("output"))
                hander.GiveLowerOutput = int.Parse(botInstruction[1].Split(' ')[^1]);
            else
                hander.GiveLowerBot = GetBot(bots, botInstruction[1]);

            if (botInstruction[2].Contains("output"))
                hander.GiveHigherOutput = int.Parse(botInstruction[2].Split(' ')[^1]);
            else
                hander.GiveHigherBot = GetBot(bots, botInstruction[2]);
        }

        var botList = bots.Values.ToList();
        foreach (var giveInstruction in inp[0])
        {
            var value = int.Parse(giveInstruction[0]);
            var bot = bots[giveInstruction[1]];
            bot.Inventory.Add(value);

            while (botList.Any(b => b.IsInventoryFull()))
            {
                var fullInvBot = botList.First(b => b.IsInventoryFull());
                fullInvBot.GiveOutput(outputs);
            }
        }

        return outputs[0][0] * outputs[1][0] * outputs[2][0];
    }

    private static Bot GetBot(IDictionary<string, Bot> bots, string botId)
    {
        if (!bots.TryGetValue(botId, out var bot)) bots[botId] = bot = new Bot(int.Parse(botId.Split(' ')[^1]));

        return bot;
    }
}

public class Bot(int id)
{
    public readonly int Id = id;
    public readonly List<int> Inventory = [];
    public Bot GiveHigherBot;
    public int GiveHigherOutput;
    public Bot GiveLowerBot;
    public int GiveLowerOutput;

    public bool IsInventoryFull() { return Inventory.Count >= 2; }

    public int GiveOutput(Dictionary<int, List<int>> outputs, params int[] getBotIdWith)
    {
        if (getBotIdWith.Length > 0 && getBotIdWith.All(v => Inventory.Contains(v))) return Id;
        // possible recursion is required if answer doesn't work
        var lower = Inventory.Min();
        var higher = Inventory.Max();

        if (GiveLowerBot is null)
            GiveOutputValue(outputs, GiveLowerOutput, lower);
        else
            GiveLowerBot.Inventory.Add(lower);

        if (GiveHigherBot is null)
            GiveOutputValue(outputs, GiveHigherOutput, higher);
        else
            GiveHigherBot.Inventory.Add(higher);

        Inventory.Clear();
        return -1;
    }

    private void GiveOutputValue(Dictionary<int, List<int>> outputs, int output, int value)
    {
        if (!outputs.TryGetValue(output, out var outputList)) outputs[output] = outputList = [];

        outputList.Add(value);
    }
}