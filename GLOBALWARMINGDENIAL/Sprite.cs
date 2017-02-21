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
    class Sprite
    {
        public Vector2 position = new Vector2();
        public Vector2 velocity;
        public float drag = 0.8f;
        public Texture2D texture;
        
        public void Draw (SpriteBatch batch)
        {
            batch.Draw(texture, position, Color.White);
        }

        // Produce a hitbox of this player
        public Rectangle GetHitbox()
        {
            return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public virtual void Update ()
        {
            position += velocity;
            velocity *= drag;
        }
    }
}
