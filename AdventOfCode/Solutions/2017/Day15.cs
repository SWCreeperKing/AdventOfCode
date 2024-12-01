using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2017;

[Day(2017, 15, "Dueling Generators")]
file class Day15
{
    private const long Lowest = 65536;
    
    [ModifyInput]
    public static (int a, int b) ProcessInput(string input)
    {
        var lines = input.Split('\n');
        return (int.Parse(lines[0].Split(' ')[^1]), int.Parse(lines[1].Split(' ')[^1]));
    }

    [Answer(573)]
    public static long Part1((int a, int b) inp)
    {
        var count = 0;

        long lastA = inp.a;
        long lastB = inp.b;
        for (var i = 0; i < 40e6; i++)
        {
            lastA = lastA * 16807L % 2147483647;
            lastB = lastB * 48271L % 2147483647;
            if (lastA % Lowest != lastB % Lowest) continue;
            count++;
        }
        
        return count;
    }

    [Answer(294)]
    public static long Part2((int a, int b) inp)
    {
        var count = 0;

        long lastA = inp.a;
        long lastB = inp.b;
        for (var i = 0; i < 5e6; i++)
        {
            do
            {
                lastA = lastA * 16807L % 2147483647;
            } while (lastA % 4 != 0);
            do
            {
                lastB = lastB * 48271L % 2147483647;
            } while (lastB % 8 != 0);

            
            if (lastA % Lowest != lastB % Lowest) continue;
            count++;
        }
        
        return count;
    }
}