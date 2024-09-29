using Godot;
using NebulaFrontier.scripts.Datataypes;
using System;

[GlobalClass]
public partial class ItemResource : Resource
{
    [Export] public string name { get; set; }
    [Export] public string _id { get; set; }
    [Export] public string _type { get; set; }
    [Export] public string _description { get; set; }
    [Export] public int _unitSize { get; set; }
    [Export] public TransportClass _transportClass { get; set; }
    [Export] public int _priceMin { get; set; }
    [Export] public int _priceMax { get; set; }
}
