using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NebulaFrontier.scenes.Ships.State
{
    internal class DockedState : IShipState
    {
        public void EnterState(BP_Ship ship)
        {
            ship.Hide();
        }
        public void UpdateState(BP_Ship ship)
        {
            if (ship.IsAutoTradingActive())
            {
                ship.ExecuteSell();
                ship.ExecuteBuy();
                ship.ChangeNextToStation();
            }
            ship.SetState(new TravelingState());
        }
        public void ExitState(BP_Ship ship)
        {
            ship.Show();
        }

    }
}
