using System.Numerics;

using Pirate.Core.entities.ships;

namespace Pirate.Core.entities.ships.types;

internal class Sloop : Ship
{
    public Sloop(Faction faction, Navmap navmap, string name, Vector2 position, Random random) : base(
        faction,
        navmap,
        random,
        position,
        name)
    {
        _maxSpeed = 8;
        _crew = 6;
        _cannons = 2;
    }
    

}
