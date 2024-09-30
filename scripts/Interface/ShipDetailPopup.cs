using Godot;
using System;
using System.Runtime.CompilerServices;


public partial class ShipDetailPopup : Control
{
    private BP_Ship _targetShip;

    private Window _popup;
    private Label _pilotname;
    private Label _shipname;
    private Label _shipID;
    private Label _currentSector;
    private Label _flightime;
    private Label _cargo;
    
    private VBoxContainer _cargoContainerItemName;
    private VBoxContainer _cargoContainerItemAmount;
    private VBoxContainer _cargoContainerItemVol;
    private VBoxContainer _cargoContainerItemTc;

    public override void _Ready()
    {
        _popup = GetNode<Window>("ShipDetailPopup");

        _pilotname = GetNode<Label>("Window/Control/VBoxContainer/pnl_pilot/lbl_pilot");
        _shipID = GetNode<Label>("Window/Control/VBoxContainer/pnl_details/vbox_details/lbl_id");
        _currentSector = GetNode<Label>("Window/Control/VBoxContainer/pnl_details/vbox_details/lbl_currenSector");
        _flightime = GetNode<Label>("Window/Control/VBoxContainer/pnl_details/vbox_details/lbl_flighttime");
        _cargo = GetNode<Label>("Window/Control/VBoxContainer/pnl_cargo/vbox_cargo/hbox_cargolegend/lbl_cargo");
        _cargoContainerItemName = GetNode<VBoxContainer>("Window/Control/VBoxContainer/pnl_cargo/vbox_cargo/pnl_shipCargo/hbox_shipCargo/vbox_cargoItemName");
        _cargoContainerItemAmount = GetNode<VBoxContainer>("Window/Control/VBoxContainer/pnl_cargo/vbox_cargo/pnl_shipCargo/hbox_shipCargo/vbox_cargoItemAmount");
        _cargoContainerItemVol = GetNode<VBoxContainer>("Window/Control/VBoxContainer/pnl_cargo/vbox_cargo/pnl_shipCargo/hbox_shipCargo/vbox_cargoItemVol");
        _cargoContainerItemTc = GetNode<VBoxContainer>("Window/Control/VBoxContainer/pnl_cargo/vbox_cargo/pnl_shipCargo/hbox_shipCargo/vbox_cargoItemTc");
    }

    public override void _Process(double delta)
    {
        if (_targetShip != null)
        {
            UpdateContent();
        }
    }

    public void OpenWindow(BP_Ship s)
    {
        _targetShip = s;
        UpdateContent();
        Show();
    }

    public void UpdateContent()
    {
        ClearItemList();
        _pilotname.Text = "Peter Maffay";
        _shipID.Text = ($"ID: {_targetShip.GetShipID()}");
        _currentSector.Text  = ($"In Sektor: {_targetShip.GetInSector().GetSectorName()}");
        _flightime.Text = "00:00:20";
        _cargo.Text = ($"Fracht: {_targetShip.GetUsedCargoSize(),-5}/{_targetShip.GetCargoSize()}");

        foreach (var item in _targetShip._cargo) 
        {
            Label labelItemName = new Label();
            Label labelItemAmount = new Label();
            Label labelItemVol = new Label();
            Label labelItemTc = new Label();

            labelItemName.Text = $"{ItemDB.Instance.GetItemByID(item.Key).name}";
            labelItemAmount.Text = $"{item.Value}";
            labelItemVol.Text = $"{ItemDB.Instance.GetItemByID(item.Key)._unitSize}";
            labelItemTc.Text = $"{ItemDB.Instance.GetItemByID(item.Key)._transportClass}";

            _cargoContainerItemName.AddChild(labelItemName);
            _cargoContainerItemAmount.AddChild(labelItemAmount);
            _cargoContainerItemVol.AddChild(labelItemVol);
            _cargoContainerItemTc.AddChild(labelItemTc);
        }
    }

    public void ClearItemList()
    {
        foreach (Node child in _cargoContainerItemName.GetChildren())
        {
            child.QueueFree();  
        }
        foreach (Node child in _cargoContainerItemAmount.GetChildren())
        {
            child.QueueFree();
        }
        foreach (Node child in _cargoContainerItemVol.GetChildren())
        {
            child.QueueFree();
        }
        foreach (Node child in _cargoContainerItemTc.GetChildren())
        {
            child.QueueFree();
        }
    }

    public void _on_window_close_requested()
    {
        _targetShip.SetHasPopup(false);
        Hide();
        QueueFree();
    }
}
