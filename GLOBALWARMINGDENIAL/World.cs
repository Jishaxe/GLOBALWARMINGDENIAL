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
    public enum TileDirection
    {
        UP, DOWN, LEFT, RIGHT
    }

    public enum TileType
    {
        EMPTY, DIRT, ROCK
    }

    public class World
    {
        public const int TILE_SIZE = 69;

        Generator gen;
        public List<Tile> tiles = new List<Tile>();
        GlobalWarmingDenial game;

        // This is a list of the 12 different tile textures for all the directions 
        // The key is the file name, the key is the texture
        public Dictionary<string, Texture2D> tileTextures = new Dictionary<string, Texture2D>();


        public int lastY = -250;

        public World (GlobalWarmingDenial game)
        {
            this.game = game;
            gen = new Generator(game, this);
        }

        public void Load(ContentManager content)
        {
            Action<string> LoadTileTexture = (string name) =>
            {
                Texture2D tex = content.Load<Texture2D>(name);
                tileTextures.Add(name, tex);
            };

            LoadTileTexture("dirt");
            LoadTileTexture("dirtn");
            LoadTileTexture("dirtnw");
            LoadTileTexture("dirtnsw");
            LoadTileTexture("dirtesw");
            LoadTileTexture("dirtnes");
            LoadTileTexture("dirtnew");
            LoadTileTexture("dirtne");
            LoadTileTexture("dirte");
            LoadTileTexture("dirts");
            LoadTileTexture("dirtw");
            LoadTileTexture("dirtes");
            LoadTileTexture("dirtew");
            LoadTileTexture("dirtns");
            LoadTileTexture("dirtsw");
            LoadTileTexture("dirtnesw");
            LoadTileTexture("rock");
        }

        public void Update ()
        {
            // Continuously make new rows of tiles as time passes
            if (lastY + game.cameraTranslation.Y < 850) // If the camera is getting close to the last generated row
            {
                int y = lastY + TILE_SIZE; // Add another row onto the last one
                lastY = y;

                for (int x = 0; x < game.graphics.GraphicsDevice.Viewport.Width; x += TILE_SIZE)
                {
                    Tile tile = gen.PickNextTile(this.GetTile(new Vector2(x, y - 50)));

                    tile.position = new Vector2(x, y);
                    if (y < 250)
                    {
                        // Set the tile to be dug from all directions so it just shows up blank
                        tile.dugEast = true; tile.dugSouth = true; tile.dugNorth = true; tile.dugWest = true;
                        tile.type = TileType.EMPTY; // Make a blank space at the top of the screen
                    }


                    tiles.Add(tile);
                    this.CheckDugDirections();
                }
            }

            List<Tile> tileCopy = new List<Tile>(tiles);

            // Prune tiles that have left the screen
            foreach (Tile tile in tileCopy)
            {
                if ((-game.cameraTranslation.Y - tile.position.Y) > 200) // If the camera is 200 far from this tile, delete it
                {
                    tiles.Remove(tile);
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

        // Goes through each tile and bores tunnels in the correct direction
        public void CheckDugDirections ()
        {
            foreach (Tile tile in tiles)
            {
                if (tile.type != TileType.EMPTY) continue;

                Tile east = tile.GetTileInDirection(TileDirection.RIGHT);
                Tile south = tile.GetTileInDirection(TileDirection.DOWN);
                Tile west = tile.GetTileInDirection(TileDirection.LEFT);
                Tile north = tile.GetTileInDirection(TileDirection.UP);

                if (east != null && east.type == TileType.EMPTY) tile.dugEast = true;
                if (south != null && south.type == TileType.EMPTY) tile.dugSouth = true;
                if (west != null && west.type == TileType.EMPTY) tile.dugWest = true;
                if (north != null && north.type == TileType.EMPTY) tile.dugNorth = true;
            }
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
                Texture2D tex = null;

                // If this is a dug tile, draw it with the appropriate direction
                if (tile.type == TileType.EMPTY) tex = tileTextures["dirt" + tile.GetTextureSuffix()];
                if (tile.type == TileType.ROCK) tex = tileTextures["rock"];
                if (tile.type == TileType.DIRT) tex = tileTextures["dirt"];

                batch.Draw(tex, tile.position + game.cameraTranslation, tile.color);
            }
        }
    }
}
