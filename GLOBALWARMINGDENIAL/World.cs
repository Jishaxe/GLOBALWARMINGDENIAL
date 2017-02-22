using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLOBALWARMINGDENIAL
{
    class World
    {
        public const int TILE_SIZE = 100;

        public Texture2D dirt;
        public List<Tile> tiles = new List<Tile>();
        GlobalWarmingDenial game;

        public World (GlobalWarmingDenial game)
        {
            this.game = game;

        }

        // Builds the world based on the current state
        public void Build ()
        {

            int width = game.graphics.GraphicsDevice.Viewport.Width;
            int height = game.graphics.GraphicsDevice.Viewport.Height;

            // For the moment, just fill up the whole screen with dirt
            for (int x = 0; x < width; x += TILE_SIZE)
            {
                for (int y = 250; y < height; y += TILE_SIZE)
                {
                    Tile tile = new Tile(game);
                    tile.position = new Vector2(x, y);
                    tiles.Add(tile);
                }
            }
        }

        // Get the tile at this position
        public Tile GetTile (Vector2 position)
        {
            Tile result = null;

            foreach (Tile tile in tiles)
            {
                // Make a rectangle out of this tile to use the Contains method
                if (new Rectangle((int)tile.position.X, (int)tile.position.Y, TILE_SIZE, TILE_SIZE)
                    .Contains(new Point((int)position.X, (int)position.Y)))
                {
                    result = tile;
                }
            }

            return result;
        }

        // Get the tiles surrounding a point
        public List<Tile> GetTilesAround(Vector2 position, int radius)
        {
            List<Tile> results = new List<Tile>();

            // Return every tile which of center point is within the radius
            foreach (Tile tile in tiles)
            {
                Vector2 centerOfTile = new Vector2(tile.position.X + TILE_SIZE / 2, tile.position.Y + TILE_SIZE / 2);
                Vector2 vectorToTarget = centerOfTile - position;

                if (vectorToTarget.Length() <= radius) results.Add(tile);
            }

            return results;
        }

        public void Draw (SpriteBatch batch)
        {
            foreach (Tile tile in tiles)
            {
                // Don't draw this tile if it's dug up
                if (tile.IsDug) continue;
                batch.Draw(dirt, tile.position + game.camera, Color.White);
            }
        }
    }
}
