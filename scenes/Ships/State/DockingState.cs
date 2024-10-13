using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NebulaFrontier.scenes.Ships.State
{
    public class DockingState : IShipState
    {
        private bool _dockingInProgress = false; 

        public void EnterState(BP_Ship ship)
        {

        }
        public void UpdateState(BP_Ship ship)
        {
            if (!_dockingInProgress && ship.HasArrivedAtTargetStation())
            {
                _dockingInProgress = true;
                ship.StartStateChangeTimer(3.0f, () => ship.SetState(new DockedState()));
            }
            else if(!_dockingInProgress)
            {
                ship.DockAtStation();
            }
        }
        public void ExitState(BP_Ship ship)
        {

        }

    }
}
