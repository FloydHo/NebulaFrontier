using Godot;
using System;

public partial class GammaSector : BP_Sector
{

    public override void InitSector()
    {
        CreateJumpgate("GaDejg02", "GaDejg01", 1000, -600);
        CreateJumpgate("BeGajg02", "BeGajg01", 0, 1200);
    }
}
