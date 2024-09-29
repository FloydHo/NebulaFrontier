using Godot;
using System;
using System.Collections.Generic;

public partial class StationDetailPopUp : Control
{
    private VBoxContainer itemListContainer;
    private VBoxContainer resListContainer;
    private VBoxContainer hangarListContainer;
    private BP_Station _targetBuilding;
    private Window _popup;

    public override void _Ready()
    {
        itemListContainer = GetNode<VBoxContainer>("PU_StationDetails/VBoxContainer/vbc_products");
        resListContainer = GetNode<VBoxContainer>("PU_StationDetails/VBoxContainer/vbc_resources");
        hangarListContainer = GetNode<VBoxContainer>("PU_StationDetails/VBoxContainer/vbc_hangar");
        _popup = GetNode<Window>("PU_StationDetails");
    }

    public override void _Process(double delta)
    {
        if (_targetBuilding != null)
        {
            UpdateContent();
        }
    }

    public void OpenWindow(BP_Station b)
    {
        _targetBuilding = b;
        UpdateContent();


        Show();
    }

    public void UpdateContent()
    {
        ClearItemList();
        _popup.Title = _targetBuilding._name;
        GetNode<Label>("PU_StationDetails/VBoxContainer/lbl_id").Text = ($"ID: {_targetBuilding._ID.ToString()}");
        foreach(var item in _targetBuilding.ProducedItems)
        {
            HBoxContainer hBoxContainer = new HBoxContainer();

            ProgressBar progressBar = new ProgressBar();            
            progressBar.CustomMinimumSize = new Vector2(150, 0);
            Control spacer = new Control();

            progressBar.Value = _targetBuilding._currentProductionTime;

            Label labelCount = new Label();
            Label labelName = new Label();
            labelCount.Text = item.Value.ToString();
            labelName.Text = ItemDB.Instance.Items[item.Key].name;

            hBoxContainer.AddChild(labelName);
            hBoxContainer.AddChild(labelCount);
            hBoxContainer.AddChild(spacer);
            hBoxContainer.AddChild(progressBar);
            itemListContainer.AddChild(hBoxContainer);

            hBoxContainer.SizeFlagsVertical = SizeFlags.Expand;
            spacer.SizeFlagsHorizontal = SizeFlags.Expand;
            progressBar.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        }

        foreach (var item in _targetBuilding.ResourceItems)
        {
            HBoxContainer hBoxContainer = new HBoxContainer();

            Control spacer = new Control();
            
            Label labelCount = new Label();
            Label labelName = new Label();
            labelCount.Text = item.Value.ToString();
            labelName.Text = ItemDB.Instance.Items[item.Key].name;

            hBoxContainer.AddChild(labelName);
            hBoxContainer.AddChild(spacer);
            hBoxContainer.AddChild(labelCount);
            resListContainer.AddChild(hBoxContainer);

            hBoxContainer.SizeFlagsVertical = SizeFlags.Expand;
            spacer.SizeFlagsHorizontal = SizeFlags.Expand;
        }

        foreach (var ship in _targetBuilding._hangar)
        {

            Label labelName = new Label();
            labelName.Text = ship.GetName();
            hangarListContainer.AddChild(labelName);

            labelName.SizeFlagsVertical = SizeFlags.Expand;
            labelName.SizeFlagsHorizontal = SizeFlags.Expand;
        }


    }

    public void ClearItemList()
    {
        foreach (Node child in itemListContainer.GetChildren())
        {
            child.QueueFree(); 
        }

        foreach (Node child in resListContainer.GetChildren())
        {
            child.QueueFree();  
        }
        foreach (Node child in hangarListContainer.GetChildren())
        {
            child.QueueFree();  
        }
    }

    public void _on_pu_station_details_close_requested()
    {
        _targetBuilding.SetHasPopup(false);
        Hide();
        QueueFree();
    }
}
