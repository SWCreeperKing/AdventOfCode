using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;
using static AdventOfCode.Helper;

namespace AdventOfCode.Solutions._2015;

[Day(2015, 21, "RPG Simulator 20XX")]
public class Day21
{
    private static readonly List<Item> Weapons = new()
    {
        new Item(8, 4), new Item(10, 5), new Item(25, 6), new Item(40, 7), new Item(74, 8)
    };

    private static readonly List<Item> Armors = new()
    {
        new Item(13, 0, 1), new Item(31, 0, 2), new Item(53, 0, 3), new Item(75, 0, 4), new Item(102, 0, 5)
    };

    private static readonly List<Item> Rings = new()
    {
        new Item(25, 1), new Item(50, 2), new Item(100, 3), new Item(20, 0, 1), new Item(40, 0, 2), new Item(80, 0, 3)
    };

    [ModifyInput]
    public static Entity ProcessInput(string input)
    {
        var arr = input.Split('\n').Select(s => s.Split(": ")[1]).Select(int.Parse).ToArray();
        return new Entity(arr[0], arr[1], arr[2]);
    }

    [Answer(78)]
    public static int Part1(Entity inp)
    {
        List<int> costs = new();

        foreach (var (weapon, armor, ring) in Iterate())
        {
            var items = Weapons.GetFrom(weapon).Concat(Armors.GetFrom(armor)).Concat(Rings.GetFrom(ring));
            var player = new Entity(100, items.Sum(i => i.Damage), items.Sum(i => i.Armor));
            var enemy = inp;

            while (player.Hp > 0 && enemy.Hp > 0)
            {
                var playerDamage = Math.Max(player.Damage - enemy.Armor, 1);
                var enemyDamage = Math.Max(enemy.Damage - player.Armor, 1);
                enemy = enemy with { Hp = enemy.Hp - playerDamage };
                if (enemy.Hp <= 0) break;
                player = player with { Hp = player.Hp - enemyDamage };
            }

            if (player.Hp <= 0) continue;
            costs.Add(items.Sum(i => i.Cost));
        }

        return costs.Min();
    }

    [Answer(148)]
    public static int Part2(Entity inp)
    {
        List<int> costs = new();

        foreach (var (weapon, armor, ring) in Iterate())
        {
            var items = Weapons.GetFrom(weapon).Concat(Armors.GetFrom(armor)).Concat(Rings.GetFrom(ring));
            var player = new Entity(100, items.Sum(i => i.Damage), items.Sum(i => i.Armor));
            var enemy = inp;

            while (player.Hp > 0 && enemy.Hp > 0)
            {
                var playerDamage = Math.Max(player.Damage - enemy.Armor, 1);
                var enemyDamage = Math.Max(enemy.Damage - player.Armor, 1);
                enemy = enemy with { Hp = enemy.Hp - playerDamage };
                if (enemy.Hp <= 0) break;
                player = player with { Hp = player.Hp - enemyDamage };
            }

            if (player.Hp > 0) continue;
            costs.Add(items.Sum(i => i.Cost));
        }

        return costs.Max();
    }

    private static IEnumerable<(IReadOnlyList<bool>, IReadOnlyList<bool>, IReadOnlyList<bool>)> Iterate()
    {
        return from weapon in SwitchingBool(Weapons.Count, 1, 1)
            from armor in SwitchingBool(Armors.Count, 1)
            from ring in SwitchingBool(Rings.Count, 2)
            select (weapon, armor, ring);
    }
}

public readonly struct Item
{
    public readonly int Cost;
    public readonly int Damage;
    public readonly int Armor;

    public Item(int cost = 0, int damage = 0, int armor = 0)
    {
        this.Cost = cost;
        this.Damage = damage;
        this.Armor = armor;
    }
}

public record Entity(int Hp, int Damage, int Armor);