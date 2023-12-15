using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 14, "Parabolic Reflector Dish")]
public class Day14
{
    [ModifyInput] public static string[] ProcessInput(string input) => input.Split('\n');

    [Answer(102497)]
    public static long Part1(string[] inp)
    {
        List<List<char>> slides = [];
        for (var i = 0; i < inp[0].Length; i++)
        {
            slides.Add([]);
        }

        foreach (var line in inp)
        {
            for (var i = 0; i < line.Length; i++)
            {
                slides[i].Add(line[i]);
            }
        }

        var load = 0;
        foreach (var slide in slides)
        {
            var empty = -1;
            for (var i = 0; i < slides.Count; i++)
            {
                switch (slide[i])
                {
                    case '.':
                        if (empty == -1)
                        {
                            empty = i;
                        }

                        continue;
                    case '#':
                        empty = i + 1;
                        continue;
                    case 'O':
                        if (empty == -1) continue;
                        (slide[i], slide[empty]) = (slide[empty], slide[i]);
                        i = empty;
                        empty = -1;
                        break;
                }
            }
        }

        foreach (var slide in slides)
        {
            for (var i = 0; i < slides.Count; i++)
            {
                if (slide[i] is not 'O') continue;
                load += slide.Count - i;
            }
        }

        return load;
    }

    [Answer(105008)]
    public static long Part2(string[] inp)
    {
        List<List<char>> slides = [];
        for (var i = 0; i < inp[0].Length; i++)
        {
            slides.Add([]);
        }

        foreach (var line in inp)
        {
            for (var i = 0; i < line.Length; i++)
            {
                slides[i].Add(line[i]);
            }
        }

        var load = 0;
        var selected = "";
        List<string> cache = new();
        List<int> cacheNumbers = new();
        var hold = 0;
        for (var k = 0; k < 1000000000; k++)
        {
            var cycle = Cycle(slides);
            if (cache.Contains(cycle))
            {
                hold = cache.IndexOf(cycle);
                break;
            }

            var l = 0;
            foreach (var slide in slides)
            {
                for (var i = 0; i < slides.Count; i++)
                {
                    if (slide[i] is not 'O') continue;
                    l += slide.Count - i;
                }
            }

            cache.Add(cycle);
            cacheNumbers.Add(l);
        }

        var str = cache[hold + (1000000000 - hold) % (cache.Count - hold) - 1];

        foreach (var slide in str.Split('\n').Select(s => s.ToCharArray().ToList()).ToList())
        {
            for (var i = 0; i < slides.Count; i++)
            {
                if (slide[i] is not 'O') continue;
                load += slide.Count - i;
            }
        }

        return load;
    }

    public static string Stringify(List<List<char>> chars)
    {
        List<List<char>> slides = [];
        for (var i = 0; i < chars[0].Count; i++)
        {
            slides.Add([]);
        }

        foreach (var line in chars)
        {
            for (var i = 0; i < line.Count; i++)
            {
                slides[i].Add(line[i]);
            }
        }

        return slides.Select(s => s.Join()).Join('\n');
    }

    public static string Cycle(List<List<char>> slides)
    {
        // north
        foreach (var slide in slides)
        {
            var empty = -1;
            for (var i = 0; i < slides.Count; i++)
            {
                switch (slide[i])
                {
                    case '.':
                        if (empty == -1)
                        {
                            empty = i;
                        }

                        continue;
                    case '#':
                        empty = i + 1;
                        continue;
                    case 'O':
                        if (empty == -1) continue;
                        (slide[i], slide[empty]) = (slide[empty], slide[i]);
                        i = empty;
                        empty = -1;
                        break;
                }
            }
        }

        // west
        for (var j = 0; j < slides.Count; j++)
        {
            var empty = -1;
            for (var i = 0; i < slides[0].Count; i++)
            {
                switch (slides[i][j])
                {
                    case '.':
                        if (empty == -1)
                        {
                            empty = i;
                        }

                        continue;
                    case '#':
                        empty = i + 1;
                        continue;
                    case 'O':
                        if (empty == -1) continue;
                        (slides[i][j], slides[empty][j]) = (slides[empty][j], slides[i][j]);
                        i = empty;
                        empty = -1;
                        break;
                }
            }
        }

        // south
        foreach (var slide in slides)
        {
            var empty = -1;
            for (var i = slides.Count - 1; i >= 0; i--)
            {
                switch (slide[i])
                {
                    case '.':
                        if (empty == -1)
                        {
                            empty = i;
                        }

                        continue;
                    case '#':
                        empty = i - 1;
                        continue;
                    case 'O':
                        if (empty == -1) continue;
                        (slide[i], slide[empty]) = (slide[empty], slide[i]);
                        i = empty;
                        empty = -1;
                        break;
                }
            }
        }

        // east
        for (var j = 0; j < slides.Count; j++)
        {
            var empty = -1;
            for (var i = slides[0].Count - 1; i >= 0; i--)
            {
                switch (slides[i][j])
                {
                    case '.':
                        if (empty == -1)
                        {
                            empty = i;
                        }

                        continue;
                    case '#':
                        empty = i - 1;
                        continue;
                    case 'O':
                        if (empty == -1) continue;
                        (slides[i][j], slides[empty][j]) = (slides[empty][j], slides[i][j]);
                        i = empty;
                        empty = -1;
                        break;
                }
            }
        }

        return slides.Select(s => s.Join()).Join('\n');
    }
}