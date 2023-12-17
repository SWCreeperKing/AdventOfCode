using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2015;

[Day(2015, 21, "RPG Simulator 20XX")]
file class Day21
{
    private static readonly List<Item> Weapons =
        [new Item(8, 4), new Item(10, 5), new Item(25, 6), new Item(40, 7), new Item(74, 8)];

    private static readonly List<Item> Armors =
        [new Item(13, 0, 1), new Item(31, 0, 2), new Item(53, 0, 3), new Item(75, 0, 4), new Item(102, 0, 5)];

    private static readonly List<Item> Rings =
    [
        new Item(25, 1), new Item(50, 2), new Item(100, 3), new Item(20, 0, 1), new Item(40, 0, 2), new Item(80, 0, 3)
    ];

    [ModifyInput]
    public static (Entity, List<Player> playerStates) ProcessInput(string input)
    {
        var playerStates = Weapons.Select(weapon => new Player(100, weapon.Damage, 0, weapon.Cost)).ToList();
        var cachedStates = playerStates.ToArray();
        foreach (var armor in Armors)
        {
            playerStates.AddRange(cachedStates.Select(playerState => playerState + armor));
        }

        List<Item> ringCombos = [];
        ringCombos.AddRange(Rings);

        for (var i = 0; i < Rings.Count; i++)
        {
            ringCombos.AddRange(Rings.Where((_, j) => i != j).Select(t => Rings[i] + t));
        }

        cachedStates = playerStates.ToArray();
        foreach (var ringCombo in ringCombos)
        {
            playerStates.AddRange(cachedStates.Select(playerState => playerState + ringCombo));
        }

        var arr = input.Split('\n').Select(s => s.Split(": ")[1]).Select(int.Parse).ToArray();
        return (new Entity(arr[0], arr[1], arr[2]), playerStates);
    }

    [Answer(78)]
    public static int Part1((Entity, List<Player> playerStates) inp)
        => inp.playerStates.Where(ps => Fight(ps, inp.Item1)).Min(ps => ps.MoneySpent);

    [Answer(148)]
    public static int Part2((Entity, List<Player> playerStates) inp)
        => inp.playerStates.Where(ps => !Fight(ps, inp.Item1)).Max(ps => ps.MoneySpent);

    public static bool Fight(Player player, Entity boss)
    {
        if (boss.Hp <= 0) return true;
        if (player.Hp <= 0) return false;
        return Fight(player with
        {
            Hp = player.Hp - Math.Max(1, boss.Damage - player.Armor)
        }, boss with
        {
            Hp = boss.Hp - Math.Max(1, player.Damage - boss.Armor)
        });
    }
}

public readonly struct Item(int cost = 0, int damage = 0, int armor = 0)
{
    public readonly int Cost = cost;
    public readonly int Damage = damage;
    public readonly int Armor = armor;

    public static Item operator +(Item i1, Item i2)
        => new(i1.Cost + i2.Cost, i1.Damage + i2.Damage, i1.Armor + i2.Armor);
}

public record Player(int Hp, int Damage, int Armor, int MoneySpent)
{
    public static Player operator +(Player player, Item i)
        => player with
        {
            MoneySpent = player.MoneySpent + i.Cost,
            Armor = player.Armor + i.Armor,
            Damage = player.Damage + i.Damage
        };
}

public record Entity(int Hp, int Damage, int Armor);