using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class Day7p1
    {
        record Bag(string Shade, string Color, int Amount = -1);

        [Run(7, 1)]
        public static long Main(string input)
        {
            var lines = input.Remove(".").Remove(" bags").Remove(" bag").Split("\n");
            var c = 0L;
            List<(Bag, Bag[])> bags = new();

            foreach (var l in lines)
            {
                var containSplit = l.Split(" contain ");
                var keyBag = containSplit[0].SplitSpace();
                var bb = new Bag(keyBag[0], keyBag[1]);
                if (!containSplit[1].Contains("no other"))
                {
                    var bagz = from b in containSplit[1].Split(", ")
                        let bS = b.SplitSpace()
                        select new Bag(bS[1], bS[2], int.TryParse(bS[0], out var i) ? i : 1);
                    bags.Add((bb, bagz.ToArray()));
                }
                else bags.Add((bb, new Bag[0]));
            }

            var finder =
                (from bBag in bags where bBag.Item2.Any(b => b.Shade == "shiny" && b.Color == "gold") select bBag.Item1)
                .ToList();

            List<Bag> hasGold = new();
            
            while (finder.Count > 0)
            {
                var first = finder.First();
                var bag = bags.FindAll(b => b.Item2.Any(bb => first.Shade == bb.Shade && first.Color == bb.Color));
                finder.Remove(first);
                hasGold.Add(first);
                bag.ForEach(b => finder.Add(b.Item1));
            }

            return hasGold.Union(hasGold).Count();
        }
    }
}