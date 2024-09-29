using Godot;
using Godot.Collections;
using NebulaFrontier.scripts.Item;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

public partial class BP_Station : Node2D
{

    Area2D area2d;

    public int _ID;
    public static int _counterBuildings = 0;
    [Export] public string _name { get; set; }
    [Export] public int _ore { get; set; }       //Production Ressource
    //public int _food { get; set; }       //Consumed Ressource 1
    [Export] public int _energy { get; set; }       //Consumed Ressource 2
    [Export] public int _productionTime { get; set; }
    [Export] public int _currentProductionTime { get; set; }
    [Export] public int _productionRate { get; set; }
    [Export] public int _energyRate { get; set; }
    [Export] public int _foodRate { get; set; }
    [Export] public bool _isActive { get; set; }
    [Export] public Godot.Collections.Dictionary<int, int> ProducedItems { get; private set; } = new Godot.Collections.Dictionary<int, int>();
    [Export] public Godot.Collections.Dictionary<int, int> ResourceItems { get; private set; } = new Godot.Collections.Dictionary<int, int>();


    public List<BP_Ship> _hangar = new List<BP_Ship>();
    private bool _hasPopUp = false;


    ColorRect _crect;
    MainScene _mainScene;





    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _crect = GetNode<ColorRect>("ColorRect");
        _mainScene = GetTree().Root.GetNode<MainScene>("MainScene");
        SetProcessInput(true);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
        {
            Vector2 localMousePosition = GetLocalMousePosition();
            if (_crect.GetRect().HasPoint(localMousePosition) && !_hasPopUp)
            {
                _mainScene.CreatePopUp(this);
                _hasPopUp = true;
            }
            else
            {
            }
        }
    }

    public void ShipDocking(BP_Ship ship)
    {
        _hangar.Add(ship);
    }

    public void ShipUndock(BP_Ship ship)
    {
        _hangar.Remove(ship);
    }

    public int BuyWareFromStation(int itemID, int amount)
    {
        try
        {
            if (ProducedItems[itemID] >= amount)
            {
                ProducedItems[itemID] -= amount;
                return amount;
            }
            else
            {
                amount = ProducedItems[itemID];
                ProducedItems[itemID] = 0;
                return amount;
            }
        }
        catch
        {
            GD.Print("Bruder kauft nichts");
            return 0;
        }
    }
    public int SellWareToStation(int itemID, int amount)
    {
        ResourceItems[itemID] += amount;
        return amount;
    }

    public void SetHasPopup(bool b)
    {
        _hasPopUp = b;
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }


    public virtual void ProductionTick()
    {

    }

}
