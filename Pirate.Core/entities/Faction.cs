using Pirate.Core.entities.ships;

namespace Pirate.Core.entities;

internal class Faction
{
    public readonly FactionType _type;
    private int _wealth;
    private List<Ship> _ships;
    private List<Settlement> _settlements;
    private readonly string _name;

    public int Wealth { get { return _wealth; } }

    public string Name {  get { return _name; } }

    public Faction(FactionType type, string name, int wealth = 10000)
    {
        _type = type;
        _name = name;
        _wealth = wealth;
        _ships = new List<Ship>();
        _settlements = new List<Settlement>();
    }
    public void AddShip(Ship ship)
    {
        _ships.Add(ship);
    }

    public void RemoveShip(Ship ship)
    {
        _ships.Remove(ship);
    }

    public void AddSettlement(Settlement settlement)
    {
        _settlements.Add(settlement);
    }

    public void RemoveSettlement(Settlement settlement)
    {
        _settlements.Remove(settlement);
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
