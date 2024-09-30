using Godot;
using System;

public partial class AlphaSector : BP_Sector
{
    public override void InitSector()
    {
        CreateJumpgate("BeAljg02", "BeAljg01", -1000, -600);
    }
}
