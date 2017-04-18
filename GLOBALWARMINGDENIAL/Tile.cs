using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLOBALWARMINGDENIAL
{
    public class Tile: Sprite
    {
        public TileType type;
        public World world;

        // The health of this tile
        public int health = 100; 

        // Keeps a track of which direction this tile has been bored into when the type is TileType.EMPTY
        // This will render a sprite with holes in the different directions
        public bool dugNorth = false;
        public bool dugEast = false;
        public bool dugSouth = false;
        public bool dugWest = false;

        public Tile(GlobalWarmingDenial game, World world, TileType type) : base(game)
        {
            this.world = world;
            this.type = type;
        }

        // Digs out this block
        public void Dig()
        {
            type = TileType.EMPTY;
            world.CheckDugDirections(); // Updates the world to recreate tunnels
        }

        // Adds the dug directions together to make a string that we will use to render the correct sprite
        public string GetTextureSuffix()
        {
            string result = "";
            if (dugNorth) result += "n";
            if (dugEast) result += "e";
            if (dugSouth) result += "s";
            if (dugWest) result += "w";
            return result;
        }

        public Tile GetTileInDirection(TileDirection direction)
        {
            switch (direction)
            {
                case TileDirection.DOWN:
                    return world.GetTile(new Vector2(position.X + World.TILE_SIZE / 2, position.Y + World.TILE_SIZE * 1.5f));
                case TileDirection.LEFT:
                    return world.GetTile(new Vector2(position.X - World.TILE_SIZE / 2, position.Y + World.TILE_SIZE / 2));
                case TileDirection.RIGHT:
                    return world.GetTile(new Vector2(position.X + World.TILE_SIZE * 1.5f, position.Y + World.TILE_SIZE / 2));
                case TileDirection.UP:
                    return world.GetTile(new Vector2(position.X + World.TILE_SIZE / 2, position.Y - World.TILE_SIZE / 2));
                default:
                    return null;
            }
        }
    }
}
