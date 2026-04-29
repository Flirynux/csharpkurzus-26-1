using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

using Pirate.Core.entities;
using Pirate.Core.entities.ships.types;
using Pirate.Core.UI;
using Pirate.Core.UI.Graphics;

namespace Pirate.Core;

internal class Program
{
    // Needed for ANSI output
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr GetStdHandle(int nStdHandle);

    public static void EnableAnsi()
    {
        var hOut = GetStdHandle(-11); // STD_OUTPUT_HANDLE
        GetConsoleMode(hOut, out uint mode);
        SetConsoleMode(hOut, mode | 0x0004); // ENABLE_VIRTUAL_TERMINAL_PROCESSING
    }

    static void Main(string[] args)
    {
        Console.CursorVisible = false;
        EnableAnsi();
        Engine engine = new Engine();
        engine.Init();
        Stopwatch timer = new Stopwatch();
        double lastTick = 0;
        timer.Start();
        // 30 FPS because I can't tell the difference here, from 60
        // Less resource usage (obv)
        double targetFrameTime = 1.0 /30.0;
        while(engine.State != EngineState.EXIT)
        {
            double currentTick = timer.Elapsed.TotalSeconds;
            float deltaTime = (float) (currentTick - lastTick);
            lastTick = currentTick;
            engine.Update(deltaTime);
            engine.Render();

            double frameProcessingTime = timer.Elapsed.TotalSeconds - currentTick;

            double sleepTime = targetFrameTime - frameProcessingTime;

            if (sleepTime > 0.002)
                Thread.Sleep((int)((sleepTime - 0.002) * 1000));

            // Busy wait, read that it makes it more responsive
            // Not sure if I feel it but couldn't hurt
            while (timer.Elapsed.TotalSeconds - currentTick < targetFrameTime)
            { }
        }
        Environment.Exit(0);
    }
}
