using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLOBALWARMINGDENIAL
{
    public class Particle
    {
        public Vector2 position = new Vector2(0, 0);
        public Vector2 velocity = new Vector2(0, 0);
        public float rotation = 0;
        public float rotationDirection = 0;
        public Color color = Color.White;
        public Rectangle surface = new Rectangle(0, 0, 0, 0);
        public ParticleType type;
        public int variation;
    }
}
