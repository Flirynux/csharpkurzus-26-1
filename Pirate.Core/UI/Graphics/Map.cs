using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using Pirate.Core.Utils;

namespace Pirate.Core.UI.Graphics;
internal class Map : IDrawable
{
    private readonly char[][] _map = new char[Constants.MAP_HEIGHT][];
    DrawPriority IDrawable.Priority => DrawPriority.MAP;

    public Map(string path)
    {
        string[] lines = File.ReadAllLines(path);

        if (lines.Length == 0) throw new IOException("Map file is empty.");
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            _map[i] = line.ToCharArray();
        }
    }

    public void Draw(RenderBuffer renderBuffer, int x, int y)
    {
        // Calculate where the top left of the cosole is on the map
        int topX = x - (Constants.DRAW_WIDTH / 2);
        int topY = y - (Constants.DRAW_HEIGHT / 2);

        for (int row = 0; row < renderBuffer.Height; row++)
        {
            for (int col = 0; col < renderBuffer.Width; col++)
            {
                // Map coordinate
                int mapY = row + topY;
                int mapX = col + topX;

                // Bounds check to ensure we don't read outside the map array
                if (mapX >= 0 && mapX < Constants.MAP_WIDTH 
                    && mapY >= 0 && mapY < Constants.MAP_HEIGHT)
                {
                    char mapChar = _map[mapY][mapX];

                    RGB pixelColor = getColorFromChar(mapChar);
                    renderBuffer[col, row] = new Pixel
                    {
                        Character = '\u2580',
                        textRGB = pixelColor,
                        bgRGB = new RGB(0, 0, 0)
                    };
                }
                
            }
        }
    }

    // Get color based on map file
    private RGB getColorFromChar(char character)
    {
        switch (character)
        {
            case '0': // Water
                return new RGB { R = 0, G = 0, B = 127 };
            case '1': // Land
                return new RGB { R = 0, G = 255, B = 0 };
            default:
                return new RGB { R = 0, G = 0, B = 0 };
        }
    }
}
