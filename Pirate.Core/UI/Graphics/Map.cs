using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using Pirate.Core.Utils;

namespace Pirate.Core.UI.Graphics;
internal class Map : IDrawable
{
    private readonly char[] _map;
    DrawPriority IDrawable.Priority => DrawPriority.MAP;

    public Map(string path)
    {
        string[] lines = File.ReadAllLines(path);

        if (lines.Length == 0) throw new FileLoadException("Map file is empty.");
        string temp = lines[0].Trim();
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            temp = string.Concat(temp, line);
        }
        _map = temp.ToCharArray();
    }

    public void Draw(int x, int y)
    {
        Console.SetCursorPosition(0,0);
        int topX = x - (Constants.DRAW_WIDTH / 2);
        int topY = y - (Constants.DRAW_HEIGHT / 2);
        for (int row = 0; row < Constants.DRAW_HEIGHT; row+=2)
        {
            Console.SetCursorPosition(0, row/2);
            int lineIndex = row + topY;
            int segmentIndex = lineIndex * Constants.MAP_WIDTH + topX;
            int segmentIndexRight = segmentIndex + Constants.DRAW_WIDTH;

            char[] top = _map[segmentIndex..segmentIndexRight];
            lineIndex++;
            segmentIndex = lineIndex * Constants.MAP_WIDTH + topX;
            segmentIndexRight = segmentIndex + Constants.DRAW_WIDTH;
            char[] bottom = _map[segmentIndex..segmentIndexRight];

            List<int> lengths = getSegmentsLengths(top, bottom);
            List<char[]> lines = getLineSegments(lengths);
            List<CombinedConsoleColor> colors = getColors(top, bottom, lengths);

            var colorIterator = colors.GetEnumerator();
            foreach (var item in lines)
            {
                colorIterator.MoveNext();
                CombinedConsoleColor color = colorIterator.Current;
                color.Apply();
                Console.Write(item);
            }
            Console.ResetColor();
        }
    }

    private List<int> getSegmentsLengths(char[] lineTop, char[] lineBottom)
    {
        List<int> res = new List<int>();
        int currSegmentStart = 0;
        char currTop = lineTop[0];
        char currBottom = lineBottom[0];
        for (int i = 0; i < lineTop.Length; i++) 
        { 
            if(currTop != lineTop[i] || currBottom != lineBottom[i])
            {
                res.Add(i-currSegmentStart);
                currSegmentStart = i;
                currTop = lineTop[i];
                currBottom = lineBottom[i];
            } 
            else if(i == lineTop.Length - 1)
            {
                res.Add(i-currSegmentStart);
            }
        }
        return res;
    }

    private List<CombinedConsoleColor> getColors(char[] lineTop, char[] lineBottom, List<int> lengths)
    {
        List<CombinedConsoleColor> res = new List<CombinedConsoleColor>();
        int currLength = 0;
        foreach (var item in lengths)
        {
            char top = lineTop[currLength+item-1];
            char bottom = lineBottom[currLength+item-1];
            ConsoleColor topColor = getColorFromChar(top);
            ConsoleColor bottomColor = getColorFromChar(bottom);
            CombinedConsoleColor temp = new CombinedConsoleColor(topColor, bottomColor);
            res.Add(temp);
            currLength += item;
        }
        return res;
    }

    private ConsoleColor getColorFromChar(char character)
    {
        switch (character)
        {
            case '0':
                return ConsoleColor.DarkBlue;
            case '1':
                return ConsoleColor.Green;
            default:
                return ConsoleColor.Black;
        }
    }

    private List<char[]> getLineSegments(List<int> lengths)
    {
        List<char[]> res = new List<char[]>();
        foreach (var item in lengths)
        {
            char[] temp = new char[item];
            for (int i = 0; i < item; i++)
            {
                temp[i] = '\u2580';
            }
            res.Add(temp);
        }
        return res;
    }
}
