

using System.Numerics;

using Pirate.Core.UI.Graphics;
using Pirate.Core.Utils;

namespace Pirate.Core.entities.ships;

internal abstract class Ship : IDrawable
{
    public string _name;
    private float _speed = 0;
    public float _maxSpeed;
    public int _crew;
    public int _crewCapacity = 25;
    public int _cannons;
    public int _cannonsCapacity = 6;
    public int _cargoHoldCapacity = 10;
    public Vector2 _position;
    private Vector2 _lastFramePos;
    private Vector2 _destination;

    private List<Vector2> _pathPoints = new List<Vector2>();
    private int _hasntMoved;

    public Modifier[] _modifiers;
    public Faction _faction;
    private readonly Navmap _navmap;
    private readonly Random _rnd;

    public float Speed
    {
        get => _speed;
        set => _speed = Math.Clamp(value, 0, _maxSpeed);
    }

    public DrawPriority Priority => DrawPriority.SHIPS;

    public Ship(Faction faction,Navmap navmap, string name, Vector2 position, Random random)
    {
        _faction = faction;
        _navmap = navmap;
        _name = name;
        _maxSpeed = 5f;
        _speed = _maxSpeed;
        _crew = 8;
        _cannons = 4;
        _modifiers = new Modifier[8];
        faction.AddShip(this);
        _position = position;
        _lastFramePos = position;
        _rnd = random;
        _hasntMoved = _rnd.Next(30) + 30;
    }
    public bool Move(Vector2 deltaPosition,float deltaTime)
    {
        if (_navmap.IsSailable(_position+ deltaPosition))
        {
            _position = _position + (deltaPosition * deltaTime * _speed);
            return true;
        }
        return false;
    }

    public void MoveToPosition(Vector2 position,float deltaTime)
    {
        Vector2 direction = position - _position;
        direction = Vector2.Normalize(direction);
        Vector2 maxTravel = _position + direction * deltaTime * _speed;
        _position = (maxTravel-position).Length() < 2 ? position : maxTravel;
        if((maxTravel - position).Length() < 2)
        {
            _position = position;
            _pathPoints.Remove(position);
        }
        else
        {
            _position = maxTravel;
        }
    }

    private void ClampInbounds()
    {
        float fixedX = Math.Clamp(_position.X, 0.0f, Constants.MAP_WIDTH);
        float fixedY = Math.Clamp(_position.Y, 0.0f, Constants.MAP_HEIGHT);
        _position = new Vector2(fixedX, fixedY);
    }

    public void Update(float deltaTime)
    {
        GoToDestination(deltaTime);
    }

    private bool checkIfStuck()
    {
        if (_lastFramePos == _position)
        {
            _hasntMoved--;
        }
        else
        {
            _hasntMoved = 60;
        }

        return _hasntMoved == 0;
    }
    public void GoToDestination(float deltaTime)
    {
        if (checkIfStuck())
        {
            _position = new Vector2(20f,20f);
            findNewDestination();
        }
        if (arrivedAtDestination())
        {
            findNewDestination();
            Node start = new Node(_position, true);
            Node goal = new Node(_destination, true);
            _pathPoints = Pathfinder.FindPath(start,goal,_navmap);
        }

        if (_pathPoints.Count > 0)
        {
            MoveToPosition(_pathPoints[0],deltaTime);
        }
        
    }
    // Selects a random settlements from own or any allied faction
    // Sets selevted settlement as destination
    private void findNewDestination()
    {
        List<Settlement> alliedSettlements = _faction.Settlements;
        List<Faction> alliedFactions = _faction.Allies;
        foreach(Faction item in alliedFactions)
        {
            alliedSettlements.AddRange(item.Settlements);
        }
        int r = _rnd.Next(alliedSettlements.Count);
        Settlement selected = alliedSettlements[r];
        _destination = selected.Position;
    }

    private bool arrivedAtDestination()
    {

        return (_destination - _position).Length() < 5;
    }

    // TODO figure out a way to make it work or abandon concept
    public abstract void ApplyModifiers();
    public abstract void AddModifier(Modifier modifier);

    public override string ToString()
    {
        string res = "";
        res += _faction.Name + ": ";
        res += _name + "\n";
        res += _position.ToString();

        return res;
    }

    public void Draw(RenderBuffer renderBuffer, int x, int y)
    {

        int topX = x - (Constants.DRAW_WIDTH / 2);
        int topY = y - (Constants.DRAW_HEIGHT / 2);
        int posX = (int)_position.X;
        int posY = (int)_position.Y;
        // Checks if inside the cameras view
        if (topX < posX && posX < topX + Constants.DRAW_WIDTH - 1 &&
           topY < posY && posY < topY + Constants.DRAW_HEIGHT)
        {
            RGB foregroundColor = _faction.GetFactionColor();
            renderBuffer[posX - topX, (posY - topY) / 2] = new Pixel
            {
                Character = 'A',
                textRGB = foregroundColor,
            };
        }
    }
}
