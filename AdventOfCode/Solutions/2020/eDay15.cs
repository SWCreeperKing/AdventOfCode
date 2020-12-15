using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2020
{
    public class eDay15
    {
        [Run(2020, 15, 1, 387)]
        public static int Part1(string input)
        {
            List<int> history = new(); // number chain
            Dictionary<int, List<int>> past = new(); // history of turns for when a number was called
            void AddToPast(int h, int indx) // method for adding an index (or turn) to the history of turns
            {
                if (!past.ContainsKey(h)) past.Add(h, new(){indx});
                else past[h].Add(indx);
            }
            
            history.AddRange(input.Split(",").Select(int.Parse)); // add input into number chain
            for (var i = 0; i < history.Count; i++) AddToPast(history[i], i); // add number chain to turn history
            
            for (var i = history.Count; i < 2020; i++) // turn iteration
            {
                var prevNumb = history[i - 1]; // get last number
                if (!past.ContainsKey(prevNumb) || past[prevNumb].Count < 2) history.Add(0); // if last number is new or only appeared once this turn is 0
                else if (past[prevNumb].Count > 1) // or if the last number appears twice or more times
                {
                    var allIndexesOf = past[prevNumb]; // get all turns where last number appeared
                    var leng = allIndexesOf.Count; // count how many turns it appeared in 
                    history.Add(allIndexesOf[leng - 1] - allIndexesOf[leng - 2]); // add (2nd from last turn - last turn) to the number chain
                }
                
                AddToPast(history[i], i); // add to turn history
            }

            return history[2020 - 1]; // get the 2020th number (goal)
        }

        [Run(2020, 15, 2,6428)]
        public static int Part2(string input)
        {
            List<int> history = new();
            Dictionary<int, List<int>> past = new();
            void AddToPast(int h, int indx)
            {
                if (!past.ContainsKey(h)) past.Add(h, new(){indx});
                else past[h].Add(indx);
            }
            
            history.AddRange(input.Split(",").Select(int.Parse));
            for (var i = 0; i < history.Count; i++) AddToPast(history[i], i);
            
            for (var i = history.Count; i < 30000000; i++)
            {
                var prevNumb = history[i - 1];
                if (!past.ContainsKey(prevNumb) || past[prevNumb].Count < 2) history.Add(0);
                else if (past[prevNumb].Count > 1)
                {
                    var allIndexesOf = past[prevNumb];
                    var leng = allIndexesOf.Count;
                    history.Add(allIndexesOf[leng - 1] - allIndexesOf[leng - 2]);
                }
                
                AddToPast(history[i], i);
            }

            return history[30000000 - 1];
        }
    }
}