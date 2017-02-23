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

        public Tile(GlobalWarmingDenial game, World world, TileType type) : base(game)
        {
            this.world = world;
            this.type = type;
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
