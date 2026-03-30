using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

using Pirate.Core.entities;
using Pirate.Core.entities.ships.types;
using Pirate.Core.UI;
using Pirate.Core.UI.Graphics;

namespace Pirate.Core;

internal class Program
{
    static void Main(string[] args)
    {
        Console.CursorVisible = false;
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "map_binary.txt");
        Map map = new Map(filePath);
        Navmap navmap = new Navmap(filePath);
        Random rand = new Random();
        Player player = new Player(navmap, rand, "Playa");
        Camera camera = new Camera(player);
        camera.AddObject(map);
        //string[] options = new string[2];
        //options[0] = "asdasd";
        //options[1] = "asdasdasdasd";
        //Menu menu = new Menu(options);
        Engine engine = new Engine(camera, player, navmap, rand);
        engine.Init();
        var input = new ConsoleKeyInfo();
        while(input.Key != ConsoleKey.Escape)
        {
            engine.Update();
            input = Console.ReadKey(true);
            player.HandleInput(input.Key);
        }
        return;
    }
}
