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

    public override string ToString()
    {
        return "x: " + x + "y: " + y;
    }
}
