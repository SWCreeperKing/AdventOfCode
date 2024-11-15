using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2021;

[Day(2021, 4, "Giant Squid")]
file class Day4
{
    [ModifyInput]
    public static (string[] arr, Card[] cards) ProcessInput(string input)
    {
        var arr = input.Split("\n\n");
        return (arr, arr.Skip(1).Select(s => new Card(s)).ToArray());
    }

    [Answer(35711)]
    public static int Part1((string[] arr, Card[] cards) inp)
    {
        foreach (var num in inp.arr[0].Split(',').Select(int.Parse))
        {
            foreach (var t in inp.cards) t.Mark(num);
            if (inp.cards.Any(c => c.Check())) break;
        }

        return inp.cards.First(c => c.Check()).Calc();
    }

    [Answer(5586)]
    public static int Part2((string[] arr, Card[] cards) inp)
    {
        foreach (var num in inp.arr[0].Split(',').Select(int.Parse))
        {
            foreach (var t in inp.cards) t.Mark(num);
            if (inp.cards.Length != 1) inp.cards = inp.cards.Where(c => !c.Check()).ToArray();
            if (inp.cards.Length == 1 && inp.cards[0].Check()) break;
        }

        return inp.cards[0].Calc();
    }
}

file class Card
{
    public readonly Dictionary<int, (int x, int y)> Board = new();
    public readonly bool[,] Marked = new bool[5, 5];
    public int LastNumber;

    public Card(string board)
    {
        var lines = board.Split('\n');
        for (var i = 0; i < lines.Length; i++)
        {
            var split = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (var j = 0; j < split.Length; j++) Board[int.Parse(split[j])] = (j, i);
        }
    }

    public void Mark(int i)
    {
        if (!Board.ContainsKey(i)) return;
        var (x, y) = Board[LastNumber = i];
        Marked[x, y] = true;
    }

    public bool Check()
    {
        for (var i = 0; i < 5; i++)
            if ((Marked[i, 0] && Marked[i, 1] && Marked[i, 2] && Marked[i, 3] && Marked[i, 4]) ||
                (Marked[0, i] && Marked[1, i] && Marked[2, i] && Marked[3, i] && Marked[4, i]))
                return true;
        return false;
    }

    public int Calc()
    {
        return Board.Sum(kv => Marked[kv.Value.x, kv.Value.y] ? 0 : kv.Key) * LastNumber;
    }
}