using Godot;
using System;

public partial class OmegaSector : BP_Sector
{
    public override void InitSector()
    {
        CreateJumpgate("BeOmjg02", "BeOmjg01", 1000, -550);
        DebugCreateStations();
    }
        

    public void DebugCreateStations()
    {
        var oreMineScene = (PackedScene)ResourceLoader.Load("res://scenes/Buildings/OreMine.tscn");
        var oreMine = (OreMine)oreMineScene.Instantiate();

        oreMine.SetSector(this);
        AddBuilding(oreMine);

        AddChild(oreMine);
        oreMine.Position = new Vector2(100, 200);

    }
}
