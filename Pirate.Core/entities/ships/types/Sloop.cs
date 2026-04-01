using System.Numerics;

using Pirate.Core.entities.ships;

namespace Pirate.Core.entities.ships.types;

internal class Sloop : Ship
{
    public Sloop(Faction faction, Navmap navmap, string name, Vector2 position, Random random) : base(
        faction,
        navmap,
        name,
        position,
        random)
    {
        _maxSpeed = 8;
        _crew = 6;
        _cannons = 2;
        _modifiers = new Modifier[8];
    }
    public override void AddModifier(Modifier modifier)
    {
        throw new NotImplementedException();
    }

    public override void ApplyModifiers()
    {
        throw new NotImplementedException();
    }

}
