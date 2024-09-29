using Godot;
using System;
using System.Collections.Generic;

public partial class BP_Sector : Node2D
{
    //General Variables

    [Export] private string _sectorName;
    [Export] private string _ownerRace;

    private List<BP_Ship> _shipList = new List<BP_Ship>();
    private List<BP_Station> _stationListBP = new List<BP_Station>();

    private bool _hasFoxus = false;

    //Hexagon Variables
    private float _hexRadius;
    HexagonScript _hex;
    int _borderWidth = 60;
    [Export] Color _borderColor = Colors.Blue;

    MainScene _mainScene;

    public override void _Ready()
    {
        //base._Ready();
        _mainScene = GetTree().Root.GetNode<MainScene>("MainScene");
        _hex = GetNode<HexagonScript>("Hexagon");
        _hexRadius = _hex.GetHexRadius();

        DebugCreateStations();
        DebugCreateShip();
    }
    public override void _Draw()
    {
        //base._Draw();
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
    }

    public void AddShip(BP_Ship ship)
    {
        _shipList.Add(ship);
        _mainScene.RegisterShip(ship);
    }

    public void DebugCreateShip()
    {
        var haulerScene = (PackedScene)ResourceLoader.Load("res://scenes/Ships/Hauler.tscn");
        var hauler = (BP_Ship)haulerScene.Instantiate();
        AddChild(hauler);
        hauler.Position = new Vector2(900, 500);
    }

    public void DebugCreateStations()
    {
        var solarScene = (PackedScene)ResourceLoader.Load("res://scenes/Buildings/SolarPowerPlant.tscn");
        var solarPlant = (SolarPowerPlant)solarScene.Instantiate();

        AddBuilding(solarPlant);

        AddChild(solarPlant);
        solarPlant.Position = new Vector2(400, 400);

        var oreMineScene = (PackedScene)ResourceLoader.Load("res://scenes/Buildings/OreMine.tscn");
        var oreMine = (OreMine)oreMineScene.Instantiate();

        AddBuilding(oreMine);

        AddChild(oreMine);
        oreMine.Position = new Vector2(100, 200);

    }

    public float GetHexRadius() => _hexRadius;






}
