using System.Numerics;

using Pirate.Core.entities.ships;
using Pirate.Core.entities.ships.types;
using Pirate.Core.UI;

namespace Pirate.Core.entities;

internal class Player : IDrawable
{
    private readonly string _name;
    private Ship _flagship;
    private readonly Faction _faction;

    private readonly char _symbol = '@';

    public DrawPriority Priority => DrawPriority.PLAYER;

    public Ship Ship { get;
        set
        {
            if (value._faction == _faction)
            {
                field = value;
            }
        } }

    public Position Position
    {
        get {
            Position result = new (
                (int)_flagship._position.X,
                (int)_flagship._position.Y
            );
            
            return result;
            }
    }

    public Player(string name)
    {
        _name = name;
        _faction = new Faction(FactionType.PLAYER, "Player", 500);
        _flagship = new Sloop(_faction,
            "The Foul Oyster", 
            new Vector2(160.0f, 60.0f));
        Ship = _flagship;
    }

    public override string ToString()
    {
        return _name + " " + _flagship.ToString() + " " +_faction.Wealth;
    }

    public void Draw(int x = 0, int y = 0)
    {
        Console.SetCursorPosition(Constants.DRAW_WIDTH/2, Constants.DRAW_HEIGHT / 2);
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write(_symbol);
        Console.ResetColor();
    }
}
