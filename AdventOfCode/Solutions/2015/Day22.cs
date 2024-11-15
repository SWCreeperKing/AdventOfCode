using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2015;

[Day(2015, 22, "Wizard Simulator 20XX")]
file class Day22
{
    [ModifyInput]
    public static GameState ProcessInput(string input)
    {
        return input.Split('\n').Inline(arr => new GameState(
            int.Parse(arr[0][(arr[0].IndexOf(": ", StringComparison.Ordinal) + 1)..]),
            int.Parse(arr[1][(arr[1].IndexOf(": ", StringComparison.Ordinal) + 1)..])));
    }

    [Answer(1269)]
    public static long Part1(GameState inp)
    {
        return Run(inp);
    }

    [Answer(1309)]
    public static long Part2(GameState inp)
    {
        return Run(inp, true);
    }

    public static int Run(GameState initState, bool part2 = false)
    {
        HashSet<int> seen = [];
        PriorityQueue<GameState, int> states = new();
        states.Enqueue(initState, initState.ManaUsed);

        while (states.Count > 0)
        {
            var state = states.Dequeue().RunEffects(part2);
            if (state.Hp < 1) continue;

            foreach (var nextState in state.PlayerTurn().Select(s => s.RunEffects()))
            {
                if (nextState.BossHp < 1) return nextState.ManaUsed;

                var afterBoss = nextState.BossTurn();
                if (afterBoss.BossHp < 1) return afterBoss.ManaUsed;
                if (afterBoss.Hp < 1) continue;

                if (!seen.Add(afterBoss.GetHashCode())) continue;

                states.Enqueue(afterBoss, afterBoss.ManaUsed);
            }
        }

        return -1;
    }
}

public record GameState(
    int BossHp,
    int BossDamage,
    int Hp = 50,
    int Mana = 500,
    int Shield = 0,
    int Poison = 0,
    int Recharge = 0,
    int ManaUsed = 0)
{
    public GameState RunEffects(bool part2 = false)
    {
        return this with
        {
            BossHp = Poison > 0 ? BossHp - 3 : BossHp,
            Hp = part2 ? Hp - 1 : Hp,
            Mana = Recharge > 0 ? Mana + 101 : Mana,
            Shield = Shield > 0 ? Shield - 1 : 0,
            Poison = Poison > 0 ? Poison - 1 : 0,
            Recharge = Recharge > 0 ? Recharge - 1 : 0
        };
    }

    public List<GameState> PlayerTurn()
    {
        if (Mana < 53) return [];
        List<GameState> states = [this with { Mana = Mana - 53, ManaUsed = ManaUsed + 53, BossHp = BossHp - 4 }];

        if (Mana < 73) return states;
        states.Add(this with { Mana = Mana - 73, ManaUsed = ManaUsed + 73, BossHp = BossHp - 2, Hp = Hp + 2 });

        if (Mana < 113) return states;
        if (Shield == 0) states.Add(this with { Mana = Mana - 113, ManaUsed = ManaUsed + 113, Shield = 6 });

        if (Mana < 173) return states;
        if (Poison == 0) states.Add(this with { Mana = Mana - 173, ManaUsed = ManaUsed + 173, Poison = 6 });

        if (Mana < 229) return states;
        if (Recharge == 0) states.Add(this with { Mana = Mana - 229, ManaUsed = ManaUsed + 229, Recharge = 5 });

        return states;
    }

    public GameState BossTurn()
    {
        return this with { Hp = Hp - (BossDamage - (Shield > 0 ? 7 : 0)) };
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(BossHp, BossDamage, Hp, Mana, Shield, Poison, Recharge, ManaUsed);
    }
}