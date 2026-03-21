using System.Numerics;

using Pirate.Core.entities.ships;
using Pirate.Core.entities.ships.types;

namespace Pirate.Core.entities;

internal class Player
{
    private readonly string _name;
    private Ship _flagship;
    private readonly Faction _faction;

    public Ship Ship { get;
        set
        {
            if (value._faction == _faction)
            {
                _flagship = value;
            }
        } }

    public Player(string name)
    {
        _name = name;
        _faction = new Faction(FactionType.PLAYER, "Player", 500);
        _flagship = new Sloop(_faction,
            "The Foul Oyster", 
            new Vector2(0.0f, 0.0f));
    }

    public override string ToString()
    {
        return _name + " " + _flagship.ToString() + " " +_faction.Wealth;
    }
}
