using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using Pirate.Core.entities;

namespace Pirate.Core.UI;
static class Constants
{
    public static int MAP_WIDTH = 320;
    public static int MAP_HEIGHT = 240;
    public static int DRAW_WIDTH = Console.WindowWidth;
    public static int DRAW_HEIGHT = Console.WindowHeight-1;
}

internal class Camera
{
    List<IDrawable> _drawables = new List<IDrawable>();
    //Camera anchored to player
    Player _player;
    public Camera(Player player) 
    {
        _player = player;
        AddObject(player);
    }
    public Camera(Player player, List<IDrawable> objects)
    {
        _player = player;
        _drawables = objects;
    }
    public void AddObject(IDrawable obj)
    {
        _drawables.Add(obj);
    }

    public void AddObject(List<IDrawable> objects)
    {
        foreach (var obj in objects)
        {
            AddObject(obj);
        }
    }

    public void RemoveObject(IDrawable obj)
    {
        _drawables.Remove(obj);
    }

    public void Render()
    {
        Position pos = _player.Position;

        var sorted = _drawables.OrderBy(obj => obj.Priority);
        foreach (var item in sorted)
        {
            item.Draw(pos);
        }
    }
}
internal class Map : IDrawable
{
    private char[] _map;
    DrawPriority IDrawable.Priority => DrawPriority.MAP;

    public Map(string path)
    {
        string temp = File.ReadAllText(path);
        temp = temp.Replace("\n", "").Replace("\r", "").Replace(" ", "");
        _map = temp.ToCharArray();
    }

    public void Draw(int x, int y)
    {
        Console.SetCursorPosition(0,0);
        int topX = x - (Constants.DRAW_WIDTH / 2);
        int topY = y - (Constants.DRAW_HEIGHT / 2);
        int fixedDrawWidth = Math.Min(Constants.MAP_WIDTH- topX, Constants.DRAW_WIDTH);
        int fixedDrawHeight= Math.Min(Constants.MAP_HEIGHT- topY, Constants.DRAW_HEIGHT);
        for (int row = 0; row < fixedDrawHeight; row++)
        {
            List<char[]> lineSegments = new List<char[]>();
            char[] currentSegment = _map[(((row + topY) * Constants.MAP_WIDTH) + topX)..((((row + topY) * Constants.MAP_WIDTH) + topX)+fixedDrawWidth+1)];
            int startOfSegment = 0;
            for (int i = 1; i < fixedDrawWidth+1; i++)
            {
                if(currentSegment[i] != currentSegment[i - 1])
                {
                    char[] temp = currentSegment[startOfSegment..i];
                    startOfSegment = i;
                    lineSegments.Add(temp);
                }
                else if(i == fixedDrawWidth)
                {
                    char[] temp = currentSegment[startOfSegment..i];
                    lineSegments.Add(temp);
                }
            }
            foreach (var item in lineSegments)
            {
                switch (item[0])
                {
                    case '0':
                        Console.BackgroundColor = ConsoleColor.Green;
                        break;
                    case '1':
                        Console.BackgroundColor= ConsoleColor.Blue;
                        break;
                    default:
                        Console.ResetColor();
                        break;
                }
                Console.Write(item);
            }
            Console.WriteLine();
        }
    }
}

public interface IDrawable
{
    public DrawPriority Priority { get; }
    public void Draw(int x, int y);

    public void Draw(Position position)
    {
        Draw(position.x, position.y);
    }
}

public enum DrawPriority
{
    MAP,
    SHIPS,
    SETTLEMENTS,
    PLAYER,
    MENU,
}