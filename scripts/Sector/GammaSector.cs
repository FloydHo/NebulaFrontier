using Godot;
using System;

public partial class GammaSector : BP_Sector
{

    public override void InitSector()
    {
        //DebugCreateStations();
        //DebugCreateShip();
    }

    public void DebugCreateShip()
    {
        var haulerScene = (PackedScene)ResourceLoader.Load("res://scenes/Ships/Hauler.tscn");
        var hauler = (BP_Ship)haulerScene.Instantiate();
        AddChild(hauler);
        hauler.Position = new Vector2(200, 200);
    }

    public void DebugCreateStations()
    {
        var solarScene = (PackedScene)ResourceLoader.Load("res://scenes/Buildings/SolarPowerPlant.tscn");
        var solarPlant = (SolarPowerPlant)solarScene.Instantiate();

        AddBuilding(solarPlant);

        AddChild(solarPlant);
        solarPlant.Position = new Vector2(-200, 0);

        var oreMineScene = (PackedScene)ResourceLoader.Load("res://scenes/Buildings/OreMine.tscn");
        var oreMine = (OreMine)oreMineScene.Instantiate();

        AddBuilding(oreMine);

        AddChild(oreMine);
        oreMine.Position = new Vector2(50, 50);

    }
}
