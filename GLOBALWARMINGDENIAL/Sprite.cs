using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLOBALWARMINGDENIAL
{
    // Basic sprite class, please extend this.
    public class Sprite
    {
        public Vector2 position = new Vector2();
        public Vector2 velocity;
        public float drag = 0.8f;
        public GlobalWarmingDenial game;
        public Texture2D texture;
        public Animation animation = new Animation();
        
        public Sprite (GlobalWarmingDenial game)
        {
            this.game = game;
        }

        public virtual void Draw (SpriteBatch batch)
        {
            if (animation.isPlaying)
            {
                animation.DrawCurrentFrame(batch, position + game.camera);
            }
            else
            {
                batch.Draw(texture, position + game.camera, Color.White);
            }
        }

        public Vector2 GetCenter()
        {
            return new Vector2(position.X + texture.Width / 2, position.Y + texture.Height / 2);
        }

        // Produce a hitbox of this player
        public Rectangle GetHitbox()
        {
            return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public virtual void Update (GameTime gameTime)
        {
            if (animation.isPlaying)
            {
                animation.Update(gameTime);
            }

            position += velocity;
            velocity *= drag;
        }
    }
}
