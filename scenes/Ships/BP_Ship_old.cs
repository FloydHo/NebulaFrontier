using Godot;
using NebulaFrontier.scenes.Ships.State;
using NebulaFrontier.scripts.Datataypes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

public partial class BP_Ship_old : Node2D
{
    //Export Variables

	[Export] private string _shipName;
    [Export] private string _shipDesc;
    [Export] private float _velocity;
	[Export] private float _steering;
	[Export] private int _crewSize;
	[Export] private int _hull;                         //HitPoints
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
    private bool _isDocked = false;
    private bool _tradeComplete = false;
    private bool _wait = false;
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
        CreateID();

        //SaveNodes
        _waitTimer = GetNode<Timer>("waitTimer");
        _waitTimer.Timeout += WaitTimer;
        _mainScene = GetTree().Root.GetNode<MainScene>("MainScene");
        _crect = GetNode<ColorRect>("ColorRect");

        SetState(new WaitState());

        _targetJumpgate = GameManager._allJumpgates["BeOmjg01"];
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
        {
            Vector2 localMousePosition = GetLocalMousePosition();
            if (_crect.GetRect().HasPoint(localMousePosition) && !_hasPopUp && Visible)
            {
                //_mainScene.CreateShipDetailPopUp(this);
                _hasPopUp = true;
            }
            else
            {
            }
        }
    }

    public override void _Process(double delta)
    {
        Travel();
    }

    public void SetState(IShipState newState)
    {
        //_currentState?.ExitState(this);
        _currentState = newState ;
        //_currentState.EnterState(this);
    }

    public void Travel()
    {
        if (_targetStationsList.Count != 0 && _jgPath.Count == 0)
        {
            if (_targetStationsList[_activeTargetStation].target.GetSector() == _inSector)
            {
                MoveToTargetStation();
            }
            else
            {
                _jgPath = PathToTargetSector(_targetStationsList[_activeTargetStation].target.GetSector());
            }
        }
        else if(_jgPath.Count != 0)
        {
            MoveToTargetJumpgate();
        }
    }


    public void MoveToTargetStation()
    {
        if (_tradingActive && _targetStationsList.Count != 0 && !_isDocked) //Docking
        {
            if ((int)Position.X <= (int)_targetStationsList[_activeTargetStation].target.Position.X + 5 && (int)Position.X >= (int)_targetStationsList[_activeTargetStation].target.Position.X - 5 &&
                (int)Position.Y <= (int)_targetStationsList[_activeTargetStation].target.Position.Y + 5 && (int)Position.Y >= (int)_targetStationsList[_activeTargetStation].target.Position.Y - 5)
            {
                //_targetStationsList[_activeTargetStation].target.ShipDocking(this);
                Hide();
                _isDocked = true;
                _waitTimer.WaitTime = _dockTimer;
                _waitTimer.Start();
            }
            else
            {
                MoveIfAlignedWithTarget((int)_targetStationsList[_activeTargetStation].target.Position.X, (int)_targetStationsList[_activeTargetStation].target.Position.Y);                                  //Traveling
            }
        }
        else if (_isDocked && !_tradeComplete && _wait)                     //Trading
        {
            _wait = false;

            if (_targetStationsList[_activeTargetStation].buy.Length > 0)   //Try to Buy
            {
                ExecuteBuy();
            }

            if (_targetStationsList[_activeTargetStation].sell.Length > 0)  //Try to Sell
            {
                ExecuteSell();
            }

            _tradeComplete = true;
            Hide();
            _waitTimer.WaitTime = _transportTimer;
            _waitTimer.Start();
        }
        else if (_isDocked && _tradeComplete && _wait)                      // Undocking   && Traiding Loop
        {
            _wait = false;

            //_targetStationsList[_activeTargetStation].target.ShipUndock(this);

            if (_activeTargetStation == _targetStationsList.Count - 1) _activeTargetStation = 0;
            else _activeTargetStation += 1;

            _tradeComplete = false;
            _isDocked = false;

            Show();
            //ShowCargo();
        }
    }

    public void ExecuteBuy()
    {
        foreach (var id in _targetStationsList[_activeTargetStation].buy)
        {
            int amount = _targetStationsList[_activeTargetStation].target.BuyWareFromStation(id, 1000);
            if (_cargo.ContainsKey(id))
            {
                _cargo[id] += amount;
            }
            else
            {
                _cargo.Add(id, amount);
            }
        }
        CalculateUsedCargo();
    }

    public void ExecuteSell() 
    {
        foreach (var id in _targetStationsList[_activeTargetStation].sell)
        {
            int amount = _targetStationsList[_activeTargetStation].target.SellWareToStation(id, _cargo[id]);
            _cargo[id] -= amount;
            if (_cargo[id] == 0) _cargo.Remove(id);
        }
        CalculateUsedCargo();
    }

    public void MoveToTargetJumpgate()
    {
        if (_jgPath.Count != 0)
        {
            _targetJumpgate = _jgPath[0];
            if ((int)Position.X <= (int)_targetJumpgate.Position.X + 5 && (int)Position.X >= (int)_targetJumpgate.Position.X - 5 &&
                (int)Position.Y <= (int)_targetJumpgate.Position.Y + 5 && (int)Position.Y >= (int)_targetJumpgate.Position.Y - 5)
            {
                //_targetJumpgate.TransferShip(this);
                _jgPath.RemoveAt(0);
            }
            else
            {
                MoveIfAlignedWithTarget((int)_targetJumpgate.Position.X, (int)_targetJumpgate.Position.Y);
            }
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

    public void MoveIfAlignedWithTarget(int tx, int ty)                               //Rotates Ship until it points in the Direction of Target (for now only Stations)   Koord einsetzen
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

public void CalculateUsedCargo() 
    {
        _usedCargoSpace = 0;
        foreach (var item in _cargo)
        {
            _usedCargoSpace += item.Value * ItemDB.Instance.GetItemByID(item.Key)._unitSize;
        }
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
        foreach(var c in _cargo)
        {
            GD.Print($"{c.Key} : {c.Value}");
        }
    }

    public void UpdateOnTick()
    {
    }
    public void WaitTimer()
    {
        _wait = true;
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
