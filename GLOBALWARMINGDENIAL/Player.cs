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
        public Player ()
        {
        }

        public void HandleInput(MouseState mouse, KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.Left)) velocity.X -= 3;
            if (keyboard.IsKeyDown(Keys.D) || keyboard.IsKeyDown(Keys.Right)) velocity.X += 3;
            if (keyboard.IsKeyDown(Keys.S) || keyboard.IsKeyDown(Keys.Down)) velocity.Y += 3;         
        }

        public override void Update ()
        {
            base.Update();

            // Gravity
            velocity.Y += 0.8f;
        }

        // Collide the player with the tiles in the world
        public void CollideWithWorld (World world)
        {
            Vector2 center = new Vector2(position.X + texture.Width / 2, position.Y + texture.Height / 2);

            // Get the tiles surrounding the player (tiles that could collide)
            List<Tile> surroundingTiles = world.GetTilesAround(center, texture.Width - 20);

            // Make two rectangles we can use to test intersection
            Rectangle playerHb = this.GetHitbox();

            foreach (Tile potentialCollision in surroundingTiles)
            {
                if (potentialCollision.IsDug) continue;
                Rectangle tileHb = new Rectangle((int)potentialCollision.position.X, (int)potentialCollision.position.Y, World.TILE_SIZE, World.TILE_SIZE);

                // If we are intersecting with this tile, push the player back out
                if (playerHb.Intersects(tileHb))
                {
                    // Hit from top
                    if (playerHb.Bottom <= tileHb.Top)
                    {
                        position.Y = tileHb.Y - playerHb.Height - velocity.Y;
                    }

                    // Hit from right
                    if (playerHb.Left >= tileHb.Right)
                    {
                        position.X = tileHb.Right + velocity.X;
                    }

                    // Hit from left
                    if (playerHb.Right <= tileHb.Left)
                    {
                        position.X = tileHb.Left - playerHb.Width - velocity.X;
                    }
                }
            }
        }
    }
}
