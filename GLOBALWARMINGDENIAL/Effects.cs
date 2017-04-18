using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLOBALWARMINGDENIAL
{
    public class Effects
    {
        public Random random = new Random();
        public GlobalWarmingDenial game;

        public List<Particle> particles = new List<Particle>();
        public ParticleFactory factory = new ParticleFactory();

        public Effects(GlobalWarmingDenial game)
        {
            this.game = game;
            this.factory.LoadParticleTextures(game.GraphicsDevice, game.Content);
        }

        /**
         * Make an exploding particle effect for digging
         */
        public void MakeTileDigEffect(Vector2 position)
        {
            for (int i = 0; i < 2; i++)
            {
                Vector2 offset = new Vector2(random.Next(-5, 5), random.Next(-5, 5));
                particles.Add(factory.MakeChunkDirtParticle(position + offset));
                particles.Add(factory.MakeTinyDirtParticle(position + offset));
            }

            for (int i = 0; i < 70; i++)
            {
                Vector2 offset = new Vector2(random.Next(-20, 20), random.Next(-20, 20));
                particles.Add(factory.MakeTinyDirtParticle(position + offset));
            }
        }

        public void Update()
        {
            // Make a copy of the list so we don't get enumeration issues
            List<Particle> particlesCopy = new List<Particle>(particles);

            // For every particle we have right now
            foreach (Particle particle in particlesCopy)
            {
                particle.Update(this);
            }
        }

        internal void MakeSparkEffect(Vector2 digPosition)
        {
        }

        public void Draw(SpriteBatch batch)
        {
            foreach (Particle particle in particles)
            {
                particle.Draw(batch, game.camera);
            }
        }
    }
}
