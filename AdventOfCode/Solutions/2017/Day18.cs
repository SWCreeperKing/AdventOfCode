using AdventOfCode.Experimental_Run.Misc;

namespace AdventOfCode.Solutions._2017;

[Day(2017, 18, "Duet")]
file class Day18
{
    [ModifyInput] public static string[][] ProcessInput(string input) => input.SuperSplit('\n', ' ');

    [Answer(7071)] public static long Part1(string[][] inp) { return new DuetProgram(inp).Run()[^1]; }

    [Copy]
    [Answer(127, AnswerState.Low)]
    // [Test("snd 1\nsnd 2\nsnd p\nrcv a\nrcv b\nrcv c\nrcv d")]
    public static long Part2(string[][] inp)
    {
        var sent = 0;
        var deadLock1 = 0;
        var deadLock2 = 0;

        DuetProgram p1 = new(inp);
        DuetProgram p2 = new(inp, 1);

        while (true)
        {
            var run1 = p1.Run();
            sent += run1.Count;
            if (run1.Count == 0)
            {
                deadLock1++;
            }
            else
            {
                foreach (var l in run1)
                {
                    p2.Queue.Enqueue(l);
                }
                deadLock1 = 0;
            }
            
            var run2 = p2.Run();
            if (run2.Count == 0)
            {
                deadLock2++;
            }
            else
            {
                foreach (var l in run2)
                {
                    p1.Queue.Enqueue(l);
                }
                deadLock2 = 0;
            }

            WriteLine($"{run1.String()} | {run2.String()}");
            if (deadLock1 >= 4 && deadLock2 >= 4) return sent;
        }
    }
}

public class DuetProgram
{
    public string[][] Instructions;
    public Queue<long> Queue = [];

    private Dictionary<string, long> Registers;
    private int ProgramCounter;
    private bool IsPart1;

    public DuetProgram(string[][] inp, int p = 0, bool part1 = true)
    {
        IsPart1 = part1;
        Registers = inp.Where(arr => !int.TryParse(arr[1], out _))
                       .DistinctBy(arr => arr[1])
                       .ToDictionary(arr => arr[1], _ => 0L);

        Instructions = inp;
        Registers["p"] = p;
    }

    public List<long> Run()
    {
        List<long> sounds = [];
        for (; ProgramCounter < Instructions.Length; ProgramCounter++)
        {
            switch (Instructions[ProgramCounter])
            {
                case ["snd", var reg]:
                    sounds.Add(Decode(reg));
                    break;
                case ["set", var reg, var val]:
                    Registers[reg] = Decode(val);
                    break;
                case ["add", var reg, var val]:
                    Registers[reg] += Decode(val);
                    break;
                case ["mul", var reg, var val]:
                    Registers[reg] *= Decode(val);
                    break;
                case ["mod", var reg, var val]:
                    Registers[reg] %= Decode(val);
                    break;
                case ["rcv", var val]:
                    if (IsPart1)
                    {
                        if (Decode(val) != 0) return sounds;
                        break;
                    }

                    if (Queue.Count == 0) return sounds;
                    Registers[val] = Queue.Dequeue();
                    break;
                case ["jgz", var reg, var y]:
                    if (Registers[reg] <= 0) continue;
                    ProgramCounter += (int)(Decode(y) - 1);
                    break;
            }
        }

        return sounds;
    }

    private long Decode(string s) { return int.TryParse(s, out var i) ? i : Registers[s]; }
}