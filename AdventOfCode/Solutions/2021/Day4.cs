using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2021
{
    public class Day4
    {
        public class Card
        {
            public bool[,] marked = new bool[5, 5];
            public Dictionary<int, (int x, int y)> board = new();
            public int lastNumber;

            public Card(string board)
            {
                var lines = board.Split('\n');
                for (var i = 0; i < lines.Length; i++)
                {
                    var split = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    for (var j = 0; j < split.Length; j++) this.board[int.Parse(split[j])] = (j, i);
                }
            }

            public void Mark(int i)
            {
                if (!board.ContainsKey(i)) return;
                var (x, y) = board[lastNumber = i];
                marked[x, y] = true;
            }

            public bool Check()
            {
                for (var i = 0; i < 5; i++)
                    if (marked[i, 0] && marked[i, 1] && marked[i, 2] && marked[i, 3] && marked[i, 4] ||
                        marked[0, i] && marked[1, i] && marked[2, i] && marked[3, i] && marked[4, i])
                        return true;
                return false;
            }

            public int Calc() => board.Sum(kv => marked[kv.Value.x, kv.Value.y] ? 0 : kv.Key) * lastNumber;
        }

        [Run(2021, 4, 1, 35711)]
        public static int Part1(string input)
        {
            var arr = input.Split("\n\n");
            var cards = arr.Skip(1).Select(s => new Card(s)).ToArray();
            foreach (var num in arr[0].Split(',').Select(int.Parse))
            {
                foreach (var t in cards) t.Mark(num);
                if (cards.Any(c => c.Check())) break;
            }

            return cards.First(c => c.Check()).Calc();
        }

        [Run(2021, 4, 2, 5586)]
        public static int Part2(string input)
        {
            var arr = input.Split("\n\n");
            var cards = arr.Skip(1).Select(s => new Card(s)).ToArray();
            foreach (var num in arr[0].Split(',').Select(int.Parse))
            {
                foreach (var t in cards) t.Mark(num);
                if (cards.Length != 1) cards = cards.Where(c => !c.Check()).ToArray();
                if (cards.Length == 1 && cards[0].Check()) break;
            }

            return cards[0].Calc();
        }
    }
}