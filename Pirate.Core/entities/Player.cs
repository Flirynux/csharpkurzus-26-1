using System.Numerics;

using Pirate.Core.entities.ships;
using Pirate.Core.entities.ships.types;
using Pirate.Core.UI.Graphics;
using Pirate.Core.UI.Input;
using Pirate.Core.Utils;

namespace Pirate.Core.entities;

internal class Player : IDrawable
{
    private readonly string _name;
    public Ship _flagship;
    private readonly Faction _faction;
    public Compass _compass = new Compass();
    public bool is_menu_active = false;


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


    public Player(Navmap navmap, Random random, string name)
    {
        _name = name;
        _faction = new Faction(FactionType.PLAYER, "Player", random, 500);
        _flagship = new Sloop(_faction,
            navmap,
            "The Foul Oyster", 
            new Vector2(140.0f, 80.0f),
            random);
        Ship = _flagship;
        _flagship.Speed = 0;
        moveInbounds();
    }

    public override string ToString()
    {
        return _name + " " + _flagship.ToString() + " " +_faction.Wealth;
    }

    public void Draw(RenderBuffer renderBuffer, int x = 0, int y = 0)
    {
        renderBuffer[Constants.DRAW_WIDTH / 2, Constants.DRAW_HEIGHT / 2] =
            new Pixel
            {
                Character = '@',
                textRGB = new RGB { R = 255, G = 0, B = 255 },
            };
        _compass.Draw(renderBuffer, 0, 0);
    }

    public void Update(float deltaTime)
    {
        Vector2 newPos = _flagship._position + _compass.Vector;
        Position intPos = new Position((int)Math.Round(newPos.X), (int)Math.Round(newPos.Y));
        // Boundary check
        if(Constants.PLAYER_MOVEMENT_BORDER_LEFT < intPos.x && intPos.x < Constants.PLAYER_MOVEMENT_BORDER_RIGHT
            && Constants.PLAYER_MOVEMENT_BORDER_TOP < intPos.y &&intPos.y < Constants.PLAYER_MOVEMENT_BORDER_BOTTOM)
        {
            _flagship.Move(_compass.Vector, deltaTime);
        }
    }
    public void HandleInput(ConsoleKey key, float deltaTime)
    {
        if (is_menu_active) return;

        switch (key) 
        {
            case ConsoleKey.W:
                _flagship.Speed += 0.5f;
                break;
            case ConsoleKey.S:
                _flagship.Speed -= 0.5f;
                break;
                
            case ConsoleKey.D:
                _compass.TurnDeg(10);
                break;
            case ConsoleKey.A:
                _compass.TurnDeg(-10);
                break;
            case ConsoleKey.Enter:
                Console.WriteLine(Position.ToString());
                break;
            default:
                break;

        }
    }

    private void moveInbounds() 
    {
        float fixedX = Math.Clamp(_flagship._position.X, Constants.PLAYER_MOVEMENT_BORDER_LEFT, Constants.PLAYER_MOVEMENT_BORDER_RIGHT);
        float fixedY = Math.Clamp(_flagship._position.Y, Constants.PLAYER_MOVEMENT_BORDER_TOP, Constants.PLAYER_MOVEMENT_BORDER_BOTTOM);
        _flagship._position =new Vector2(fixedX, fixedY);
    }

    public void LoadState(PlayerSaveData data)
    {
        // Overwrite the hardcoded defaults with saved values
        _flagship.Speed = data.Speed;
        _flagship._position = new Vector2(data.X, data.Y);
        _compass.Direction = data.Angle; // You may need to add SetAngle to Compass
    }
}

public class PlayerSaveData
{
    // Mutable Stats (Change during play)
    public float Wealth { get; set; }
    public float Speed { get; set; }

    // Position/Orientation
    public float X { get; set; }
    public float Y { get; set; }
    public float Angle { get; set; } // The compass direction
}