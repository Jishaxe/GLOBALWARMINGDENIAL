using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLOBALWARMINGDENIAL
{
    // Information about the current digging state of the player
    public class DiggingState
    {
        public bool IsDigging = false; // Is the player digging?
        public Tile diggingTarget = null; // What is the player digging?
        public int moveForce = 10; // How fast should the player move to the target? (lower numbers: stronger)
        public Vector2 moveTarget = new Vector2(); // Where should the player move when digging?
        public int timeLeft = 0; // How much time is left before the digging is finished?
    }
}
