using Pirate.Core.Utils;

namespace Pirate.Core.UI.Graphics;

public interface IDrawable
{
    public DrawPriority Priority { get; }
    public void Draw(RenderBuffer renderBuffer, int x, int y);

    public void Draw(RenderBuffer renderBuffer, Position position)
    {
        Draw(renderBuffer, position.x, position.y);
    }
}
