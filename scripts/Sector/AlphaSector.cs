using Godot;
using System;

public partial class AlphaSector : BP_Sector
{
    public override void InitSector()
    {
        CreateJumpgate("BeAljg02", "BeAljg01", -1000, -600);
        DebugCreateStations();
    }

    public void DebugCreateStations()
    {
        var StationScene = (PackedScene)ResourceLoader.Load("res://scenes/Buildings/TradingStation.tscn");
        var station = (BP_Station)StationScene.Instantiate();
        station.SetSector(this);
        AddBuilding(station);

        AddChild(station);
        station.Position = new Vector2(100, 200);

    }
}
