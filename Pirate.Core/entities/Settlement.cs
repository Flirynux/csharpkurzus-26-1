using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Pirate.Core.UI.Graphics;
using Pirate.Core.Utils;

namespace Pirate.Core.entities;

internal class Settlement : IDrawable
{
    private Faction _faction;
    private string _name;
    private int _population;
    private int _wealth;
    private Position _position;

    public Position Position {  get { return _position; } }
    public int Wealth { get; set; }
    public Faction Faction { get; set; }

    public DrawPriority Priority => DrawPriority.SETTLEMENTS;

    public Settlement(Faction faction, Position position, string name, int population = 100, int wealth = 100)
    {
        _faction = faction;
        _position = position;
        _name = name;
        _population = population;
        _wealth = wealth;
    }

    public void Draw(RenderBuffer renderBuffer, int x, int y)
    {

        int topX = x - (Constants.DRAW_WIDTH / 2);
        int topY = y - (Constants.DRAW_HEIGHT / 2);
        // Checks if inside the cameras view
        if (topX < _position.x && _position.x < topX + Constants.DRAW_WIDTH - 1 &&
           topY < _position.y && _position.y < topY + Constants.DRAW_HEIGHT)
        {
            RGB foregroundColor = _faction.GetFactionColor();
            renderBuffer[_position.x - topX, (_position.y - topY)] = new Pixel
            {
                Character = 'X',
                textRGB = foregroundColor,
            };
            // Checks how much of the name can fit on screen, writes the fitting part
            int maxTextLength = Constants.DRAW_WIDTH - (_position.x - topX) - 3;
            if (maxTextLength > _name.Length) maxTextLength = _name.Length;
            for (int i = 0; i < maxTextLength; i++)
            {
                renderBuffer[_position.x - topX + 2 + i, (_position.y - topY)] = new Pixel
                {
                    Character = _name[i],
                    textRGB = foregroundColor,
                    bgRGB = new RGB(0,0,0)
                };
            }
        } 
    }
}
