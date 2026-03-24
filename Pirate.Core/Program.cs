using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

using Pirate.Core.entities;
using Pirate.Core.entities.ships.types;
using Pirate.Core.UI;

namespace Pirate.Core;

internal class Program
{
    static void Main(string[] args)
    {
        Console.CursorVisible = false;
        Faction brittain = new Faction(FactionType.ENGLISH, "Britts", 10000);
        for (int i = 0; i < 10; i++)
        {
            new Sloop(brittain, i.ToString(), new Vector2(i, i));
        }
        Player player = new Player("Playa");
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "map_binary.txt");
        Map map = new Map(filePath);
        Camera camera = new Camera(player);
        camera.AddObject(map);
        camera.Render();
        //map.Draw(95, 205);
        Console.ReadKey();
    }
}
