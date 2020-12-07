using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class Day7p2
    {
        record Bag(string Shade, string Color, int Amount = -1)
        {
            public string Compare => $"{Shade} {Color}";
        }

        [Run(7, 2)]
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

            var catalyst =
                (from bBag in bags where bBag.Item1.Shade == "shiny" && bBag.Item1.Color == "gold" select bBag).First();

            long Counter(Bag bag, Bag[] bags, List<(Bag, Bag[])> bagz)
            {
                return bags.Sum(b =>
                    b.Amount + b.Amount * Counter(b, bagz.Find(bb => bb.Item1.Compare == b.Compare).Item2, bagz));
            }

            return Counter(catalyst.Item1, catalyst.Item2, bags);
        }
    }
}