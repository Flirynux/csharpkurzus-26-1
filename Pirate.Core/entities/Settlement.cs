using System;
using System.Collections.Generic;
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

    public void Draw(int x, int y)
    {

        int topX = x - (Constants.DRAW_WIDTH / 2);
        int topY = y - (Constants.DRAW_HEIGHT / 2);
        if (topX < _position.x && _position.x < topX + Constants.DRAW_WIDTH &&
           topY < _position.y && _position.y < topY + Constants.DRAW_HEIGHT) 
        {
            Console.SetCursorPosition(_position.x-topX, (_position.y-topY)/2);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write('X');
        }
    }
}
