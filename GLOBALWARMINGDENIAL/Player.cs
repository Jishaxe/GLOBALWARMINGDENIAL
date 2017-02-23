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
        public DiggingState Digging = new DiggingState();

        public Player(GlobalWarmingDenial game) : base(game)
        {
        }

        public void HandleInput(MouseState mouse, KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.Left)) DigInDirection(TileDirection.LEFT);
            if (keyboard.IsKeyDown(Keys.D) || keyboard.IsKeyDown(Keys.Right)) DigInDirection(TileDirection.RIGHT);
            if (keyboard.IsKeyDown(Keys.S) || keyboard.IsKeyDown(Keys.Down)) DigInDirection(TileDirection.DOWN);
            if (keyboard.IsKeyDown(Keys.W) || keyboard.IsKeyDown(Keys.Up)) velocity.Y -= 4f;
        }

        // Makes the player dig in a direction
        public void DigInDirection(TileDirection direction)
        {
            if (Digging.IsDigging) return; // Cancel if already digging

            Tile currentTile = game.world.GetTile(position);

            if (currentTile == null) return; // Cancel if there is no tile for the player

            Tile tile = currentTile.GetTileInDirection(direction);

            if (tile == null) return; // Cancel if there is no tile below

            // Only dig if there is not a dug tile already
            if (!tile.IsDug)
            {
                // Set up the digging state to begin the digging
                Digging.IsDigging = true;
                Digging.diggingTarget = tile;
                Digging.moveTarget = new Vector2(tile.position.X + World.TILE_SIZE / 2, tile.position.Y + World.TILE_SIZE / 2);
                Digging.timeLeft = 15;
            }
            else if (direction == TileDirection.LEFT) velocity.X -= 3f; // Instead move left and right if there are no tiles on those sides
            else if (direction == TileDirection.RIGHT) velocity.X += 3f;
        }

        public override void Update ()
        { 
            // Gravity
            velocity.Y += 1.1f;

            // Make it dig
            if (Digging.IsDigging)
            {
                // Reduce the time left on this dig
                Digging.timeLeft--;

                // Move the player to the center of the target
                Vector2 moveBy = (this.GetCenter() - Digging.moveTarget) / 20;
                velocity -= moveBy;

                if (Digging.timeLeft == 0)
                {
                    // Digging has finished
                    Digging.IsDigging = false;
                    Digging.diggingTarget.IsDug = true;
                }
            }

            // Bound the player within the walls
            if (position.Y <= 0) position.Y = 0;
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

                    playerHb = this.GetHitbox();
                   
                    if (previousHitbox.Bottom <= tileHb.Top + 1) // Hit from top
                    {
                        position.Y -= velocity.Y / 10;
                    } else if (previousHitbox.Left >= tileHb.Right) // Hit from the right
                    {
                        position.X -= velocity.X / 10;
                    } else if (previousHitbox.Right <= tileHb.Left) // Hit from the left
                    {
                        position.X -= velocity.X / 10;
                    } else if (previousHitbox.Top >= tileHb.Bottom) // Hit from the bottom
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
