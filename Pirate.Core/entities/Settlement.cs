using System;
using System.Collections.Generic;
using System.Text;

namespace Pirate.Core.entities;

internal class Settlement
{
    private Faction _faction;
    private int _population;
    private int _wealth;

    public int Wealth { get; set; }
    public Faction Faction { get; set; }
    public Settlement(Faction faction, int population, int wealth = 100)
    {
        _faction = faction;
        _population = population;
        _wealth = wealth;
    }
}
