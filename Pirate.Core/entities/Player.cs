using System.Numerics;

using Pirate.Core.entities.ships;
using Pirate.Core.entities.ships.types;
using Pirate.Core.UI.Graphics;
using Pirate.Core.UI.Input;
using Pirate.Core.Utils;

namespace Pirate.Core.entities;

internal class Player : IDrawable, IInputHandler
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
            new Vector2(140.0f, 80.0f));
        Ship = _flagship;
        movement(0, 0);
    }

    public override string ToString()
    {
        return _name + " " + _flagship.ToString() + " " +_faction.Wealth;
    }

    public void Draw(int x = 0, int y = 0)
    {
        Console.SetCursorPosition(Constants.DRAW_WIDTH/2, Constants.DRAW_HEIGHT / 4);
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write(_symbol);
        Console.ResetColor();
    }


    public void HandleInput(ConsoleKey key)
    {
        switch (key) 
        {
            case ConsoleKey.W:
                movement(0, -1);
                break;
            case ConsoleKey.S:
                movement(0, 1);
                break;
                
            case ConsoleKey.D:
                movement(1,0);
                break;
            case ConsoleKey.A:
                movement(-1,0);
                break;
            default:
                break;

        }
    }

    private void movement(float deltaX, float deltaY) 
    {
        float fixedX = Math.Clamp(deltaX + _flagship._position.X, Constants.PLAYER_MOVEMENT_BORDER_LEFT, Constants.PLAYER_MOVEMENT_BORDER_RIGHT);
        float fixedY = Math.Clamp(deltaY + _flagship._position.Y, Constants.PLAYER_MOVEMENT_BORDER_TOP, Constants.PLAYER_MOVEMENT_BORDER_BOTTOM);
        _flagship._position = new Vector2(fixedX, fixedY);
    }
}
