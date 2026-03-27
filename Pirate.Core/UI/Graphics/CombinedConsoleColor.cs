namespace Pirate.Core.UI.Graphics;

internal class CombinedConsoleColor
{
    private readonly ConsoleColor _foreground;
    private readonly ConsoleColor _background;

    public CombinedConsoleColor(ConsoleColor foreground, ConsoleColor background)
    {
        _foreground = foreground;
        _background = background;
    }

    public void Apply() 
    { 
        Console.ForegroundColor = _foreground;
        Console.BackgroundColor = _background;
    }

    public override bool Equals(object? obj)
    {
        return obj is CombinedConsoleColor color &&
               _foreground == color._foreground &&
               _background == color._background;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_foreground, _background);
    }
}