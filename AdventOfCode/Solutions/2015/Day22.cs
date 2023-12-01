using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2015;

/*
 * I completely admit my loss on this
 * I took the solution from: https://github.com/encse/adventofcode/blob/master/2015/Day22/Solution.cs
 * and then modified it to use Records
 */
[Day(2015, 22, "Wizard Simulator 20XX")]
public class Day22
{
    [ModifyInput]
    public static GameState ProcessInput(string input)
    {
        var split = input.Split('\n').Select(s => s[(s.IndexOf(':') + 2)..]).Select(int.Parse).ToArray();
        return new GameState(split[0], split[1], 50, 500);
    }

    [Answer(1269)]
    public static long Part1(GameState inp)
    {
        return BinarySearch(mana => TrySolve(inp.WithManaLimit(mana), false));
    }

    [Answer(1309)]
    public static long Part2(GameState inp)
    {
        return BinarySearch(mana => TrySolve(inp.WithManaLimit(mana), true));
    }

    private static int BinarySearch(Func<int, bool> f)
    {
        var hi = 1;
        while (!f(hi)) hi *= 2;

        var lo = hi / 2;
        while (hi - lo > 1)
        {
            var m = (hi + lo) / 2;
            if (f(m)) hi = m;
            else lo = m;
        }

        return hi;
    }

    private static bool TrySolve(GameState gs, bool hard)
    {
        if (hard) gs = gs.Damage(1);
        gs = gs.ApplyEffects();

        foreach (var stateT in gs.PlayerSteps())
        {
            gs = stateT.ApplyEffects();
            gs = gs.BossStep();
            if (gs.BossHp > 0 && (gs.PlayerHp <= 0 || !TrySolve(gs, hard))) continue;
            return true;
        }

        return false;
    }
}

public record GameState(int BossHp, int BossDamage, int PlayerHp, int PlayerMana, int PlayerArmor = 0,
    int ManaLimit = 0, int UsedMana = 0, int ShieldTurns = 0, int PoisonTurns = 0, int RechargeTurns = 0);

public static class Runner
{
    public static GameState WithManaLimit(this GameState gs, int manaLimit) => gs with { ManaLimit = manaLimit };

    public static GameState ApplyEffects(this GameState gs)
    {
        if (gs.PlayerHp <= 0 || gs.BossHp <= 0) return gs;

        var gsHolder = gs;
        if (gsHolder.PoisonTurns > 0)
        {
            gsHolder = gsHolder with { BossHp = gsHolder.BossHp - 3, PoisonTurns = gsHolder.PoisonTurns - 1 };
        }

        if (gsHolder.RechargeTurns > 0)
        {
            gsHolder = gsHolder with
            {
                PlayerMana = gsHolder.PlayerMana + 101, RechargeTurns = gsHolder.RechargeTurns - 1
            };
        }

        if (gsHolder.ShieldTurns > 0)
        {
            gsHolder = gsHolder with { PlayerArmor = 7, ShieldTurns = gsHolder.ShieldTurns - 1 };
        }
        else gsHolder = gsHolder with { PlayerArmor = 0 };

        return gsHolder;
    }

    public static GameState Damage(this GameState gs, int damage)
    {
        return gs.PlayerHp <= 0 || gs.BossHp <= 0 ? gs : gs with { PlayerHp = gs.PlayerHp - damage };
    }

    public static GameState BossStep(this GameState gs)
    {
        return gs.PlayerHp <= 0 || gs.BossHp <= 0
            ? gs
            : gs with { PlayerHp = gs.PlayerHp - Math.Max(1, gs.BossDamage - gs.PlayerArmor) };
    }

    public static IEnumerable<GameState> PlayerSteps(this GameState gs)
    {
        if (gs.PlayerHp <= 0 || gs.BossHp <= 0)
        {
            yield return gs;
            yield break;
        }

        if (gs.PlayerMana >= 53 && 53 + gs.UsedMana <= gs.ManaLimit)
        {
            yield return gs.UseMana(53) with { BossHp = gs.BossHp - 4 };
        }

        if (gs.PlayerMana >= 73 && 73 + gs.UsedMana <= gs.ManaLimit)
        {
            yield return gs.UseMana(73) with
            {
                BossHp = gs.BossHp - 2, PlayerHp = gs.PlayerHp + 2
            };
        }

        if (gs.PlayerMana >= 113 && gs.ShieldTurns == 0 && 113 + gs.UsedMana <= gs.ManaLimit)
        {
            yield return gs with { PlayerMana = gs.PlayerMana - 113, UsedMana = gs.UsedMana + 113, ShieldTurns = 6 };
        }

        if (gs.PlayerMana >= 173 && gs.PoisonTurns == 0 && 173 + gs.UsedMana <= gs.ManaLimit)
        {
            yield return gs.UseMana(173) with { PoisonTurns = 6 };
        }

        if (gs.PlayerMana < 229 || gs.RechargeTurns != 0 || 229 + gs.UsedMana > gs.ManaLimit) yield break;
        yield return gs.UseMana(229) with { RechargeTurns = 5 };
    }

    private static GameState UseMana(this GameState gs, int mana)
    {
        return gs with { PlayerMana = gs.PlayerMana - mana, UsedMana = gs.UsedMana + mana };
    }
}