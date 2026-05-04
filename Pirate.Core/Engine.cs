using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Text;
using System.Text.Json;

using Pirate.Core.entities;
using Pirate.Core.entities.ships.types;
using Pirate.Core.UI.Graphics;
using Pirate.Core.Utils;

namespace Pirate.Core;

internal class Engine
{
    private readonly Camera _camera;
    private readonly Player _player;
    private readonly List<Faction> _factions;
    private readonly static Random s_random = new Random(42);
    private static Navmap s_navmap;
    bool haltExecution = false;

    EngineState _state = EngineState.STOPPED; 

    public EngineState State {  get { return _state; } }
    public static Random Random {  get { return s_random; } }

    public Engine()
    {

        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "map_binary.txt");
        Map map = new Map(filePath);
        s_navmap = new Navmap(filePath);
        _player = new Player(s_navmap, s_random, "Playa");
        _camera = new Camera(_player);
        _camera.AddObject(map);
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

    public void Update(float deltaTime)
    {
        EngineTask task = EngineTask.PASS;
        
        if (Console.KeyAvailable)
        {
            var key = Console.ReadKey(true).Key;
            task = _camera._menu.HandleInput(key);
            haltExecution = HandleTask(task);
            _player.is_menu_active = haltExecution;
            _player.HandleInput(key, deltaTime);
        }
        if (!haltExecution)
        {
            _player.Update(deltaTime);
            foreach (var item in _factions)
            {
                foreach (var ship in item.Ships)
                {
                    ship.Update(deltaTime);
                }
            }
        }
    }

    private bool HandleTask(EngineTask task)
    {
        switch(task)
        {
            case EngineTask.HALT:
                return true;
            case EngineTask.SAVE:
                Save();
                break;
            case EngineTask.LOAD:
                Load(); 
                break;
            case EngineTask.EXIT:
                Exit();
                return true;
            default: return false;
        }
        return false;
    }

    public void Render()
    {
        _camera.Render();
    }

    public void Save()
    {
        // Accessing private fields might require adding a GetSaveData() method to Player.cs
        var saveData = new PlayerSaveData
        {
            Speed = _player._flagship.Speed,
            Angle = _player._compass.Direction,
            X = _player.Position.x,
            Y = _player.Position.y,
            // Wealth = _player.GetWealth(), // Example helper
            // Speed = _player.Ship.Speed
        };

        string json = JsonSerializer.Serialize(saveData, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText("player_save.json", json);
    }

    public void Load()
    {
        string filePath = "player_save.json";
        if (!File.Exists(filePath)) return;

        string json = File.ReadAllText(filePath);
        PlayerSaveData data = JsonSerializer.Deserialize<PlayerSaveData>(json);

        if (data != null)
        {
            _player.LoadState(data);
        }
    }

    public void Exit()
    {
        _state = EngineState.EXIT;
    }
}

public enum EngineState
{
    STOPPED,
    RUNNING,
    EXIT
}

public enum EngineTask
{
    HALT,
    SAVE,
    LOAD,
    EXIT,
    PASS
}