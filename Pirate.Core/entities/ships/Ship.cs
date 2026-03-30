

using System.Numerics;

using Pirate.Core.Utils;

namespace Pirate.Core.entities.ships;

internal abstract class Ship
{
    public string _name;
    public float _speed = 0;
    public float _maxSpeed;
    public int _crew;
    public int _crewCapacity = 25;
    public int _cannons;
    public int _cannonsCapacity = 6;
    public int _cargoHoldCapacity = 10;
    public Vector2 _position;


    public Modifier[] _modifiers;
    public Faction _faction;
    private Navmap _navmap;

    public Ship(Faction faction,Navmap navmap, string name, Vector2 position)
    {
        _faction = faction;
        _navmap = navmap;
        _name = name;
        _maxSpeed = 10;
        _crew = 8;
        _cannons = 4;
        _modifiers = new Modifier[8];
        faction.AddShip(this);
        _position = position;
    }
    public void Move(Vector2 position)
    {
        if (_navmap.IsSailable(position))
        {
            _position = position;
        }
    }

    private void movement(float deltaX, float deltaY)
    {
        float fixedX = Math.Clamp(deltaX + _position.X, 0.0f, Constants.MAP_WIDTH);
        float fixedY = Math.Clamp(deltaY + _position.Y, 0.0f, Constants.MAP_HEIGHT);
        _position = new Vector2(fixedX, fixedY);
    }
    public abstract void ApplyModifiers();
    public abstract void AddModifier(Modifier modifier);

    public override string ToString()
    {
        string res = "";
        res += _faction.Name + ": ";
        res += _name + "\n";
        res += _position.ToString();

        return res;
    }
}
