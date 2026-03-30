using System.Numerics;
using System.Xml.Linq;

namespace Pirate.Core.entities;

public struct Position
{
    public int x;
    public int y;

    public Position(int x, int y)
    {
        this.x = x;
        this.y = y;
    }


    public static implicit operator Vector2(Position pos)
    {
        return new Vector2(pos.x, pos.y);
    }
    public override string ToString()
    {
        return "x: " + x + "y: " + y;
    }

    public override int GetHashCode()
    {
        return x.GetHashCode() ^ y.GetHashCode();
    }
}
