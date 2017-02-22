using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLOBALWARMINGDENIAL
{
    class Tile: Sprite
    {
        public bool IsDug = false;
        public World world;

        public Tile(GlobalWarmingDenial game, World world) : base(game)
        {
            this.world = world;
        }

        // Returns the tile immediately below this one
        public Tile GetBelow()
        {
            return world.GetTile(new Vector2(position.X + World.TILE_SIZE / 2, position.Y + World.TILE_SIZE * 1.5f));
        }

        // Returns the tile immediately left of this one
        public Tile GetLeft()
        {
            return world.GetTile(new Vector2(position.X - World.TILE_SIZE / 2, position.Y + World.TILE_SIZE / 2));
        }

        // Returns the tile immediately right of this one
        public Tile GetRight()
        {
            return world.GetTile(new Vector2(position.X + World.TILE_SIZE * 1.5f, position.Y + World.TILE_SIZE / 2));
        }

        // Returns the tile immediately above this one
        public Tile GetAbove()
        {
            return world.GetTile(new Vector2(position.X + World.TILE_SIZE / 2, position.Y - World.TILE_SIZE / 2));
        }


    }
}
