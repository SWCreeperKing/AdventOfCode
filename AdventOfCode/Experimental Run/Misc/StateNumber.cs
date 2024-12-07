namespace AdventOfCode.Experimental_Run.Misc;

public class StateNumber(int length, int max)
{
    private int Max = max;
    private int[] Tracker = new int[length];

    public bool Increment()
    {
        Tracker[0]++;
        for (var i = 0; i < Tracker.Length; i++)
        {
            if (Tracker[i] < max) return true;
            if (i == Tracker.Length - 1) return false;
            Tracker[i] = 0;
            Tracker[i + 1]++;
        }

        return true;
    }

    public int this[int i] => Tracker[i];
}