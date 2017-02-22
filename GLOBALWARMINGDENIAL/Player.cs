using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLOBALWARMINGDENIAL
{
    class Player: Sprite
    {
        public Rectangle previousHitbox = new Rectangle();

        public Player ()
        {
        }

        public void HandleInput(MouseState mouse, KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.Left)) velocity.X -= 0.5f;
            if (keyboard.IsKeyDown(Keys.D) || keyboard.IsKeyDown(Keys.Right)) velocity.X += 0.5f;
            if (keyboard.IsKeyDown(Keys.S) || keyboard.IsKeyDown(Keys.Down)) velocity.Y += 0.5f;
            if (keyboard.IsKeyDown(Keys.W) || keyboard.IsKeyDown(Keys.Up)) velocity.Y -= 1.5f;
        }

        public override void Update ()
        { 
            // Gravity
            velocity.Y += 0.6f;

            base.Update();
        }

        // Collide the player with the tiles in the world
        public void CollideWithWorld (World world)
        {
            Vector2 center = new Vector2(position.X + texture.Width / 2, position.Y + texture.Height / 2);

            // Get the tiles surrounding the player (tiles that could collide)
            List<Tile> surroundingTiles = world.GetTilesAround(center, texture.Width + 200);
            
            // Make two rectangles we can use to test intersection
            Rectangle playerHb = this.GetHitbox();

            foreach (Tile potentialCollision in world.tiles)
            {
                if (potentialCollision.IsDug) continue;
                Rectangle tileHb = new Rectangle((int)potentialCollision.position.X, (int)potentialCollision.position.Y, World.TILE_SIZE, World.TILE_SIZE);

                // If we are intersecting with this tile, push the player back out
                // The velocity is being multiplied by 3 to give it the shaken look

                if (playerHb.Intersects(tileHb))
                {
                    // Hit from top
                    if (previousHitbox.Bottom <= tileHb.Top + 1)
                    {
                        position.Y = tileHb.Y - playerHb.Height - velocity.Y * 3f;
                    }
                    
                    // Hit from right
                    if (previousHitbox.Left >= tileHb.Right)
                    {
                        position.X = tileHb.Right - velocity.X * 3f;
                    }

                    // Hit from left
                    if (previousHitbox.Right <= tileHb.Left)
                    {
                        position.X = tileHb.Left - playerHb.Width - velocity.X * 3;
                    }
                }
            }

            // Keep track of this hitbox for the next frame
            previousHitbox = playerHb;
        }
    }
}
