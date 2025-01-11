namespace AdventOfCode.Solutions._2024;

file class Day9() : Puzzle<string>(2024, 9, "Disk Fragmenter")
{
    public override string ProcessInput(string input) => input;

    [Answer(6242766523059)]
    public override object Part1(string inp)
    {
        var line = inp.SelectArr(c => c - '0');
        var drive = new int[line.Sum()];
        var id = 0;

        for (int i = 0, j = 0; i < drive.Length; id++)
        {
            var sec = line[j++];
            for (var k = 0; k < sec; k++)
            {
                drive[i + k] = id;
            }

            i += sec;
            if (j >= line.Length) continue;
            var space = line[j++];
            for (var k = 0; k < space; k++)
            {
                drive[i + k] = -1;
            }

            i += space;
        }

        var back = drive.Length - 1;

        for (var i = 0; i < back; i++)
        {
            if (drive[i] != -1) continue;
            drive[i] = drive[back];
            drive[back--] = -1;

            while (drive[back] == -1)
            {
                back--;
            }
        }

        long check = 0;

        for (var i = 0; i < drive.Length; i++)
        {
            if (drive[i] == -1) break;
            check += i * drive[i];
        }

        return check;
    }

    [Answer(6272188244509)]
    public override object Part2(string inp)
    {
        var line = inp.SelectArr(c => c - '0');
        var drive = new int[line.Sum()];
        var id = 0;
        List<(int id, int leng)> driveSeg = [];

        for (var i = 0; i < line.Length; id++)
        {
            var sec = line[i++];
            driveSeg.Add((id, sec));
            if (i >= line.Length) continue;
            var space = line[i++];
            driveSeg.Add((-1, space));
        }

        var lowestI = driveSeg.Count - 1;
        var changed = true;
        while (changed)
        {
            changed = false;
            for (var i = lowestI; i >= 0; i--)
            {
                var seg = driveSeg[i];
                if (seg.id == -1) continue;
                var matchI = driveSeg.FirstIndexWhere(t => t.Item1 == -1 && t.Item2 >= seg.leng);
                if (matchI >= i || matchI == -1) continue;
                var match = driveSeg[matchI];
                changed = true;
                if (seg.leng == match.leng)
                {
                    driveSeg.RemoveAt(matchI);
                    driveSeg.Insert(matchI, seg);
                    driveSeg.RemoveAt(i);
                    driveSeg.Insert(i, match);
                }
                else
                {
                    var split1 = (-1, seg.leng);
                    var split2 = (-1, match.leng - seg.leng);
                    driveSeg.RemoveAt(i);
                    driveSeg.Insert(i, split1);
                    driveSeg.RemoveAt(matchI);
                    driveSeg.Insert(matchI, seg);
                    driveSeg.Insert(matchI + 1, split2);
                }

                lowestI = Math.Min(lowestI, i);
                break;
            }
        }

        for (int i = 0, j = 0; j < driveSeg.Count && i < drive.Length; j++)
        {
            var seg = driveSeg[j];
            for (var k = 0; k < seg.leng; k++, i++)
            {
                drive[i] = seg.id;
            }
        }
        
        long check = 0;
        for (var i = 0; i < drive.Length; i++)
        {
            if (drive[i] == -1) continue;
            check += i * drive[i];
        }

        return check;
    }
}