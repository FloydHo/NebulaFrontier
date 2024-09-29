using Godot;
using System;

public partial class SolarPowerPlant : BP_Station
{
    public SolarPowerPlant() 
    {
        _counterBuildings++;
        _ID = _counterBuildings;
        _name = "";
        _ore = 0;
        _energy = 0;
        _productionTime = 0;
        _currentProductionTime = 0;
        _productionRate = 0;
        _energyRate = 0;
        _foodRate = 0;
        _isActive = true;
    }
    public override void ProductionTick()
    {
        if (_isActive)
        {

            if (_currentProductionTime >= _productionTime)
            {
                ProducedItems[101] += _productionRate;
                // _food -= _foodRate;
                _energy += _productionRate;
                _currentProductionTime = 0;

            }
            else
                _currentProductionTime += 10;


        }
        else _isActive = false;


    }
}
