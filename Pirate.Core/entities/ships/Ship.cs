

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
    private Vector2 _start;
    private Vector2 _destination;
    private bool wallHugging = false;
    private float _hitDistance;
    private int _hasntMoved;

    public Modifier[] _modifiers;
    public Faction _faction;
    private Navmap _navmap;
    private static Random _rnd;

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
        _rnd = random;
        _lastFramePos = _position;
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

    private void ClampInbounds()
    {
        float fixedX = Math.Clamp(_position.X, 0.0f, Constants.MAP_WIDTH);
        float fixedY = Math.Clamp(_position.Y, 0.0f, Constants.MAP_HEIGHT);
        _position = new Vector2(fixedX, fixedY);
    }

    public void Update(float deltaTime)
    {
        goToDestination(deltaTime);
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
    private void goToDestination(float deltaTime)
    {
        if (checkIfStuck())
        {
            _position = new Vector2(20f,20f);
            findNewDestination();
        }
        if (arrivedAtDestination())
        {
            findNewDestination();
            wallHugging = false;
        }
       

        Vector2 dirToGoal = Vector2.Normalize(_destination - _position);
        float distToGoal = Vector2.Distance(_position, _destination);

        // Open terrain
        if (!wallHugging)
        {
            if (!Move(dirToGoal, deltaTime))
            {
                wallHugging = true;
                _hitDistance = distToGoal;
            }
        }
        // Wall huggging
        else
        {
            bool crossedLine = Math.Sign(DistToMLine(_position)) != Math.Sign(DistToMLine(_lastFramePos));
            if ((crossedLine || NearMLine()) && distToGoal < _hitDistance - 1.0f)
            {
                if (_navmap.IsSailable(_position + dirToGoal))
                {
                    wallHugging = false;
                }
            }

            Vector2 escapeDir = dirToGoal;
            bool foundPath = false;

            for (float angle = 10; angle < 360; angle += 10)
            {
                Vector2 testDir = RotateVector(dirToGoal, angle);
                if (_navmap.IsSailable(_position + testDir))
                {
                    Move(testDir, deltaTime);
                    foundPath = true;
                    break;
                }
            }

            if (!foundPath) wallHugging = false; // Reset if stuck
        }

        _lastFramePos = _position;
        ClampInbounds();
    }

    private Vector2 RotateVector(Vector2 v, float degrees)
    {
        float radians = degrees * (MathF.PI / 180f);
        float sin = MathF.Sin(radians);
        float cos = MathF.Cos(radians);
        return new Vector2(v.X * cos - v.Y * sin, v.X * sin + v.Y * cos);
    }

    private bool NearMLine(float threshold = 0.1f)
    {
        Vector2 mLine = _destination - _start;
        Vector2 shipVec = _position - _start;

        float distToLine = MathF.Abs(DistToMLine(_position)) / mLine.Length();

        return distToLine < threshold;
    }

    private float DistToMLine(Vector2 position)
    {
        Vector2 mLine = _destination - _start;
        Vector2 shipVec = position - _start;

        return mLine.X * shipVec.Y - mLine.Y * shipVec.X;
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
        _start = new Vector2(_position.X, _position.Y);
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
