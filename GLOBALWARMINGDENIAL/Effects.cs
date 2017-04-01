using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLOBALWARMINGDENIAL
{
    public enum ParticleType
    {
        DIRT, SPARKS
    }

    public class Effects
    {
        public Random random = new Random();
        const int PARTICLE_SIZE = 16;
        GlobalWarmingDenial game;
        public Texture2D dirtParticles;
        public Texture2D sparkParticles;
        public List<Particle> particles = new List<Particle>();

        public Effects(GlobalWarmingDenial game)
        {
            this.game = game;
        }

        /**
         * Make an exploding particle effect for digging
         */
        public void MakeTileDigEffect(Vector2 position, ParticleType type)
        {
            int burstCount = random.Next(0, 8);
            for (int i = 0; i < burstCount; i++)
            {
                Vector2 positionOffset = new Vector2(random.Next(-5, 5), random.Next(-5, 5));

                Particle part = new Particle();
                part.position = position + positionOffset;
                part.type = type;

                part.rotationDirection = ((float)(random.NextDouble() - 1) * 2f) / 20f;

                int scale = random.Next(2, 5);
                part.surface.Width = PARTICLE_SIZE * scale;
                part.surface.Height = PARTICLE_SIZE * scale;

                part.variation = random.Next(0, GetVariationCount(type));

                int brightness = random.Next(150, 256);
                part.color = new Color(brightness, brightness, brightness);
                part.velocity = new Vector2(random.Next(-5, 5), random.Next(-5, 5));

                if (random.NextDouble() > 0.9d) part.sticker = true;
                particles.Add(part);
            }
        }

        public void MakeFlash(Vector2 position)
        {

        }

        public void Update()
        {
            List<Particle> particlesCopy = new List<Particle>(particles);

            foreach (Particle particle in particlesCopy)
            {
                particle.position += particle.velocity;
                particle.rotation += particle.rotationDirection;

                if (particle.sticker && !particle.stuck)
                {
                    particle.surface.Width += 3;
                    particle.surface.Height += 3;
                    particle.velocity.Y += 0.1f;

                    if (particle.surface.Width > 120)
                    {
                        particle.stuck = true;
                        particle.position += game.camera;
                    }
                } else if (particle.stuck)
                {
                    particle.velocity.Y += 0.1f;
                    particle.velocity.X = 0;
                    particle.rotationDirection = 0;
                    if (particle.position.Y > game.graphics.GraphicsDevice.Viewport.Height) particles.Remove(particle);
                } else
                {
                    particle.velocity.Y += 1.1f;
                    if (particle.position.Y + game.camera.Y > game.graphics.GraphicsDevice.Viewport.Height) particles.Remove(particle);
                }


            }
        }

        public void Draw(SpriteBatch batch)
        {
            foreach (Particle particle in particles)
            {
                Texture2D tex = GetTexture(particle.type);

                if (!particle.stuck)
                {
                    particle.surface.X = (int)(particle.position.X + game.camera.X);
                    particle.surface.Y = (int)(particle.position.Y + game.camera.Y);
                } else
                {
                    particle.surface.X = (int)particle.position.X;
                    particle.surface.Y = (int)particle.position.Y;
                }

                batch.Draw(tex, particle.surface, GetVariation(particle.variation), particle.color, particle.rotation, new Vector2(0, 0), SpriteEffects.None, 1);
            }
        }

        public Rectangle GetVariation(int i)
        {
            return new Rectangle(PARTICLE_SIZE * i, 0, PARTICLE_SIZE, PARTICLE_SIZE);
        }

        public Texture2D GetTexture(ParticleType type)
        {
            if (type == ParticleType.DIRT) return dirtParticles;
            if (type == ParticleType.SPARKS) return sparkParticles;
            return null;
        }

        public int GetVariationCount(ParticleType type)
        {
            return GetTexture(type).Width / PARTICLE_SIZE;
        }
    }
}
