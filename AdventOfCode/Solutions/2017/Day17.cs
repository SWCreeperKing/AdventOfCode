namespace AdventOfCode.Solutions._2017;

[Day(2017, 17, "Spinlock")]
file class Day17
{
    [ModifyInput] public static int ProcessInput(string input) => int.Parse(input);

    [Answer(1025)]
    public static long Part1(int inp)
    {
        List<int> spinlock = [0];
        inp++;
        var last = 0;
        var lastIndex = 0;
        for (int i = 1, j = inp % 1; i <= 2017; i++, j = (j + inp) % i)
        {
            spinlock.Insert(j + 1, i);
        }
        return spinlock[spinlock.FindIndexOf(2017) + 1];
    }

    [Answer(37803463)]
    public static long Part2(int inp)
    {
        inp++;
        var last = 0;
        for (int i = 1, j = inp % 1; i <= 50000000; i++, j = (j + inp) % i)
        {
            if (j + 1 != 1) continue;
            last = i;
        }
        
        return last;
    }
}