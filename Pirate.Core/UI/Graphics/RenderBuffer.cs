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

    public Pixel(char character, RGB text, RGB bg)
    {
        Character = character;
        textRGB = text;
        bgRGB = bg;
    }

    public Pixel(char character, RGB text) : 
        this(character,text,new RGB(0,0,0)) { }
    public Pixel(char character) : 
        this(character, new RGB(0,0,0), new RGB(0,0,0)) { }
}

public readonly struct RGB
{
    public byte R { get; }
    public byte G { get; }
    public byte B { get; }

    public RGB(byte r, byte g, byte b)
    {
        R = r; G = g; B = b;
    }
}