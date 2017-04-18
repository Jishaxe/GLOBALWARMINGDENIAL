using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace GLOBALWARMINGDENIAL
{
    public class ParticleFactory
    {
        public Texture2D chunkDirtTexture;
        public Texture2D tinyDirtTexture;
        Random random = new Random();

        // Make a tiny dirt particle
        public TinyDirtParticle MakeTinyDirtParticle(Vector2 position)
        {
            TinyDirtParticle particle = new TinyDirtParticle();

            particle.position = position;

            int scale = random.Next(1, 8);
            particle.sizeRectangle.Width = scale;
            particle.sizeRectangle.Height = scale;

            particle.color = new Color(40, 28, 2);
            particle.texture = tinyDirtTexture;

            // Choose a random velocity
            particle.velocity = new Vector2(random.Next(-5, 5), random.Next(-5, 5));

            return particle;
        }

        // Make a chunky dirt particle at the specified position
        public ChunkDirtParticle MakeChunkDirtParticle(Vector2 position)
        {
            ChunkDirtParticle particle = new ChunkDirtParticle();
            particle.position = position;

            // Chose a random spin speed
            particle.rotationVelocity = ((float)(random.NextDouble() - 1) * 2f) / 20f;

            // Choose a random scale
            int scale = random.Next(2, 5);
            particle.sizeRectangle.Width = particle.PARTICLE_SIZE * scale;
            particle.sizeRectangle.Height = particle.PARTICLE_SIZE * scale;

            // Choose a random variation
            particle.variation = random.Next(0, chunkDirtTexture.Width / particle.PARTICLE_SIZE);


            // Choose a random brightness
            int brightness = random.Next(150, 256);
            particle.color = new Color(brightness, brightness, brightness);

            // Choose a random velocity
            particle.velocity = new Vector2(random.Next(-5, 5), random.Next(-5, 5));

            // Small chance of being a particle that sticks to the screen
            if (random.NextDouble() > 0.99d) particle.sticker = true;

            particle.texture = chunkDirtTexture;

            return particle;
        }

        public void LoadParticleTextures(GraphicsDevice graphics, ContentManager content)
        {
            this.chunkDirtTexture = content.Load<Texture2D>("particles/dirt");
            this.tinyDirtTexture = new Texture2D(graphics, 1, 1);
            this.tinyDirtTexture.SetData<Color>(new Color[]{ Color.White });
        }
    }
}
