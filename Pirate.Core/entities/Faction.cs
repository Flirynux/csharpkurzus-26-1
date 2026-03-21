using Pirate.Core.entities.ships;

namespace Pirate.Core.entities;

internal class Faction
{
    private readonly FactionType _type;
    private int _wealth;
    private List<Ship> _ships;
    private readonly string _name;

    public int Wealth { get { return _wealth; } }

    public string Name {  get { return _name; } }

    public Faction(FactionType type, string name, int wealth = 10000)
    {
        _type = type;
        _name = name;
        _wealth = wealth;
        _ships = new List<Ship>();
    }
    public void AddShip(Ship ship)
    {
        _ships.Add(ship);
    }

    public void RemoveShip(Ship ship)
    {
        _ships.Remove(ship);
    }

    public override string ToString()
    {
        string res = string.Empty;
        res += _name + " " + _wealth + "\n";
        foreach (Ship ship in _ships)
        {
            res += "\t" + ship.ToString() + "\n";
        }
        return res;
    }
}
