using Godot;
using System;
using System.Collections.Generic;

public partial class Sector : Node2D
{
    [Export] public string _sectorName;
    [Export] public int gridSize;

    private MainScene mainScene;

    public List<BP_Station> buildings = new List<BP_Station>();
    public List<BP_Ship> ships = new List<BP_Ship>();

    public bool isActive = false;

    public override void _Ready()
	{
        GameManager.Instance.AddSector(this);

        mainScene = GetTree().Root.GetNode<MainScene>("MainScene");

        var oreMineScene = (PackedScene)ResourceLoader.Load("res://scenes/Buildings/OreMine.tscn");
        var oreMine = (OreMine)oreMineScene.Instantiate();



        AddChild(oreMine);
        oreMine.Position = new Vector2(100, 200);


        AddBuilding(oreMine);


        var solarScene = (PackedScene)ResourceLoader.Load("res://scenes/Buildings/SolarPowerPlant.tscn");
        var solarPlant = (SolarPowerPlant)solarScene.Instantiate();

        AddChild(solarPlant);
        solarPlant.Position = new Vector2(400, 400);

        AddBuilding(solarPlant);


        var haulerScene = (PackedScene)ResourceLoader.Load("res://scenes/Ships/Hauler.tscn");
        var hauler = (BP_Ship)haulerScene.Instantiate();
        AddChild(hauler);
        hauler.Position = new Vector2(300, 500);
    }

    public void AddBuilding(BP_Station building)
    {
        buildings.Add(building);
        mainScene.RegisterBuilding(building);
    }

    public void AddShip(BP_Ship ship)
    {
        ships.Add(ship);
        mainScene.RegisterShip(ship);
    }

    public override void _Process(double delta)
	{
	}

    public override void _Draw()
    {
        Vector2 VPsize = GetViewportRect().Size;
        for (int x = 0; x < VPsize.X + gridSize; x += gridSize)
        {
            DrawLine(new Vector2(x, 0), new Vector2(x, VPsize.Y), new Godot.Color(1, 1, 1, 0.2f));
        }

        for (int y = 0; y < VPsize.X; y += gridSize)
        {
            DrawLine(new Vector2(0, y), new Vector2(VPsize.X, y), new Godot.Color(1, 1, 1, 0.2f));
        }

        //DrawCircle(new Vector2(VPsize.Y / 2, VPsize.Y / 2), 1, new Godot.Color(1, 1, 1, 1));
    }

}
