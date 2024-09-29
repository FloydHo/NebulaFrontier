using Godot;
using NebulaFrontier.scripts.Datataypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NebulaFrontier.scripts.Item
{    public class Item
    {
        [Export] public string _name { get; set; }
        [Export] public string _type { get; set; }
        [Export] public string _description { get; set; }
        [Export] public int _UnitSize { get; set; }
        [Export] public TransportClass _transportClass { get; set; }
        [Export] public int _priceMin { get; set; }
        [Export] public int _priceMax { get; set; }

        public Item(string name, string type, string desciption, int unitSize, TransportClass tclass, int minPrice, int maxPrice)
        {
            _name = name;
            _type = type;   
            _description = desciption;
            _UnitSize = unitSize;
            _transportClass = tclass;
            _priceMin = minPrice;
            _priceMax = maxPrice;

        }
    }

}
