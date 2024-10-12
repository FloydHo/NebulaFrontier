using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NebulaFrontier.scenes.Ships.State
{
    public interface IShipState
    {
        public void EnterState(BP_Ship ship);
        public void UpdateState(BP_Ship ship);
        public void ExitState(BP_Ship ship);
    }
}
