using Godot;
using NebulaFrontier.scripts.Item;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class MainScene : Node2D
{
    public Timer globalTimer;
    public static List<BP_Station> allBuildings = new List<BP_Station>();
    public List<BP_Ship> allShips = new List<BP_Ship>();
    public BP_Station selectedBuilding;


    private PackedScene activeScene;
    private CanvasLayer _canvasLayer;
    private StationDetailPopUp _stationPopUp;
    private ShipDetailPopup _shipDetailPopup;

     // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        StartTimer();

        activeScene = GD.Load<PackedScene>("res://scenes/Sectors/Alpha_Sector.tscn");
        _canvasLayer = GetNode<CanvasLayer>("CanvasLayer");

        var sectorInstance = (Sector)activeScene.Instantiate();
        //AddChild(sectorInstance);
    }

    private void StartTimer()
    {
        globalTimer = new Timer();
        //globalTimer.Autostart = true;
        globalTimer.WaitTime = 1.0f;
        globalTimer.OneShot = false;
        globalTimer.Timeout += OnGlobalTimerTick;
        AddChild(globalTimer);
        globalTimer.Start();
    }

    public void OnGlobalTimerTick()
    {
        //Debug.WriteLine("Tick!");
        UpdateAllBuildings();


    }

    private void OnMapObjectClicked(BP_Station b)
    {
        selectedBuilding = b;
    }

    public void CreatePopUp(BP_Station b)
    {
        _stationPopUp = (StationDetailPopUp)GD.Load<PackedScene>("res://scenes/Interface/StationDetailPopUp.tscn").Instantiate();
        _canvasLayer.AddChild(_stationPopUp);
        _stationPopUp.OpenWindow(b);
    }
    public void CreateShipDetailPopUp(BP_Ship b)
    {
        _shipDetailPopup = (ShipDetailPopup)GD.Load<PackedScene>("res://scenes/Interface/ShipDetailPopup.tscn").Instantiate();
        _canvasLayer.AddChild(_shipDetailPopup);
        _shipDetailPopup.OpenWindow(b);
    }



    public void UpdateAllBuildings()
    {
        foreach (BP_Station b in allBuildings)
        {
            b.ProductionTick();
        }
    }

    public void UpdateAllShips()
    {
        foreach (BP_Ship ship in allShips)
        {
            ship.UpdateOnTick();
        }
    }

    public void RegisterBuilding(BP_Station building)
    {
        allBuildings.Add(building);
        GameManager._allBuildings.Add(building);
        selectedBuilding = allBuildings[0];
    }

    public void RegisterShip(BP_Ship ship)
    {
        allShips.Add(ship);
    }

    public List<BP_Station> GetAllBuildings()
    {
        return allBuildings;
    }

    public override void _Process(double delta)
    {
    }

}
