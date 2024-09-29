using Godot;
using System;
using System.Diagnostics;

public partial class OreMine : BP_Station
{

    public OreMine()
    {
        _counterBuildings++;
        _ID = _counterBuildings;
        _name = "OreMine";
        _ore = 0;
        _energy = 10;
        _productionTime = 100;
        _currentProductionTime = 0;
        _productionRate = 5;
        _energyRate = 20;
        _foodRate = 5;
        _isActive = true;
    }
    public override void ProductionTick()
    {
        if (_isActive && _energy >= _energyRate)
        {

            if (_currentProductionTime >= _productionTime)
            {
                ProducedItems[100] += _productionRate;
                // _food -= _foodRate;
                ResourceItems[101] -= _energyRate;
                _currentProductionTime = 0;
                
            }
            else
                _currentProductionTime += 5;


        }
        else _isActive = false;

        
    }
}
