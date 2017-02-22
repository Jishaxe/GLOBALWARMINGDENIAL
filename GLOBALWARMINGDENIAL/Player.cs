using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public Player(GlobalWarmingDenial game) : base(game)
        {
        }

        public void HandleInput(MouseState mouse, KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.Left)) velocity.X -= 1f;
            if (keyboard.IsKeyDown(Keys.D) || keyboard.IsKeyDown(Keys.Right)) velocity.X += 1f;
            if (keyboard.IsKeyDown(Keys.S) || keyboard.IsKeyDown(Keys.Down)) velocity.Y += 0.5f;
            if (keyboard.IsKeyDown(Keys.W) || keyboard.IsKeyDown(Keys.Up)) velocity.Y -= 1.5f;
        }

        public override void Update ()
        { 
            // Gravity
            velocity.Y += 0.6f;

            if (position.X < 0) position.X = 0;
            if (position.X + texture.Width > game.GraphicsDevice.Viewport.Width) position.X = game.GraphicsDevice.Viewport.Width - texture.Width;
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

            foreach (Tile potentialCollision in surroundingTiles)
            {
                int attempts = 0;
                if (potentialCollision.IsDug) continue;
                Rectangle tileHb = new Rectangle((int)potentialCollision.position.X, (int)potentialCollision.position.Y, World.TILE_SIZE, World.TILE_SIZE);

                // If we are intersecting with this tile, push the player back out

                while (playerHb.Intersects(tileHb) && attempts < 100)
                {
                    attempts++;
                    // Hit from top
                    if (previousHitbox.Bottom <= tileHb.Top + 1)
                    {
                        position.Y -= velocity.Y / 10;
                    }

                    playerHb = this.GetHitbox();
                    
                    // Hit from right
                    if (previousHitbox.Left >= tileHb.Right)
                    {
                        position.X -= velocity.X / 10;
                    }

                    // Hit from left
                    if (previousHitbox.Right <= tileHb.Left)
                    {
                        position.X -= velocity.X / 10;
                    }

                    // Hit from bottom
                    if (previousHitbox.Top >= tileHb.Bottom)
                    {
                        position.Y -= velocity.Y / 10;
                    }
                }
            }

            // Keep track of this hitbox for the next frame
            previousHitbox = playerHb;
        }
    }
}
