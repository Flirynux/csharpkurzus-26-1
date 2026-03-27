using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;

using Pirate.Core.Utils;

internal class Navmap
{
    private readonly BitArray _map;

    public Navmap(string path)
    {
        // Read lines to preserve the grid structure (Rows/Cols)
        // This handles \r and \n automatically by splitting them into an array
        string[] lines = File.ReadAllLines(path);

        if (lines.Length == 0) throw new FileLoadException("Map file is empty.");


        _map = new BitArray(Constants.MAP_WIDTH * Constants.MAP_HEIGHT);

        for (int r = 0; r < Constants.MAP_HEIGHT; r++)
        {
            // Trim individual lines to remove potential trailing \r or spaces
            string line = lines[r].Trim();

            for (int c = 0; c < Constants.MAP_WIDTH; c++)
            {
                // Map 2D (r, c) to 1D index: (RowIndex * TotalColumns + ColumnIndex)
                if (c < line.Length && line[c] == '0')
                {
                    _map.Set(r * Constants.MAP_WIDTH + c, true);
                }
            }
        }
    }

    // Public read-only access
    public bool IsWalkable(int row, int col)
    {
        if (row < 0 || row >= Constants.MAP_HEIGHT || col < 0 || col >= Constants.MAP_WIDTH)
            return false;

        return _map.Get(row * Constants.MAP_WIDTH + col);
    }
}