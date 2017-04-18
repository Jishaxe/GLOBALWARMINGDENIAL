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
        public bool facingLeft = false;
        public bool facingRight = false;

        public Player(GlobalWarmingDenial game) : base(game)
        {
        }

        public void HandleInput(MouseState mouse, KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.Left))
            {

                DigInDirection(TileDirection.LEFT);
            }
            else if (keyboard.IsKeyDown(Keys.D) || keyboard.IsKeyDown(Keys.Right))
            {
                DigInDirection(TileDirection.RIGHT);
            }
            else if (keyboard.IsKeyDown(Keys.S) || keyboard.IsKeyDown(Keys.Down))
            {
                DigInDirection(TileDirection.DOWN);
            }
        }

        // Makes the player dig in a direction
        public void DigInDirection(TileDirection direction)
        {
            Tile currentTile = game.world.GetTile(previousHitbox.Center.ToVector2());
            Tile tile;

            if (currentTile != null && (tile = currentTile.GetTileInDirection(direction)) != null)
            {
                Vector2 digPosition = new Vector2(0, 0);
                if (direction == TileDirection.DOWN) digPosition = previousHitbox.Center.ToVector2() + new Vector2(0, previousHitbox.Height / 2);
                if (direction == TileDirection.LEFT) digPosition = previousHitbox.Center.ToVector2() + new Vector2(-previousHitbox.Width, 0);
                if (direction == TileDirection.RIGHT) digPosition = previousHitbox.Center.ToVector2() + new Vector2(previousHitbox.Width, 0);

                if (tile.type == TileType.DIRT)
                {
                    tile.Dig();
                    game.effects.MakeTileDigEffect(digPosition);
                } else if (tile.type != TileType.EMPTY)
                {
                    game.effects.MakeSparkEffect(digPosition);

                    switch (tile.type)
                    {
                        case TileType.GOLD:
                            tile.health -= 20;
                            game.money += 5;
                            break;
                        case TileType.COPPER:
                            tile.health -= 10;
                            game.money += 2;
                            break;
                        case TileType.CERAMIC:
                            tile.health -= 40;
                            break;
                        case TileType.ROCK:
                            tile.health -= 5;
                            break;
                    }

                    if (tile.health <= 0) tile.Dig();
                }

                // Position the player above the just digged block
                if (direction == TileDirection.DOWN) position.X = tile.position.X + (World.TILE_SIZE / 2) - previousHitbox.Width / 2;
            }

            //if (direction == TileDirection.DOWN) velocity.Y += 1f;
            if (direction == TileDirection.LEFT) velocity.X -= 5f; // Instead move left and right if there are no tiles on those sides
            if (direction == TileDirection.RIGHT) velocity.X += 5f;
        }

        public override void Update ()
        { 
            // Only apply gravity when on the ground
            if (!isOnGround()) velocity.Y += 1.1f;

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
                while (playerHb.Intersects(tileHb) && attempts < 100)
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
            return new Rectangle((int)position.X, (int)position.Y, 50, 60);
        }
    }
}
