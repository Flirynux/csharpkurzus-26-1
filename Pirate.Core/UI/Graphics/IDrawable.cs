using Pirate.Core.entities;

namespace Pirate.Core.UI.Graphics;

public interface IDrawable
{
    public DrawPriority Priority { get; }
    public void Draw(int x, int y);

    public void Draw(Position position)
    {
        Draw(position.x, position.y);
    }
}
