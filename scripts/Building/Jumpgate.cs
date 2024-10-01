using Godot;
using System;

public partial class Jumpgate : Node2D
{
    [Export] private string _name;
    [Export] private string _ID;
    [Export] private string _connectedGateID;
    [Export] private BP_Sector _connectToSector;
    [Export] private BP_Sector _currentSector;
    [Export] private Jumpgate _connectToJumpgate;


    public void TransferShip(BP_Ship ship)
    {
        RemoveShipFromSector(ship);
        AddShipToSector(ship);
    }

    public void InitGate() 
    {
        _connectToJumpgate = GameManager._allJumpgates[_connectedGateID];
        _connectToSector = _connectToJumpgate.GetSector();
    }

    public void RemoveShipFromSector(BP_Ship ship)
    {
        ship.GetParent().RemoveChild(ship);
        _currentSector.RemoveShipFromSector(ship);
    }

    public void AddShipToSector(BP_Ship ship)
    {
        _connectToSector.AddChild(ship);
        _connectToSector.AddShipToSector(ship);
        ship.Position = _connectToJumpgate.Position;
    }

    public string GetID() => _ID;
    public void SetID(string id) => _ID = id;
    public void SetCurrentSector(BP_Sector sec) => _currentSector = sec;
    public BP_Sector GetSector() => _currentSector;
    public void SetTargetSecotr(BP_Sector sect) => _connectToSector = sect;
    public BP_Sector GetTargetSector() => _connectToSector;
    public void SetConnectedJumpgate(Jumpgate jg) => _connectToJumpgate = jg;
    public Jumpgate GetConnectedJumpgate() => _connectToJumpgate;
    public void SetConnectedJumpgateID(string cid) => _connectedGateID = cid;

}
