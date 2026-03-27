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

    public Engine(Camera camera, Player player)
    {
        _camera = camera;
        _player = player;
        _factions = new List<Faction>(4);
    }

    public void Init()
    {
        initFactions();
        initSettlements();
    }

    private void initFactions()
    {
        Faction brits = new Faction(FactionType.ENGLISH, "English");
        Faction spanish = new Faction(FactionType.SPANISH, "Spanish");
        Faction dutch = new Faction(FactionType.DUTCH, "Dutch");
        Faction french = new Faction(FactionType.FRENCH, "French");

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

                // Default to a neutral/pirate faction if not found
                if (faction == null) continue;

                // 2. Create the Position and Settlement
                Position pos = new Position(x, y);
                Settlement settlement = new Settlement(faction, pos, cityName);

                // 3. Add to the faction's internal list (assuming Faction has an AddSettlement method)
                faction.AddSettlement(settlement);
                _camera.AddObject(settlement);
            }
        }
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
