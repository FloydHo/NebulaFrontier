using Godot;
using NebulaFrontier.scenes.Ships.State;
using NebulaFrontier.scripts.Datataypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class BP_Ship : Node2D
{
    //Export Variables

    [Export] private string _shipName;
    [Export] private string _shipDesc;
    [Export] private float _velocity;
    [Export] private float _steering;
    [Export] private int _crewSize;
    [Export] private int _hull; //HitPoints
    [Export] private int _armour;
    [Export] private int _weaponDmg;
    [Export] private int _cargoSize;
    [Export] private TransportClass _transportClass;
    [Export] private int _price;
    [Export] private string _race;
    [Export] public Godot.Collections.Dictionary<int, int> _cargo { get; private set; } = new Godot.Collections.Dictionary<int, int>();

    //Intern Variables
    private int _usedCargoSpace = 0;
    private BP_Sector _inSector;
    private string _shipID;

    //Variables for Trading (Buy/Sell)
    private bool _tradingActive = true;
    List<(BP_Station target, int[] buy, int[] sell)> _targetStationsList = new List<(BP_Station, int[], int[])>();
    int _activeTargetStation = 0;

    //States
    private IShipState _currentState;

    List<Jumpgate> _jgPath = new List<Jumpgate>();
    Jumpgate _targetJumpgate;

    Timer _waitTimer;
    int _dockTimer = 3;
    int _transportTimer = 6;
    int _undockTimer = 3;


    //Interface Variables 
    private bool _hasPopUp = false;
    private MainScene _mainScene;
    ColorRect _crect;


    public override void _Ready()
    {
        SetState(new WaitState());

        CreateID();
        _crect = GetNode<ColorRect>("ColorRect");
        _mainScene = GetTree().Root.GetNode<MainScene>("MainScene");

        _targetJumpgate = GameManager._allJumpgates["BeOmjg01"];
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
        {
            Vector2 localMousePosition = GetLocalMousePosition();
            if (_crect.GetRect().HasPoint(localMousePosition) && !_hasPopUp && Visible)
            {
                _mainScene.CreateShipDetailPopUp(this);
                _hasPopUp = true;
            }
            else
            {
            }
        }
    }

    public override void _Process(double delta)
    {
        _currentState.UpdateState(this);
    }

    public void SetState(IShipState newState)
    {
        _currentState?.ExitState(this);
        _currentState = newState;
        _currentState.EnterState(this);
    }

    public bool HasTarget()
    {
        if (_targetStationsList .Count > 0) return true;
        return false;
    }

    public bool HasReachedTargetSector()
    {
        if (_targetStationsList[_activeTargetStation].target.GetSector() == _inSector)
        {
            return true;
        }
        else
        {
            _jgPath = PathToTargetSector(_targetStationsList[_activeTargetStation].target.GetSector());
            return false;
        }
    }

    public bool HasArrivedAtTargetJumpgate()
    {
        return HasArrivedAtTarget((int)_targetJumpgate.Position.X, (int)_targetJumpgate.Position.Y);
    }

    public bool HasArrivedAtTargetStation()
    {
        return HasArrivedAtTarget((int)_targetStationsList[_activeTargetStation].target.Position.X, (int)_targetStationsList[_activeTargetStation].target.Position.Y);
    }

    public bool HasArrivedAtTarget(int tx, int ty)
    {
        if ((int)Position.X <= tx + 2 && (int)Position.X >= tx - 2 &&
            (int)Position.Y <= ty + 2 && (int)Position.Y >= ty - 2)
        {
            return true;
        }
        return false;
    }

    public void MoveToNextJumpgate()
    {
        if (_jgPath.Count != 0)
        {
            _targetJumpgate = _jgPath[0];
            MoveIfAlignedWithTarget((int)_targetJumpgate.Position.X, (int)_targetJumpgate.Position.Y);
        }
        else GD.Print("Jumpgate List Empty Error 'MoveToNextJumpgate()'");
    }

    public void DockAtStation()
    {
        MoveIfAlignedWithTarget((int)_targetStationsList[_activeTargetStation].target.Position.X, (int)_targetStationsList[_activeTargetStation].target.Position.Y);                                  //Traveling
    }

    public void MoveIfAlignedWithTarget(int tx, int ty) //Rotates Ship until it points in the Direction of Target (for now only Stations)   Koord einsetzen
    {
        if (isAligned(tx, ty))
        {
            Vector2 directionToStation = (new Vector2(tx, ty) - Position).Normalized();
            Position = new Vector2(Position.X + directionToStation.X * 0.5f, Position.Y + directionToStation.Y * 0.5f);
        }
        else
        {
            RotateTowards(tx, ty);
        }
    }

    public bool isAligned(int x, int y)
    {
        Vector2 directionToStation = (new Vector2(x, y) - Position).Normalized();
        float angleToStation = directionToStation.Angle();
        float rotationDifference = Mathf.Abs(Mathf.Wrap(Rotation - angleToStation, -Mathf.Pi, Mathf.Pi));
        return rotationDifference <= 0.01;
    }

    public void RotateTowards(int x, int y)
    {
        Vector2 directionToStation = (new Vector2(x, y) - Position).Normalized();
        float targetRotation = directionToStation.Angle();
        float rotationDifference = Mathf.Wrap(targetRotation - Rotation, -Mathf.Pi, Mathf.Pi);
        float direction = rotationDifference > 0 ? 1 : -1;
        Rotation += direction * _steering * (float)GetProcessDeltaTime();
    }

    public void JumpgateTransfer()
    {
        _targetJumpgate.TransferShip(this);
        _jgPath.RemoveAt(0);
    }



    //Misc

    public void CalculateUsedCargo()
    {
        _usedCargoSpace = 0;
        foreach (var item in _cargo)
        {
            _usedCargoSpace += item.Value * ItemDB.Instance.GetItemByID(item.Key)._unitSize;
        }
    }

    public List<Jumpgate> PathToTargetSector(BP_Sector target)
    {
        bool searchOn = true;

        List<Jumpgate> path = new List<Jumpgate>();

        List<(BP_Sector sector, List<Jumpgate> paths)> toCheck = new List<(BP_Sector, List<Jumpgate>)>();
        List<BP_Sector> checkedSector = new List<BP_Sector>();

        toCheck.Add((_inSector, new List<Jumpgate>()));
        while (searchOn)
        {
            var check = toCheck[0];
            foreach (var gate in check.sector.GetListOfJumpgates())
            {
                if (checkedSector.Contains(gate.GetTargetSector()))
                {

                }
                else if (gate.GetTargetSector() == target)
                {
                    searchOn = false;
                    path = check.paths;
                    path.Add(gate);
                }
                else
                {
                    List<Jumpgate> newpath = new List<Jumpgate>(check.paths);
                    newpath.Add(gate);
                    toCheck.Add((gate.GetTargetSector(), newpath));

                }
            }

            checkedSector.Add(check.sector);
            toCheck.RemoveAt(0);

            if (toCheck.Count == 0) searchOn = false;
        }
        return path;
    }

        private void CreateID()
    {
        bool unique = false;
        string identifier = "";
        while (!unique)
        {
            identifier = Guid.NewGuid().ToString().Substring(0, 4).ToUpper();
            identifier += "-";
            identifier += Guid.NewGuid().ToString().Substring(0, 3).ToUpper();
            if (!GameManager._allShips.ContainsKey(identifier))
            {
                unique = true;
            }
        }
        _shipID = identifier;
    }

    public int GetUsedCargoSize() => _usedCargoSpace;

    public int GetCargoSize() => _cargoSize;

    public void ShowCargo()
    {
        foreach (var c in _cargo)
        {
            GD.Print($"{c.Key} : {c.Value}");
        }
    }

    public void UpdateOnTick()
    {
    }

    public void SetHasPopup(bool b)
    {
        _hasPopUp = b;
    }

    public void SetPath(BP_Sector sec)
    {
        _jgPath = PathToTargetSector(sec);
    }

    public new string GetName() => _shipName;
    public string GetShipID() => _shipID;
    public BP_Sector GetInSector() => _inSector;
    public void SetInSector(BP_Sector insec) => _inSector = insec;
    public void AddTargetStation((BP_Station target, int[] buy, int[] sell) give) => _targetStationsList.Add(give);


}

