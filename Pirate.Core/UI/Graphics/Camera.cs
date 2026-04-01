using System.Reflection.Metadata;
using System.Text;

using Pirate.Core.entities;

namespace Pirate.Core.UI.Graphics;

internal class Camera
{
    List<IDrawable> _drawables = new List<IDrawable>();
    //Camera anchored to player
    Player _player;
    RenderBuffer _buffer = new RenderBuffer();
    private readonly StringBuilder _builder = new StringBuilder();
    public Camera(Player player) 
    {
        _player = player;
        AddObject(player);
    }
    public Camera(Player player, List<IDrawable> objects)
    {
        _player = player;
        _drawables = objects;
    }
    public void AddObject(IDrawable obj)
    {
        _drawables.Add(obj);
        SortObjects();
    }

    public void AddObject(List<IDrawable> objects)
    {
        foreach (var obj in objects)
        {
            AddObject(obj);
        }
    }

    public void RemoveObject(IDrawable obj)
    {
        _drawables.Remove(obj);
        SortObjects();
    }

    private void SortObjects()
    {
        _drawables = _drawables.OrderBy(item => item.Priority).ToList();
    }

    // Used some AI for ANSI
    public void Render()
    {
        WriteRenderBuffer();

        _builder.Clear();
        _builder.Append("\x1b[H"); // Move cursor to top-left

        for (int y = 0; y < _buffer.Height; y += 2)
        {
            for (int x = 0; x < _buffer.Width; x++)
            {
                Pixel top = _buffer[x, y];
                Pixel bottom = _buffer[x, y + 1];

                // Assumption: normal map tile
                RGB fg = top.textRGB;
                RGB bg = bottom.textRGB;
                char displayChar = top.Character == '\0' ? ' ' : top.Character;

                // Check if pixel contains entity/text  (not half-block)
                bool topHasText = top.Character != '\u2580' && top.Character != '\0';
                bool bottomHasText = bottom.Character != '\u2580' && bottom.Character != '\0';

                if (topHasText)
                {
                    displayChar = top.Character;
                    fg = top.textRGB;
                    bg = new RGB(0, 0, 0); // Force black background for readability
                }
                else if (bottomHasText)
                {
                    displayChar = bottom.Character;
                    fg = bottom.textRGB;   // Grab the text color from the bottom pixel
                    bg = new RGB(0, 0, 0); // Force black background for readability
                }

                // Apply colors and draw the character
                _builder.Append($"\x1b[38;2;{fg.R};{fg.G};{fg.B}m");
                _builder.Append($"\x1b[48;2;{bg.R};{bg.G};{bg.B}m");
                _builder.Append(displayChar);
            }
            _builder.Append("\x1b[0m\n");
        }

        Console.Write(_builder.ToString());
    }

    private void WriteRenderBuffer()
    {
        foreach (var drawable in _drawables)
        {
            drawable.Draw(_buffer, _player.Position);
        }
    }
}
