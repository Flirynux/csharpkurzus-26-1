namespace Pirate.Core.UI;

internal class Menu : IDrawable
{
    public DrawPriority Priority => DrawPriority.MENU;
    List<char[]> _menuElements = new List<char[]>(8);

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

    public void Draw(int x, int y)
    {
        int menuWidth = _menuElements.Max(obj => obj.Length) + 2;
        for (int i = -1; i < _menuElements.Count+2; i++)
        {
            Console.SetCursorPosition(x, y + i);
            if (i == 0) {
                Console.Write("\u2554");
                char[] chars = new char[menuWidth];
                for (int j = 0; j < chars.Length; j++)
                {
                    chars[j] = '\u2550';
                }
                Console.Write(chars);
                Console.Write("\u2557");

            }
            else if (i == _menuElements.Count -1)
            {
                Console.Write("\u255A");
                char[] chars = new char[menuWidth];
                for (int j = 0; j < chars.Length; j++)
                {
                    chars[j] = '\u2550';
                }
                Console.Write(chars);
                Console.Write("\u255D");
            }
            else 
            {
                Console.Write("\u2551 ");
                Console.Write(_menuElements[i]);
                Console.Write(" \u2551");
            }
        }
    }
}