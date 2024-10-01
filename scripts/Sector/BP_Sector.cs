using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class BP_Sector : Node2D
{
    //General Variables

    [Export] private string _sectorName;
    [Export] private string _ownerRace;

    private List<BP_Ship> _shipList = new List<BP_Ship>();
    private List<BP_Station> _stationListBP = new List<BP_Station>();
    private List<Jumpgate> _jumpgates = new List<Jumpgate>();

    private bool _hasFoxus = false;

    //Hexagon Variables
    private float _hexRadius;
    HexagonScript _hex;
    int _borderWidth = 60;
    [Export] Color _borderColor = Colors.Blue;

    MainScene _mainScene;

    public override void _Ready()
    {
        

        _mainScene = GetTree().Root.GetNode<MainScene>("MainScene");
        _hex = GetNode<HexagonScript>("Hexagon");
        _hexRadius = _hex.GetHexRadius();

        GameManager._allSectors.Add(this);

        InitSector();
    }

    public virtual void InitSector()
    {

    }

    public void ConnectJumpgates()
    {
        foreach (var gate in _jumpgates)
        {
            gate.InitGate();
        }
    }

    public override void _Draw()
    {
        DrawHexagon();
    }


    public void DrawHexagon()
    {
        Vector2[] points = new Vector2[6];
        for (int i = 0; i < 6; i++)
        {
            float angle = (Mathf.Pi / 3) * i; 
            points[i] = new Vector2(
                _hexRadius * Mathf.Cos(angle),
                _hexRadius * Mathf.Sin(angle)
            );
        }
        _hex.Polygon = points;
        DrawPolyline(new Vector2[] { points[0], points[1], points[2], points[3], points[4], points[5], points[0] }, _borderColor, _borderWidth, true);
    }

    public void AddBuilding(BP_Station building)
    {
        _stationListBP.Add(building);
        _mainScene.RegisterBuilding(building);
        GameManager._allBuildings.Add(building);
    }

    public void AddShip(BP_Ship ship)
    {
        AddShipToSector(ship);
        _mainScene.RegisterShip(ship);
        GameManager._allShips.Add(ship.GetShipID(), ship);
    }

    public void RemoveShipFromSector(BP_Ship ship) 
    { 
        _shipList.Remove(ship);
    }
    public void AddShipToSector(BP_Ship ship)
    {
        _shipList.Add(ship);
        ship.SetInSector(this);
    }

    public void CreateJumpgate(string id, string cid, int x, int y)
    {
        Vector2 newPos = new Vector2(x, y);
        PackedScene jg = (PackedScene)ResourceLoader.Load("res://scenes/Buildings/Jumpgate.tscn");
        Jumpgate jumpgate = (Jumpgate)jg.Instantiate();
        jumpgate.Position = newPos;
        jumpgate.SetID(id);
        jumpgate.SetCurrentSector(this);
        jumpgate.SetConnectedJumpgateID(cid);
        AddChild(jumpgate);
        AddJumpgate(jumpgate);
    }

    public void AddJumpgate(Jumpgate jg)
    {
        _jumpgates.Add(jg);
        GameManager._allJumpgates.Add(jg.GetID(), jg);
    }

    public float GetHexRadius() => _hexRadius;
    public string GetSectorName() => _sectorName;

    public List<Jumpgate> GetListOfJumpgates() => _jumpgates;






}
