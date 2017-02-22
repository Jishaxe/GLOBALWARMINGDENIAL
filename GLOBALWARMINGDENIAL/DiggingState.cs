using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLOBALWARMINGDENIAL
{
    public class DiggingState
    {
        public bool IsDigging = false;
        public Tile diggingTarget = null;
        public Vector2 moveTarget = new Vector2();
        public int timeLeft = 0;
    }
}
