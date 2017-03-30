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
    public class Player: Sprite
    {
        public Rectangle previousHitbox = new Rectangle();
        public DiggingState Digging = new DiggingState();
        public int diggingDelay = 8;

        public Player(GlobalWarmingDenial game) : base(game)
        {
        }

        public void HandleInput(MouseState mouse, KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.Left)) DigInDirection(TileDirection.LEFT);
            else if (keyboard.IsKeyDown(Keys.D) || keyboard.IsKeyDown(Keys.Right)) DigInDirection(TileDirection.RIGHT);
            else if (keyboard.IsKeyDown(Keys.S) || keyboard.IsKeyDown(Keys.Down)) DigInDirection(TileDirection.DOWN);
            //else if (keyboard.IsKeyDown(Keys.W) || keyboard.IsKeyDown(Keys.Up)) velocity.Y -= 4f;
        }

        // Makes the player dig in a direction
        public void DigInDirection(TileDirection direction)
        {
            //if (Digging.IsDigging) return; // Cancel if already digging

            Tile currentTile = game.world.GetTile(position);

            //if (currentTile == null) return; // Cancel if there is no tile for the player

            Tile tile = currentTile.GetTileInDirection(direction);

            //if (tile == null) return; // Cancel if there is no tile below
            /*
            // Only dig if there is not a dug tile already
            if (tile != null && tile.type == TileType.DIRT)
            {
                // This is where the player will try to move to when it is digging
                Vector2 moveTarget = new Vector2();

                if (direction == TileDirection.DOWN)
                {
                    moveTarget = new Vector2(tile.position.X + World.TILE_SIZE / 2, tile.position.Y);
                    Digging.timeLeft = diggingDelay;
                    Digging.moveForce = 10;
                }

                if (direction == TileDirection.LEFT)
                {
                    // If there is an empty tile below, can't move left
                    Tile below = currentTile.GetTileInDirection(TileDirection.DOWN);
                    if (below != null && below.type == TileType.EMPTY) return;

                    moveTarget = new Vector2(tile.position.X + World.TILE_SIZE, tile.position.Y + World.TILE_SIZE / 2);
                    Digging.timeLeft = diggingDelay;
                    Digging.moveForce = 8;
                }

                if (direction == TileDirection.RIGHT)
                {
                    // If there is an empty tile below, can't move right
                    Tile below = currentTile.GetTileInDirection(TileDirection.DOWN);
                    if (below != null && below.type == TileType.EMPTY) return;
                    moveTarget = new Vector2(tile.position.X, tile.position.Y + World.TILE_SIZE / 2);
                    Digging.timeLeft = diggingDelay;
                    Digging.moveForce = 8;
                }

                // Set up the digging state to begin the digging

                Digging.IsDigging = true;
                Digging.diggingTarget = tile;
                Digging.moveTarget = moveTarget;
               // tile.Dig();
            }*/
            if (direction == TileDirection.LEFT) velocity.X -= 1.5f; // Instead move left and right if there are no tiles on those sides
            if (direction == TileDirection.RIGHT) velocity.X += 1.5f;
        }

        public override void Update ()
        { /*
            // Make it dig
            if (Digging.IsDigging)
            {
                this.animations.Play("Drill_Dig");
                // Reduce the time left on this dig
                Digging.timeLeft--;

                // Move the player to the center of the target
                Vector2 moveBy = (this.GetCenter() - Digging.moveTarget) / Digging.moveForce;
                velocity -= moveBy;

                if (Digging.timeLeft == 0)
                {
                    // Digging has finished
                    Digging.IsDigging = false;
                    Digging.diggingTarget.Dig();
                    this.animations.Play("Drill_Idle");
                }
            } else*/
            {
                // Only apply gravity when not digging
                if (!isOnGround()) velocity.Y += 1.1f;
            }

            // Bound the player within the walls
            if (position.Y <= 0) position.Y = 0;
            if (position.X < 80) position.X = 80;
            if (position.X + texture.Width > game.GraphicsDevice.Viewport.Width - 80) position.X = game.GraphicsDevice.Viewport.Width - texture.Width - 80;
            base.Update();
        }

        public bool isOnGround()
        {
            Rectangle playerHb = this.GetHitbox();
            Tile tile = game.world.GetTile(new Vector2(position.X + playerHb.Width / 2, position.Y + playerHb.Height + 5));
            return tile != null && tile.type != TileType.EMPTY;
        }

        // Collide the player with the tiles in the world
        public void CollideWithWorld (World world)
        {
            if (Digging.IsDigging) return; // If we're digging, don't do collision

            Vector2 center = new Vector2(position.X + texture.Width / 2, position.Y + texture.Height / 2);

            // Get the tiles surrounding the player (tiles that could collide)
            List<Tile> surroundingTiles = world.tiles;

            // Make two rectangles we can use to test intersection
            Rectangle playerHb = this.GetHitbox();

            foreach (Tile potentialCollision in surroundingTiles)
            {
                int attempts = 0;
                if (potentialCollision.type == TileType.EMPTY) continue;
                Rectangle tileHb = new Rectangle((int)potentialCollision.position.X, (int)potentialCollision.position.Y, World.TILE_SIZE, World.TILE_SIZE);

                // If we are intersecting with this tile, push the player back out
                while (playerHb.Intersects(tileHb) && attempts < 10000)
                {
                    attempts++;

                    playerHb = this.GetHitbox();

                    if (previousHitbox.Bottom <= tileHb.Top + 1) // Hit from top
                    {
                        position.Y = tileHb.Y - playerHb.Height;
                        velocity.Y = 0;
                    }

                    if (previousHitbox.Left >= tileHb.Right) // Hit from the right
                    {
                        position.X = tileHb.X + tileHb.Width;
                        velocity.X = 0;
                    }

                    if (previousHitbox.Right <= tileHb.Left) // Hit from the left
                    {
                        position.X = tileHb.X - playerHb.Width;
                        velocity.X = 0;
                    }

                    if (previousHitbox.Top >= tileHb.Bottom) // Hit from the bottom
                    {
                        position.Y = tileHb.Y + tileHb.Height;
                        velocity.Y = 0;
                    }
                }
            }

            // Keep track of this hitbox for the next frame
            previousHitbox = playerHb;
        }

        public override Rectangle GetHitbox()
        {
            return new Rectangle((int)position.X, (int)position.Y + 10, 50, 50);
        }
    }
}
