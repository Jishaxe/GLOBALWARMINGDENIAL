using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace GLOBALWARMINGDENIAL
{
    // Base class for a particle
    public class Particle
    {
        // Position, linear velocity, rotation, rotational velocity
        public Vector2 position = new Vector2(0, 0);
        public Vector2 velocity = new Vector2(0, 0);
        public float rotation = 0;
        public float rotationVelocity = 0;

        // Texture for this particle
        public Texture2D texture;

        // Colour to draw this particle this
        public Color color = Color.White;

        // A rectangle that determines what size the particle is drawn at
        public Rectangle sizeRectangle = new Rectangle(0, 0, 0, 0);

        // Which variation of particle this is
        public int variation = 0;

        // If the particle is a "sticker", it will increase in size, stop at a certain size, then slowly drop down attached to the camera
        // Almost like the chunk of dirt has smacked into the camera
        // If it is stuck, the particle is currently in a position where it is sliding down the screen.
        public bool sticker = false;
        public bool stuck = false;

        // The size of each tile in a strip of particle frames
        public int PARTICLE_SIZE = 0;

        // Get the rectangle in the source texture that represents this variation
        public Rectangle GetSourceRectangle()
        {
            return new Rectangle(PARTICLE_SIZE * variation, 0, PARTICLE_SIZE, PARTICLE_SIZE);
        }

        public void Draw(SpriteBatch batch, Vector2 camera)
        {
            // If this particle is stuck, add the camera position to the destination square
            if (!stuck)
            {
                this.sizeRectangle.X = (int)(this.position.X + camera.X);
                this.sizeRectangle.Y = (int)(this.position.Y + camera.Y);
            }
            else
            {
                this.sizeRectangle.X = (int)(this.position.X);
                this.sizeRectangle.Y = (int)(this.position.Y);
            }
            Rectangle srcR = GetSourceRectangle();

            batch.Draw(texture, sizeRectangle, GetSourceRectangle(), color, rotation, new Vector2(0, 0), SpriteEffects.None, 1);
        }
    }

    public class ChunkDirtParticle: Particle
    {
        public ChunkDirtParticle()
        {
            this.PARTICLE_SIZE = 16;
        }
    }

    public class TinyDirtParticle: Particle
    {
        public TinyDirtParticle()
        {
            this.PARTICLE_SIZE = 1;
        }
    }
}
