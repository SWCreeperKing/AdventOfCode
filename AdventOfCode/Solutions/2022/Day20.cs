namespace AdventOfCode.Solutions._2022;

[Day(2022, 20, "Grove Positioning System")]
file class Day20
{
    [ModifyInput] public static int[] ProcessInput(string inp) { return inp.Split('\n').ToIntArr(); }

    public static long Part1(int[] inp)
    {
        var arr = inp.ToList();
        var moved = new bool[inp.Length].ToList();

        while (moved.Any(b => !b))
        {
            var indexMove = moved.IndexOf(false);
            var holder = arr[indexMove];
            arr.RemoveAt(indexMove);
            moved.RemoveAt(indexMove);

            var newIndex = indexMove + holder;
            if (holder < 0) newIndex = indexMove + (holder % inp.Length - 1) + (inp.Length - 1);
            newIndex %= inp.Length;

            arr.Insert(newIndex, holder);
            moved.Insert(newIndex, true);
        }

        var offset = arr.IndexOf(0);
        var sum = 0;
        for (var i = 1; i <= 3000; i++)
        {
            if (i % 1000 != 0) continue;
            var j = (i + offset) % inp.Length;
            sum += arr[j];
        }

        return sum;
    }

    public static long Part2(int[] inp) { return 0; }
}