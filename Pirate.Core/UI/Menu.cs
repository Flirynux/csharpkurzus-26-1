using Pirate.Core.entities;
using Pirate.Core.UI.Graphics;
using Pirate.Core.Utils;

namespace Pirate.Core.UI;

internal class Menu : IDrawable
{
    public DrawPriority Priority => DrawPriority.MENU;
    List<char[]> _menuElements = new List<char[]>();
    public int selectedIndex = 0;
    public bool active = false;

    public Menu(List<char[]> menuElements)
    {
        _menuElements = menuElements;
    }

    public Menu(string[] menuElements)
    {
        for (int i = 0; i < menuElements.Length; i++)
        {
            _menuElements.Add(menuElements[i].ToCharArray());
        }
    }

    public EngineTask HandleInput(ConsoleKey key)
    {
        if (key == ConsoleKey.Escape)
        {
            active = !active;
        }
        if (!active) return EngineTask.PASS;
        switch (key)
        {
            case ConsoleKey.W:
                selectedIndex--;
                MoveSelected();
                break;
            case ConsoleKey.S:
                selectedIndex++;
                MoveSelected();
                break;
            case ConsoleKey.Enter:
                active = !active;
                return HandleSelected();
            default:
                break;

        }
        return EngineTask.HALT;
    }

    private EngineTask HandleSelected()
    {
        switch(selectedIndex)
        {
            case 0:
                return EngineTask.PASS;
            case 1:
                return EngineTask.SAVE;
            case 2:
                return EngineTask.LOAD;
            case 3:
                return EngineTask.EXIT;
            default:
                break;
        }
        return EngineTask.PASS;
    }

    private void MoveSelected()
    {
        selectedIndex = Math.Clamp(selectedIndex, 0, _menuElements.Count-1);
    }

    public void Draw(RenderBuffer renderBuffer, int camX, int camY)
    {
        if (!active || _menuElements.Count == 0) return;


        int maxTextLen = _menuElements.Max(obj => obj.Length);
        int menuWidth = maxTextLen + 5;

        int menuHeight = (_menuElements.Count * 2) + 3;

        int startX = Constants.DRAW_WIDTH / 2 - menuWidth / 2;
        int startY = Constants.DRAW_HEIGHT / 2 - menuHeight / 2;

        for (int row = 0; row < menuHeight; row++)
        {
            for (int col = 0; col < menuWidth; col++)
            {
                char c = ' ';

                // Corners
                if (row == 0 && col == 0) c = '\u2554';
                else if (row == 0 && col == menuWidth - 1) c = '\u2557';
                else if (row == menuHeight - 1 && col == 0) c = '\u255A';
                else if (row == menuHeight - 1 && col == menuWidth - 1) c = '\u255D';

                // Borders
                else if (row == 0 || row == menuHeight - 1) c = '\u2550'; 
                else if (col == 0 || col == menuWidth - 1) c = '\u2551'; 

                // Content (every other for rendering)
                else if (row % 2 == 0 && row >= 2 && row < menuHeight - 1)
                {
                    int itemIndex = (row - 2) / 2;

                    if (itemIndex < _menuElements.Count)
                    {
                        char[] item = _menuElements[itemIndex];

                        // Cursor
                        if (col == 1)
                        {
                            c = (itemIndex == selectedIndex) ? '>' : ' ';
                        }
                        // Text
                        else if (col >= 3 && col - 3 < item.Length)
                        {
                            c = item[col - 3];
                        }
                    }
                }
                if (startX + col >= 0 && startX + col < renderBuffer.Width &&
                    startY + row >= 0 && startY + row < renderBuffer.Height)
                {
                    renderBuffer[startX + col, startY + row] = new Pixel
                    {
                        Character = c,
                        textRGB = new RGB(255, 255, 255),
                        bgRGB = new RGB(0, 0, 0)
                    };
                }
            }
        }
    }
}

