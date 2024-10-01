using Godot;
using System;

public partial class DeltaSector : BP_Sector
{
    public override void InitSector()
    {
        CreateJumpgate("GaDejg01", "GaDejg02", -1000, 600);
        DebugStaion();
    }


    public void DebugStaion()
    {
        var solarScene = (PackedScene)ResourceLoader.Load("res://scenes/Buildings/SolarPowerPlant.tscn");
        var solarPlant = (SolarPowerPlant)solarScene.Instantiate();

        AddBuilding(solarPlant);
        solarPlant.SetSector(this);
        AddChild(solarPlant);
        solarPlant.Position = new Vector2(0, 0);
    }
}
