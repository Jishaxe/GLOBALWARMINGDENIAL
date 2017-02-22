using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLOBALWARMINGDENIAL
{
    class Tile: Sprite
    {
        public bool IsDug = false;

        public Tile(GlobalWarmingDenial game) : base(game)
        {
        }
    }
}
