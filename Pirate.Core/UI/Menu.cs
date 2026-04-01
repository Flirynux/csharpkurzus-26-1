using Pirate.Core.UI.Graphics;

namespace Pirate.Core.UI;

//TODO handle menu options
internal class Menu : IDrawable
{
    public DrawPriority Priority => DrawPriority.MENU;
    List<char[]> _menuElements = new List<char[]>(8);
    int selectedIndex = 0;
    bool active = false;

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

    
    public void Draw(RenderBuffer renderBuffer, int x, int y)
    {
        int menuWidth = _menuElements.Max(obj => obj.Length) + 2;
        for (int i = -1; i < _menuElements.Count+1; i++)
        {
            if (i == -1) {
                renderBuffer[x, y + i] = new Pixel
                {
                    Character = '\u2554',
                    textRGB = new RGB(255,255,255),
                    bgRGB = new RGB(0,0,0),

                };
                for (int j = 0; j < menuWidth; j++)
                {
                    renderBuffer[x + j, y + i] = new Pixel
                    {
                        Character = '\u2550',
                        textRGB = new RGB(255, 255, 255),
                        bgRGB = new RGB(0, 0, 0),

                    };
                }
                renderBuffer[x, y + i] = new Pixel
                {
                    Character = '\u2557',
                    textRGB = new RGB(255, 255, 255),
                    bgRGB = new RGB(0, 0, 0),

                };

            }
            
            else {
                if (i == _menuElements.Count)
                {
                    renderBuffer[x, y + i] = new Pixel
                    {
                        Character = '\u255A',
                        textRGB = new RGB(255, 255, 255),
                        bgRGB = new RGB(0, 0, 0),

                    };
                    for (int j = 0; j < menuWidth; j++)
                    {
                        renderBuffer[x, y + i + j] = new Pixel
                        {
                            Character = '\u2550',
                            textRGB = new RGB(255, 255, 255),
                            bgRGB = new RGB(0, 0, 0),

                        };
                    }
                    renderBuffer[x, y + i] = new Pixel
                    {
                        Character = '\u255D',
                        textRGB = new RGB(255, 255, 255),
                        bgRGB = new RGB(0, 0, 0),

                    };
                }
                else 
                {
                    renderBuffer[x, y + i] = new Pixel
                    {
                        Character = '\u2551',
                        textRGB = new RGB(255, 255, 255),
                        bgRGB = new RGB(0, 0, 0),

                    };
                    int spaceCount = menuWidth - _menuElements[i].Length;
                    if (i == selectedIndex)
                    {
                        renderBuffer[x, y + i] = new Pixel
                        {
                            Character = '>',
                            textRGB = new RGB(255, 255, 255),
                            bgRGB = new RGB(0, 0, 0),

                        };
                    }
                    for (int j = 0; j < spaceCount-2; j++)
                    {
                    }
                    renderBuffer[x, y + i] = new Pixel
                    {
                        Character = '\u2551',
                        textRGB = new RGB(255, 255, 255),
                        bgRGB = new RGB(0, 0, 0),

                    };
                }
            }
        }
    }
}
