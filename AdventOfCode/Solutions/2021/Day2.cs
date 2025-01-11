namespace AdventOfCode.Solutions._2021;

file class Day2() : Puzzle<(char c, int i)[]>(2021, 2, "Dive!")
{
    public override (char c, int i)[] ProcessInput(string input)
    {
        return input.Split('\n').Select(s => (c: s[0], i: int.Parse(s.Split(' ')[^1]))).ToArray();
    }

    [Answer(2070300)]
    public override object Part1((char c, int i)[] inp)
    {
        return inp.Aggregate((h: 0, d: 0), (a, n) => (a.h + (n.c is 'f' ? n.i : 0), a.d + (n.c is not 'f'
            ? n.c is 'u'
                ? -n.i
                : n.i
            : 0)), res => res.h * res.d);
    }

    [Answer(2078985210)]
    public override object Part2((char c, int i)[] inp)
    {
        return inp.Aggregate((h: 0, a: 0, d: 0), (a, n) => (a.h + (n.c is 'f' ? n.i : 0), a.a + (n.c is not 'f'
            ? n.c is 'u'
                ? -n.i
                : n.i
            : 0), a.d + (n.c is 'f' ? a.a * n.i : 0)), res => res.h * res.d);
    }
}