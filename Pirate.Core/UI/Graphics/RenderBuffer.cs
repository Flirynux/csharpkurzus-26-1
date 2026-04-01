using Pirate.Core.Utils;

namespace Pirate.Core.UI.Graphics;

public class RenderBuffer
{
    public Pixel[,] Grid { get; }
    public int Width { get; }
    public int Height { get; }

    public RenderBuffer()
    {
        Width = Constants.DRAW_WIDTH;
        Height = Constants.DRAW_HEIGHT;
        Grid = new Pixel[Constants.DRAW_WIDTH, Constants.DRAW_HEIGHT];
    }

    public Pixel this[int x,int y] 
    { 
        get => Grid[x,y]; 
        set => Grid[x,y] = value; 
    }
}

public struct Pixel
{
    public char Character;
    public RGB textRGB;
    public RGB bgRGB;
}

public struct RGB
{
    public byte R;
    public byte G;
    public byte B;

    public RGB(byte r,byte g,byte b)
    {
        R = r; G = g; B = b;
    }
}