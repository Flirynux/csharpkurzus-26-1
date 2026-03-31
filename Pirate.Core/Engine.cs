using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Text;

using Pirate.Core.entities;
using Pirate.Core.entities.ships.types;
using Pirate.Core.UI.Graphics;

namespace Pirate.Core;

internal class Engine
{
    Camera _camera;
    Player _player;
    List<Faction> _factions;
    static Random s_random = new Random(42);
    static Navmap s_navmap;
    static Stopwatch timer = new Stopwatch();

    EngineState _state = EngineState.STOPPED; 

    public EngineState State {  get { return _state; } }
    public static Random Random {  get { return s_random; } }

    public Engine()
    {

        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "map_binary.txt");
        Map map = new Map(filePath);
        Navmap navmap = new Navmap(filePath);
        _player = new Player(navmap, s_random, "Playa");
        _camera = new Camera(_player);
        _camera.AddObject(map);
        s_navmap = navmap;
        _factions = new List<Faction>(4);
    }

    public void Init()
    {
        initFactions();
        initSettlements();
        initShips();
    }

    private void initFactions()
    {
        Faction brits = new Faction(FactionType.ENGLISH, "English",s_random);
        Faction spanish = new Faction(FactionType.SPANISH, "Spanish", s_random);
        Faction dutch = new Faction(FactionType.DUTCH, "Dutch", s_random);
        Faction french = new Faction(FactionType.FRENCH, "French", s_random);

        _factions.Add(brits);
        _factions.Add(spanish);
        _factions.Add(dutch);
        _factions.Add(french);
    }

    private void initSettlements()
    {
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Settlements.csv");
        
        if (!File.Exists(filePath)) throw new FileLoadException("Settlements file is missing");

        using (StreamReader reader = new StreamReader(filePath))
        {
            // skip header
            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;

                var values = line.Split(',');

                FactionType type = FactionType.PIRATE;
                switch (values[0])
                {
                    case "Spanish":
                        type = FactionType.SPANISH;
                        break;

                    case "English":
                        type = FactionType.ENGLISH;
                        break;

                    case "French":
                        type = FactionType.FRENCH;
                        break;

                    case "Dutch":
                        type = FactionType.DUTCH;
                        break;
                    case "Pirate":
                        type= FactionType.PIRATE;
                        break;
                    default:
                        type = FactionType.PIRATE;
                        break;
                }
                string cityName = values[1];
                int x = int.Parse(values[2]);
                int y = int.Parse(values[3]);

                Faction faction = _factions.Find(f => f._type.Equals(type));

                if (faction == null) continue;

                Position pos = new Position(x, y);
                Settlement settlement = new Settlement(faction, pos, cityName);

                faction.AddSettlement(settlement);
                _camera.AddObject(settlement);
            }
        }
    }

    private void initShips()
    {
        foreach (var faction in _factions)
        {
            for (int i = 0; i < 5; i++)
            {
                int r = s_random.Next(faction.Settlements.Count);
                Sloop temp = new Sloop(faction, s_navmap,i.ToString(), faction.Settlements[r].Position, s_random);
                _camera.AddObject(temp);
            }
        }
    }

    public void Update()
    {

        var input = Console.ReadKey(true);
        _player.HandleInput(input.Key);
        foreach (var item in _factions)
        {
            foreach (var ship in item.Ships)
            {
                ship.Update();
            }
        }
        _camera.Render();
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

public enum EngineState
{
    STOPPED,
    RUNNING,
    EXIT
}