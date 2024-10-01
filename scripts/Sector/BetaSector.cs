using Godot;
using System;

public partial class BetaSector : BP_Sector
{
    public override void InitSector()
    {
        CreateJumpgate("BeAljg01","BeAljg02" , 1000, 500);
        CreateJumpgate("BeGajg01", "BeGajg02", 0, -1150);
        CreateJumpgate("BeOmjg01", "BeOmjg02", -1000, 500);
        DebugCreateShip();
    }



    public void DebugCreateShip()
    {
        var haulerScene = (PackedScene)ResourceLoader.Load("res://scenes/Ships/Hauler.tscn");
        var hauler = (BP_Ship)haulerScene.Instantiate();
        AddChild(hauler);
        hauler.Position = new Vector2(-1000, 800);
        AddShipToSector(hauler);
    }
}
