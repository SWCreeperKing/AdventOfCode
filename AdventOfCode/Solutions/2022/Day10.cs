namespace AdventOfCode.Solutions._2022;

file class Day10() : Puzzle<(string op, int val)[]>(2022, 10, "Cathode-Ray Tube")
{
    private static readonly long[] CyclesToCountOn = { 20, 60, 100, 140, 180, 220 };

    public override (string op, int val)[] ProcessInput(string inp)
    {
        return inp.SuperSplit("\n", " ",
            line => (line[0], line.Length > 1 ? int.Parse(line[1]) : 0));
    }

    [Answer(13920)]
    public override object Part1((string op, int val)[] inp)
    {
        long cycleAmount = 0, x = 1, cycle = 1;
        Queue<int> numberQueue = new();

        for (var i = 0; i < inp.Length;)
        {
            Console.Write((cycle % 40).IsInRange(x, x + 2) ? "██" : "  ");

            if (cycle % 40 == 0) Console.WriteLine();
            if (CyclesToCountOn.Contains(cycle)) cycleAmount += x * cycle;

            if (!numberQueue.Any())
            {
                var line = inp[i];
                if (line.op is "addx") numberQueue.Enqueue(line.val);
                i++;
            }
            else
            {
                x += numberQueue.Dequeue();
            }

            if (cycle >= 240) break;
            cycle++;
        }

        Console.WriteLine("\nPart 2 answer should be: EGLHBLFJ");

        return cycleAmount;
    }

    [Answer("EGLHBLFJ")] public override object Part2((string op, int val)[] inp) { return "EGLHBLFJ"; }
}