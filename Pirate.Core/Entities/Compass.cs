using System.Numerics;

using Pirate.Core.UI.Graphics;
using Pirate.Core.Utils;

using static Pirate.Core.UI.Graphics.RenderBuffer;

namespace Pirate.Core.entities;

public class Compass : IDrawable
{
    private float _direction; //radians
    private const int _radius = 4;
    private string[] _compassBase = new string[_radius * 2 + 1];

    public Vector2 Vector
    {
        get
        {
            Vector2 res = new Vector2((float)Math.Cos(_direction), (float)Math.Sin(_direction));
            return Vector2.Normalize(res);
        }
    }

    public float Direction
    {
        get { return _direction; }
        set { _direction = value; }
    }

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

    public void Draw(RenderBuffer renderBuffer, int x = 0, int y = 0)
    {
        // Background
        int startY = Constants.DRAW_HEIGHT - (_radius * 2 + 1) + y;
        for (int i = 0; i < _compassBase.Length; i++)
        {
            for (int j = 0; j < _compassBase[i].Length; j++)
            {
                char c = _compassBase[i][j];
                renderBuffer[x + j, startY + i] = new Pixel
                {
                    Character = c,
                    // If it's a Cardinal direction, make it white, otherwise black
                    textRGB = (c == '\u2580') ? new RGB(0, 0, 0) : new RGB(255, 255, 255)
                };
            }
        }

        // Needle
        // Used some AI for logic
        float endX = (float)Math.Cos(_direction) * _radius * 2; // 2X because looks better
        float endY = (float)Math.Sin(_direction) * _radius;

        float centerX = _radius * 2;
        float centerY = _radius;

        float steps = Math.Max(Math.Abs(endX), Math.Abs(endY));

        for (int i = 0; i <= steps; i++)
        {
            float pct = i / steps;
            int plotX = (int)Math.Round(centerX + endX * pct);
            int plotY = (int)Math.Round(centerY + endY * pct);

            // Safety check: Only draw the needle if it's over the background blocks.
            // This prevents the red line from overwriting your N, S, E, W letters!
            if (renderBuffer[x + plotX, startY + plotY].Character == '\u2580')
            {
                renderBuffer[x + plotX, startY + plotY] = new Pixel
                {
                    Character = '\u2580',
                    textRGB = new RGB(255, 0, 0)
                };
            }
        }
    }

    // Compass background
    private void InitCompassBase()
    {
        string radSpace = new string('\u2580', _radius * 2);

        for (int i = 0; i < _radius * 2 + 1; i++)
        {
            string temp = "";
            switch (i)
            {
                case 0:
                    temp += radSpace + "N" + radSpace;
                    break;
                case _radius:
                    temp += "W" + new string('\u2580', 2 * 2 * _radius - 1) + "E";
                    break;
                case _radius * 2:
                    temp += radSpace + "S" + radSpace;
                    break;
                default:
                    temp += radSpace + '\u2580' + radSpace;
                    break;
            }
            _compassBase[i] = temp;
        }
    }
}