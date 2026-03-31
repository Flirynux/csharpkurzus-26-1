using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

using Pirate.Core.entities;
using Pirate.Core.entities.ships.types;
using Pirate.Core.UI;
using Pirate.Core.UI.Graphics;
using System.Diagnostics;

namespace Pirate.Core;

internal class Program
{
    static void Main(string[] args)
    {
        Console.CursorVisible = false;
        //string[] options = new string[2];
        //options[0] = "asdasd";
        //options[1] = "asdasdasdasd";
        //Menu menu = new Menu(options);
        Engine engine = new Engine();
        engine.Init();
        while(engine.State != EngineState.EXIT)
        {
            engine.Update();
        }
        return;
    }
}
