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
        GlobalWarmingDenial game;

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
                // Move the particle by the linear velocity, and rotate with the rotation velocity
                particle.position += particle.velocity;
                particle.rotation += particle.rotationVelocity;

                // A "sticker" particle is one that is destined to be stuck to the screen
                if (particle.sticker && !particle.stuck)
                {
                    // If this particle is going to, but is not yet, stuck to the screen, move it closer
                    particle.sizeRectangle.Width += 2;
                    particle.sizeRectangle.Height += 2;
                    particle.velocity.Y += 0.1f;

                    // Once this particle has reached the desired size, make it into a sticker
                    if (particle.sizeRectangle.Width > 120)
                    {
                        particle.stuck = true;
                        particle.position += game.camera;
                    }
                } else if (particle.stuck)
                {
                    // If this particle is stuck to the screen, slowly slide it downwards until it falls off
                    particle.velocity.Y += 0.1f;
                    particle.velocity.X = 0;
                    particle.rotationVelocity = 0;
                    if (particle.position.Y > game.graphics.GraphicsDevice.Viewport.Height) particles.Remove(particle);
                } else
                {
                    // if this is just a normal particle, apply some gravity to it
                    particle.velocity.Y += 1.1f;
                    if (particle.position.Y + game.camera.Y > game.graphics.GraphicsDevice.Viewport.Height) particles.Remove(particle);
                }
            }
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
