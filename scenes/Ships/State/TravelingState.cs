using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NebulaFrontier.scenes.Ships.State
{
    public class TravelingState : IShipState
    {
        public void EnterState(BP_Ship ship)
        {

        }
        public void UpdateState(BP_Ship ship)
        {
            if (ship.HasReachedTargetSector())
            {
                ship.SetState(new DockingState());
            }
            else
            {
                if (ship.HasArrivedAtTargetJumpgate())
                {
                    ship.JumpgateTransfer();
                }
                else
                {
                    ship.MoveToNextJumpgate();
                }
            }
        }
        public void ExitState(BP_Ship ship)
        {

        }

    }
}
