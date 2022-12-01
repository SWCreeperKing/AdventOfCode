using System;

namespace AdventOfCode.Better_Run;

public abstract class Puzzle<TInput, TOutput> : IStartable where TOutput : IEquatable<TOutput>
{
    private string Input { init; get; }
    public abstract (TOutput part1, TOutput part2) Result { get; }
    public abstract (int year, int day) PuzzleSolution { get; }

    public Puzzle()
    {
        Input = Inputs.inputs[(PuzzleSolution.year, PuzzleSolution.day)];
        Inputs.puzzles.Add(new Inputs.Puzz(PuzzleSolution.year, PuzzleSolution.day, 1), Part1Real);
        Inputs.puzzles.Add(new Inputs.Puzz(PuzzleSolution.year, PuzzleSolution.day, 2), Part2Real);
    }
    //
    // public void AddToPuzzleList()
    // {
    //     puzzles.Add(new Puzz(PuzzleSolution.year, PuzzleSolution.day, 1), Part1Real);
    //     puzzles.Add(new Puzz(PuzzleSolution.year, PuzzleSolution.day, 2), Part2Real);
    // }

    public (Inputs.State, string) Part1Real()
    {
        var res = Part1(ProcessInput(Input));
        return (res is null || Result.Item1 is null
            ? Inputs.State.Fail
            : res.Equals(Result.Item1)
                ? Inputs.State.Success
                : Result.Item1.Equals(default)
                    ? Inputs.State.Possible
                    : Inputs.State.Fail, res!.ToString());
    }    
        
    public (Inputs.State, string) Part2Real()
    {
        var res = Part2(ProcessInput(Input));
        return (res is null || Result.Item2 is null
            ? Inputs.State.Fail
            : res.Equals(Result.Item2)
                ? Inputs.State.Success
                : Result.Item2.Equals(default)
                    ? Inputs.State.Possible
                    : Inputs.State.Fail, res!.ToString());
    }

    public abstract TInput ProcessInput(string input);
    public abstract TOutput Part1(TInput inp);
    public abstract TOutput Part2(TInput inp);
}

public interface IStartable
{
    // public void AddToPuzzleList();
}