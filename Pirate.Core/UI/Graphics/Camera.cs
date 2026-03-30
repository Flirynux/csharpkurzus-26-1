using Pirate.Core.entities;

namespace Pirate.Core.UI.Graphics;

internal class Camera
{
    List<IDrawable> _drawables = new List<IDrawable>();
    //Camera anchored to player
    Player _player;
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

    public void Render()
    {
        Position pos = _player.Position;

        var sorted = _drawables.OrderBy(obj => obj.Priority);
        foreach (var item in sorted)
        {
            item.Draw(pos);
        }
    }
}
