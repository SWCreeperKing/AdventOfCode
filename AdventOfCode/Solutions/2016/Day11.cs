using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2016;

// i admit defeat :p
// https://github.com/encse/adventofcode/blob/master/2016/Day11/Solution.cs

[Day(2016, 11, "WIP")]
public class Day11
{
    public static readonly Regex ParseChipsReg = new(@"(\w+)-compatible", RegexOptions.Compiled);
    public static readonly Regex ParseGensReg = new(@"(\w+) generator", RegexOptions.Compiled);

    [ModifyInput]
    public static ulong ProcessInput(string input)
    {
        var nextMask = 1;
        var elementToMask = new Dictionary<string, int>();

        int Mask(string element)
        {
            if (elementToMask.TryGetValue(element, out var value)) return value;
            if (elementToMask.Count == 5) throw new NotImplementedException();

            value = nextMask;
            elementToMask[element] = value;
            nextMask <<= 1;

            return value;
        }

        ulong state = 0;
        var floor = 0;
        foreach (var line in input.Split('\n'))
        {
            var chips = 0;
            foreach (Match m in ParseChipsReg.Matches(line))
            {
                chips += Mask(m.Groups[1].Value);
            }

            var generators = 0;
            foreach (Match m in ParseGensReg.Matches(line))
            {
                generators += Mask(m.Groups[1].Value);
            }

            state = state.SetFloor((ulong) floor, (ulong) chips, (ulong) generators);
            floor++;
        }

        return state;
    }

    [Answer(37)]
    public static long Part1(ulong inp) => Solve(inp);

    [Answer(61)]
    public static long Part2(ulong inp)
        => Solve(inp
            .AddGenerator(0, Element.Elerium).AddChip(0, Element.Elerium)
            .AddGenerator(0, Element.Dilithium).AddChip(0, Element.Dilithium));

    private static int Solve(ulong state)
    {
        var seen = new HashSet<ulong>();
        var q = new Queue<(int steps, ulong state)>();
        q.Enqueue((0, state));
        while (q.Count != 0)
        {
            (var steps, state) = q.Dequeue();
            if (state.Final()) return steps;

            foreach (var nextState in state.NextStates())
            {
                if (seen.Contains(nextState)) continue;
                q.Enqueue((steps + 1, nextState));
                seen.Add(nextState);
            }
        }

        return 0;
    }
}

internal enum Element
{
    Thulium = 0b1,
    Plutonium = 0b10,
    Strontium = 0b100,
    Promethium = 0b1000,
    Ruthenium = 0b10000,
    Elerium = 0b100000,
    Dilithium = 0b1000000
}

internal static class StateExtensions
{
    private const int ElementCount = 7;
    private const int ElevatorShift = 8 * ElementCount;
    private const int GeneratorShift = 0;

    private static int[] FloorShift = { 0, 2 * ElementCount, 4 * ElementCount, 6 * ElementCount };

    private const ulong ElevatorMask = 0b00111111111111111111111111111111111111111111111111111111;
    private const ulong ChipMask = 0b00000001111111;
    private const ulong GeneratorMask = 0b11111110000000;

    private static ulong[] FloorMask =
    {
        0b1111111111111111111111111111111111111111111100000000000000,
        0b1111111111111111111111111111110000000000000011111111111111,
        0b1111111111111111000000000000001111111111111111111111111111,
        0b1100000000000000111111111111111111111111111111111111111111
    };

    public static ulong SetFloor(this ulong state, ulong floor, ulong chips, ulong generators)
        => (state & FloorMask[floor]) |
           (((chips << ElementCount) | (generators << GeneratorShift)) << FloorShift[floor]);

    public static ulong GetElevator(this ulong state) => state >> ElevatorShift;

    public static ulong GetChips(this ulong state, ulong floor)
        => (((state & ~FloorMask[floor]) >> FloorShift[floor]) & ~ChipMask) >> ElementCount;

    public static ulong GetGenerators(this ulong state, ulong floor)
        => (((state & ~FloorMask[floor]) >> FloorShift[floor]) & ~GeneratorMask) >> GeneratorShift;

    public static ulong AddChip(this ulong state, ulong floor, Element chip)
        => state | ((ulong) chip << ElementCount << FloorShift[floor]);

    public static ulong AddGenerator(this ulong state, ulong floor, Element genetator)
        => state | ((ulong) genetator << GeneratorShift << FloorShift[floor]);

    public static bool Valid(this ulong state)
    {
        for (var floor = 3; floor >= 0; floor--)
        {
            var chips = state.GetChips((ulong) floor);
            var generators = state.GetGenerators((ulong) floor);
            var pairs = chips & generators;
            var unpairedChips = chips & ~pairs;
            if (unpairedChips != 0 && generators != 0) return false;
        }

        return true;
    }

    public static IEnumerable<ulong> NextStates(this ulong state)
    {
        var floor = state.GetElevator();
        for (ulong i = 1; i < 0b100000000000000; i <<= 1)
        {
            for (ulong j = 1; j < 0b100000000000000; j <<= 1)
            {
                var iOnFloor = i << FloorShift[floor];
                var jOnFloor = j << FloorShift[floor];
                if ((state & iOnFloor) == 0 || (state & jOnFloor) == 0) continue;
                ulong stateNext;
                if (floor > 0)
                {
                    var iOnPrevFloor = i << FloorShift[floor - 1];
                    var jOnPrevFloor = j << FloorShift[floor - 1];
                    var elevatorOnPrevFloor = (floor - 1) << ElevatorShift;
                    stateNext = (state & ~iOnFloor & ~jOnFloor & ElevatorMask) | iOnPrevFloor | jOnPrevFloor |
                                elevatorOnPrevFloor;
                    if (stateNext.Valid())
                        yield return stateNext;
                }

                if (floor >= 3) continue;
                var iOnNextFloor = i << FloorShift[floor + 1];
                var jOnNextFloor = j << FloorShift[floor + 1];
                var elevatorOnNextFloor = (floor + 1) << ElevatorShift;
                stateNext = (state & ~iOnFloor & ~jOnFloor & ElevatorMask) | iOnNextFloor | jOnNextFloor |
                            elevatorOnNextFloor;
                if (stateNext.Valid()) yield return stateNext;
            }
        }
    }

    public static bool Final(this ulong state)
        => (state & 0b0000000000000000111111111111111111111111111111111111111111) == 0;
}