namespace AdventOfCode.Solutions._2020;

file class Day16() : Puzzle<(string[] inp, string[] nearbyTickets)>(2020, 16, "Ticket Translation")
{
    public override (string[] inp, string[] nearbyTickets) ProcessInput(string input)
    {
        return input.Split("\n\n").Flatten(arr => (arr, arr[2].Split('\n')[1..]));
    }

    [Answer(29878)]
    public override object Part1((string[] inp, string[] nearbyTickets) inp)
    {
        var rules = inp.inp[0].Split('\n');
        List<Func<int, bool>> requirements = [];

        foreach (var r in rules)
        {
            var reg = Regex.Match(r, ".*: ([0-9]*)-([0-9]*) or ([0-9]*)-([0-9]*)").Groups;
            var (one, two, three, four) = (int.Parse(reg[1].Value), int.Parse(reg[2].Value),
                int.Parse(reg[3].Value), int.Parse(reg[4].Value));
            requirements.Add(i => (one <= i && i <= two) || (three <= i && i <= four));
        }

        return inp.nearbyTickets.Select(ticket => ticket.Split(",").Select(int.Parse).ToArray())
                  .Aggregate(0L,
                       (current1, split) => split.Where(i => !requirements.Any(f => f.Invoke(i)))
                                                 .Aggregate(current1, (current, i) => current + i));
    }

    [Answer(855438643439)]
    public override object Part2((string[] inp, string[] nearbyTickets) inp)
    {
        var myTicket = inp.inp[1].Split('\n')[^1].Split(",");
        Dictionary<string, Func<int, bool>> requirementsRaw = new();

        foreach (var r in inp.inp[0].Split('\n'))
        {
            var reg = Regex.Match(r, @"(.*): ([0-9]*)-([0-9]*) or ([0-9]*)-([0-9]*)").Groups;
            var (one, two, three, four) = (int.Parse(reg[2].Value), int.Parse(reg[3].Value),
                int.Parse(reg[4].Value), int.Parse(reg[5].Value));
            requirementsRaw.Add(reg[1].Value, i => (one <= i && i <= two) || (three <= i && i <= four));
        }

        var requirements = requirementsRaw.Values.ToList();
        inp = (inp.inp, inp.nearbyTickets
                           .Where(ticket => ticket.Split(",").All(s => requirements.Any(f => f.Invoke(int.Parse(s)))))
                           .ToArray());
        var restructure = new List<int>[myTicket.Length];

        foreach (var split in inp.nearbyTickets.Select(ticket => ticket.Split(",").Select(int.Parse).ToArray()))
            for (var i = 0; i < split.Length; i++)
                if (restructure[i] is null) restructure[i] = [split[i]];
                else restructure[i].Add(split[i]);

        var concrete = new int[requirements.Count];
        Array.Fill(concrete, -1);
        var candidates = new List<int>[requirements.Count];

        for (var i = 0; i < restructure.Length; i++)
            foreach (var satisfy in restructure[i]
                        .Select(data =>
                             requirements.FindAll(f => f.Invoke(data)).Select(f => requirements.IndexOf(f)).ToArray()))
            {
                candidates[i] ??= Enumerable.Range(0, requirements.Count).ToList();
                if (satisfy.Length >= 20) continue;
                candidates[i].RemoveAll(j => candidates[i].Except(satisfy).Contains(j));
            }

        while (concrete.Any(l => l == -1))
            for (var i = 0; i < candidates.Length; i++)
            {
                if (concrete[i] != -1 || candidates[i].Count > 1) continue;
                var found = concrete[i] = candidates[i].First();
                foreach (var t in candidates)
                    if (t.Count > 1)
                        t.Remove(found);
            }

        var keys = requirementsRaw.Keys.ToArray();
        return keys.Where(s => s.Contains("departure"))
                   .Aggregate(1L,
                        (current, f)
                            => current * long.Parse(myTicket[concrete.ToList().IndexOf(keys.ToList().IndexOf(f))]));
    }
}