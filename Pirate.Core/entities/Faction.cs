using Pirate.Core.entities.ships;
using Pirate.Core.UI.Graphics;

namespace Pirate.Core.entities;

internal class Faction
{
    public readonly FactionType _type;
    private int _wealth;
    private List<Ship> _ships;
    private List<Settlement> _settlements;
    private readonly string _name;

    private List<Faction> _allies = new List<Faction>();
    private List<Faction> _enemies = new List<Faction>();

    private static Random _rnd;

    public List<Faction> Allies {  get { return _allies; } }
    public List<Faction> Enemies {  get { return _enemies; } }
    public List<Settlement> Settlements {  get { return _settlements; } }
    public List<Ship> Ships { get { return _ships; } }
    public int Wealth { get { return _wealth; } }
    public string Name {  get { return _name; } }

    public Faction(FactionType type, string name, Random random, int wealth = 10000)
    {
        _type = type;
        _name = name;
        _wealth = wealth;
        _ships = new List<Ship>();
        _settlements = new List<Settlement>();
        _rnd = random;
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

    public void Alliance(Faction ally)
    {
        AddEnemy(ally);
        ally.AddAlly(ally);
    }

    protected void AddAlly(Faction ally)
    {
        _allies.Add(ally);
    }

    public void War(Faction enemy)
    {
        AddEnemy(enemy);
        enemy.AddEnemy(this);
    }

    protected void AddEnemy(Faction enemy)
    {
        _enemies.Add(enemy);
    }

    public void SetNeutralRelation(Faction other)
    {
        NeutralRelations(other);
        other.NeutralRelations(this);
    }

    protected void NeutralRelations(Faction other)
    {
        _allies.Remove(other);
        _enemies.Remove(other);
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

    // Returns RGB values for the given faction
    // single source of truth for drawing
    public RGB GetFactionColor()
    {
        switch (_type)
        {
            case FactionType.ENGLISH:
                return new RGB { R = 255, G = 0, B = 0 };
            case FactionType.DUTCH:
                return new RGB { R = 0, G = 127, B = 127 };
            case FactionType.FRENCH:
                return new RGB { R = 0, G = 0, B = 255 };
            case FactionType.SPANISH:
                return new RGB { R = 0, G = 255, B = 255 };
            default:
                return new RGB { R = 127, G = 127, B = 127 };
        }
    }
}
