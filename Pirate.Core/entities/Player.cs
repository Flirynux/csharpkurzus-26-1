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
    private Compass _compass = new Compass();

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
        movement(0, 0);
    }

    public override string ToString()
    {
        return _name + " " + _flagship.ToString() + " " +_faction.Wealth;
    }

    public void Draw(int x = 0, int y = 0)
    {
        Console.SetCursorPosition(Constants.DRAW_WIDTH/2, Constants.DRAW_HEIGHT / (2*Constants.VERTICAL_SCALE));
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write(_symbol);
        Console.ResetColor();
        _compass.Draw();
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
                _compass.TurnDeg(5);
                break;
            case ConsoleKey.A:
                movement(-1,0);
                _compass.TurnDeg(-5);
                break;
            case ConsoleKey.Enter:
                Console.WriteLine(Position.ToString());
                break;
            default:
                break;

        }
    }

    private void movement(float deltaX, float deltaY) 
    {
        float fixedX = Math.Clamp(deltaX + _flagship._position.X, Constants.PLAYER_MOVEMENT_BORDER_LEFT, Constants.PLAYER_MOVEMENT_BORDER_RIGHT);
        float fixedY = Math.Clamp(deltaY + _flagship._position.Y, Constants.PLAYER_MOVEMENT_BORDER_TOP, Constants.PLAYER_MOVEMENT_BORDER_BOTTOM);
        _flagship.Move(new Vector2(fixedX, fixedY));
    }
}

public class Compass : IDrawable
{
    private float _direction; //radians
    private const int _radius = 3;
    private string[] compassBase = new string[_radius * 2 + 1];

    public Compass(float directionInRadians = (float)(Math.PI/-2))
    {
        _direction = directionInRadians;
        InitCompassBase();
    }

    public DrawPriority Priority => DrawPriority.PLAYER;

    public void TurnRad(float rad)
    {
        _direction += rad;
        _direction %= (float)(2 * Math.PI);
    }

    public void TurnDeg(float degree)
    {
        float rad = (float)Math.PI * (degree/180);
        TurnRad(rad);
    }

    public void Draw(int x = 0, int y = 0)
    {
        //Draw background
        int startY = (Constants.DRAW_HEIGHT / Constants.VERTICAL_SCALE) - (_radius * 2 + 1) + y;
        for (int i = 0; i < compassBase.Length; i++)
        {
            Console.SetCursorPosition(x, startY + i);
            Console.Write(compassBase[i]);
        }

        float endX = (float)Math.Cos(_direction) * _radius * 2; // 2X for character scale compensation
        float endY = (float)Math.Sin(_direction) * _radius;

        float centerX = _radius * 2;
        float centerY = _radius;

        float steps = Math.Max(Math.Abs(endX), Math.Abs(endY));

        for (int i = 0; i <= steps; i++)
        {
            float pct = i / steps;
            int plotX = (int)Math.Round(centerX + endX * pct);
            int plotY = (int)Math.Round(centerY + endY * pct);

            Console.SetCursorPosition(x + plotX, startY + plotY);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write(" ");
        }

        Console.ResetColor();
    }


    private void InitCompassBase()
    {
        string radSpace = "";
        for (int i = 0; i < _radius*2; i++)
        {
            radSpace += " ";
        }
        for (int i = 0; i < _radius * 2 + 1; i++)
        {
            string temp = "";
            switch (i)
            {
                case 0:
                    temp += radSpace;
                    temp += "N";
                    temp += radSpace;
                    break;
                case _radius:
                    temp += "W";
                    for (int j = 0; j < 2 * 2 * _radius -1; j++)
                    {
                        temp += " ";
                    }
                    temp += "E";
                    break;
                case _radius * 2:
                    temp += radSpace;
                    temp += "S";
                    temp += radSpace;
                    break;
                default:
                    temp += radSpace; 
                    temp += radSpace;
                    temp += " ";
                    break;
            }
            compassBase[i] = temp;
        }
    }
}