using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 20, "WIP"), Run]
file class Day20
{
    [ModifyInput]
    public static (string[] broadcaster, Dictionary<string, Module> modules) ProcessInput(string input)
    {
        string[] broadcaster = [];
        Dictionary<string, string[]> rawModules = new();
        Dictionary<string, Module> modules = new();

        foreach (var line in input.Split('\n'))
        {
            var split = line.Split(" -> ");
            if (line.StartsWith("broadcaster"))
            {
                broadcaster = split[1].Split(", ");
                continue;
            }

            rawModules.Add(split[0][1..], split[1].Split(", "));
            modules.Add(split[0][1..], split[0][0] is '%' ? new FlipFlop() : new Conjunction());
        }

        foreach (var (key, outputs) in rawModules)
        {
            var module = modules[key];
            module.Outputs.AddRange(outputs);

            foreach (var output in outputs)
            {
                if (!modules.TryGetValue(output, out var outputModule))
                {
                    modules[output] = new Output();
                    continue;
                }

                if (outputModule is not Conjunction conjunction) continue;
                conjunction.Listening[key] = Signal.Low;
            }
        }

        return (broadcaster, modules);
    }

    [Answer(899848294)]
    public static long Part1((string[] broadcaster, Dictionary<string, Module> modules) inp)
    {
        var (broadcaster, modules) = inp;

        var count1 = 0L;
        var count2 = 0L;
        for (var i = 0; i < 1000; i++)
        {
            var (low, high) = Run();
            count1 += low;
            count2 += high;
        }

        return count1 * count2;

        (long low, long high) Run()
        {
            long[] sentSignals = [0, 0];
            Queue<State> signalStates = new();
            sentSignals[0]++;
            foreach (var output in broadcaster)
            {
                sentSignals[0]++;
                signalStates.Enqueue(new State(output, "broadcaster", Signal.Low));
            }

            while (signalStates.Count > 0)
            {
                var (module, lastModule, signal) = signalStates.Dequeue();

                var receivedSignal = modules[module].Operate(signal, lastModule);
                if (receivedSignal is Signal.None) continue;
                sentSignals[receivedSignal is Signal.Low ? 0 : 1] += modules[module].Outputs.Count;
                foreach (var output in modules[module].Outputs)
                {
                    signalStates.Enqueue(new State(output, module, receivedSignal));
                }
            }

            return (sentSignals[0], sentSignals[1]);
        }
    }

    [Answer(247454898168563)]
    public static long Part2((string[] broadcaster, Dictionary<string, Module> modules) inp)
    {
        var (broadcaster, modules) = inp;

        var rxConjunction = modules.First(kv => kv.Value.Outputs.Contains("rx")).Key;
        var required = modules.Where(kv => kv.Value.Outputs.Contains(rxConjunction))
            .Select(kv => kv.Key).ToDictionary(k => k, _ => 0L);

        var buttonPresses = 0;

        while (Run())
        {
        }

        return required.Values.Aggregate((a, b) => a.LCM(b));

        bool Run()
        {
            Queue<State> signalStates = new();
            foreach (var output in broadcaster)
            {
                signalStates.Enqueue(new State(output, "broadcaster", Signal.Low));
            }

            buttonPresses++;

            while (signalStates.Count > 0)
            {
                var (module, lastModule, signal) = signalStates.Dequeue();

                var receivedSignal = modules[module].Operate(signal, lastModule);

                if (required.TryGetValue(module, out var l) && l == 0 && receivedSignal == Signal.High)
                {
                    required[module] = buttonPresses;
                }

                if (required.Values.All(l => l != 0)) return false;

                if (receivedSignal is Signal.None) continue;
                foreach (var output in modules[module].Outputs)
                {
                    signalStates.Enqueue(new State(output, module, receivedSignal));
                }
            }

            return true;
        }
    }
}

file readonly struct State(string module, string lastModule, Signal signal)
{
    public readonly string Module = module;
    public readonly string LastModule = lastModule;
    public readonly Signal Signal = signal;

    public void Deconstruct(out string module, out string lastModule, out Signal signal)
    {
        module = Module;
        lastModule = LastModule;
        signal = Signal;
    }
}

file abstract class Module
{
    public readonly List<string> Outputs = [];

    public abstract Signal Operate(Signal signal, string lastModule);
}

file class Output : Module
{
    public override Signal Operate(Signal signal, string lastModule) => Signal.None;
}

file class FlipFlop : Module
{
    private bool On;

    public override Signal Operate(Signal signal, string lastModule)
    {
        if (signal is Signal.High) return Signal.None;
        On = !On;
        return On ? Signal.High : Signal.Low;
    }
}

file class Conjunction : Module
{
    public readonly Dictionary<string, Signal> Listening = new();

    public override Signal Operate(Signal signal, string lastModule)
    {
        if (lastModule == "broadcaster") return Signal.None;
        Listening[lastModule] = signal;
        return Listening.Values.All(signal => signal is Signal.High) ? Signal.Low : Signal.High;
    }
}

file enum Signal
{
    None,
    High,
    Low
}