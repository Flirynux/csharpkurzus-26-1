using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Reflection.Metadata;

using Pirate.Core.Utils;

internal class Navmap
{
    private readonly BitArray _map;

    public Navmap(string path)
    {
        string[] lines = File.ReadAllLines(path);

        if (lines.Length == 0) throw new FileLoadException("Map file is empty.");

        _map = new BitArray(Constants.MAP_WIDTH * Constants.MAP_HEIGHT);

        for (int r = 0; r < Constants.MAP_HEIGHT; r++)
        {
            string line = lines[r].Trim();

            for (int c = 0; c < Constants.MAP_WIDTH; c++)
            {
                if (line[c] == '0')
                {
                    _map.Set(r * Constants.MAP_WIDTH + c, true);
                }
            }
        }
    }

    public bool IsSailable(int x, int y)
    {
        if (y < 0 || y >= Constants.MAP_HEIGHT || x < 0 || x >= Constants.MAP_WIDTH)
            return false;

        return _map.Get(y * Constants.MAP_WIDTH + x);
    }

    public bool IsSailable(Vector2 position)
    {
        return IsSailable((int)Math.Round(position.X), (int)Math.Round(position.Y));
    }
}