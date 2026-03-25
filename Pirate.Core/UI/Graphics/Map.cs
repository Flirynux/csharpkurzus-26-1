using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Pirate.Core.UI.Graphics;
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
            int segmentIndex = (((row + topY) * Constants.MAP_WIDTH) + topX);
            int segmentIndexRight = segmentIndex + fixedDrawWidth + 1;
            char[] currentSegment = _map[segmentIndex..segmentIndexRight];
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
