using Godot;
using Godot.Collections;
using NebulaFrontier.scripts.Item;
using System;

public partial class ItemDB : Node
{
    public static ItemDB Instance { get; private set; }

    [Export] public Godot.Collections.Dictionary<int, ItemResource> Items { get; private set; } = new Godot.Collections.Dictionary<int, ItemResource>();

    public override void _Ready()
    {
        if (Instance == null)
        {
            Instance = this;
            GD.Print("ItemDB initialized");
        }
        else
        {
            QueueFree();
        }

    }

    public ItemResource GetItemByID(int id)
    {
        if (Items.ContainsKey(id))
        {
            return Items[id];
        }
        return null;
    }
}
