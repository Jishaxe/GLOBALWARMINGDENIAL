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

        // The size of each tile in a strip of particle frames
        public int PARTICLE_SIZE = 0;

        // Get the rectangle in the source texture that represents this variation
        public Rectangle GetSourceRectangle()
        {
            return new Rectangle(PARTICLE_SIZE * variation, 0, PARTICLE_SIZE, PARTICLE_SIZE);
        }

        public virtual void Update(Effects effects)
        {
            // Move the particle by the linear velocity, and rotate with the rotation velocity
            position += velocity;
            rotation += rotationVelocity;
            velocity.Y += 1.1f;
            if (position.Y + effects.game.camera.Y > effects.game.graphics.GraphicsDevice.Viewport.Height) effects.particles.Remove(this);
        }

        public virtual void Draw(SpriteBatch batch, Vector2 camera)
        {
            this.sizeRectangle.X = (int)(this.position.X + camera.X);
            this.sizeRectangle.Y = (int)(this.position.Y + camera.Y);

            batch.Draw(texture, sizeRectangle, GetSourceRectangle(), color, rotation, new Vector2(0, 0), SpriteEffects.None, 1);
        }
    }

    public class ChunkDirtParticle: Particle
    {
        // If the particle is a "sticker", it will increase in size, stop at a certain size, then slowly drop down attached to the camera
        // Almost like the chunk of dirt has smacked into the camera
        // If it is stuck, the particle is currently in a position where it is sliding down the screen.
        public bool sticker = false;
        public bool stuck = false;

        public ChunkDirtParticle()
        {
            this.PARTICLE_SIZE = 16;
        }

        public override void Update(Effects effects)
        {
            // Move the particle by the linear velocity, and rotate with the rotation velocity
            position += velocity;
            rotation += rotationVelocity;

            // A "sticker" particle is one that is destined to be stuck to the screen
            if (sticker && !stuck)
            {
                // If this particle is going to, but is not yet, stuck to the screen, move it closer
                sizeRectangle.Width += 2;
                sizeRectangle.Height += 2;
                velocity.Y += 0.1f;

                // Once this particle has reached the desired size, make it into a sticker
                if (sizeRectangle.Width > 120)
                {
                    stuck = true;
                    position += effects.game.camera;
                }
            }
            else if (stuck)
            {
                // If this particle is stuck to the screen, slowly slide it downwards until it falls off
                velocity.Y += 0.1f;
                velocity.X = 0;
                rotationVelocity = 0;
                if (position.Y > effects.game.graphics.GraphicsDevice.Viewport.Height) effects.particles.Remove(this);
            }
            else
            {
                // if this is just a normal particle, apply some gravity to it
                velocity.Y += 1.1f;
                if (position.Y + effects.game.camera.Y > effects.game.graphics.GraphicsDevice.Viewport.Height) effects.particles.Remove(this);
            }
        }

        public override void Draw(SpriteBatch batch, Vector2 camera)
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

            batch.Draw(texture, sizeRectangle, GetSourceRectangle(), color, rotation, new Vector2(0, 0), SpriteEffects.None, 1);
        }
    }

    public class TinyDirtParticle: Particle
    {
        public TinyDirtParticle()
        {
            this.PARTICLE_SIZE = 1;
        }
    }

    public class SparkParticle: Particle
    {
        public int life = 20;

        public SparkParticle()
        {
            this.PARTICLE_SIZE = 1;
        }

        public override void Update(Effects effects)
        {
            base.Update(effects);
            life--;

            if (life <= 0) effects.particles.Remove(this);
        }
    }
}
