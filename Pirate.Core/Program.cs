using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

using Pirate.Core.entities;
using Pirate.Core.entities.ships.types;

namespace Pirate.Core;

internal class Program
{
    static void Main(string[] args)
    {
        Faction brittain = new Faction(FactionType.ENGLISH, "Britts", 10000);
        for (int i = 0; i < 10; i++)
        {
            new Sloop(brittain, i.ToString(), new Vector2(i, i));
        }
        Player player = new Player("Playa");
        Console.WriteLine(brittain.ToString());
        Console.WriteLine(player.ToString());
        Console.ReadKey();
    }
}
