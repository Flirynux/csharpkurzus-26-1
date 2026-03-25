using System;
using System.Collections.Generic;
using System.Text;

using Pirate.Core.entities;
using Pirate.Core.UI.Graphics;

namespace Pirate.Core;

internal class Engine
{
    Camera _camera;
    Player _player;
    List<Faction> _factions;

    public Engine(Camera camera, Player player, List<Faction> factions)
    {
        _camera = camera;
        _player = player;
        _factions = factions;
    }

    public void Init()
    {

    }

    public void Update()
    {

    }

    public void Save()
    {

    }

    public void Load()
    {

    }

    public void Exit()
    {

    }
}
