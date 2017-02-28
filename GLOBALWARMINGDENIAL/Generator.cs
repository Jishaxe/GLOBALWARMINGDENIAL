using System;
using System.Collections.Generic;

namespace GLOBALWARMINGDENIAL
{
    // Picks tiles to generate
    class Generator
    {
        GlobalWarmingDenial game;
        World world;
        Random rng = new Random();

        public Generator (GlobalWarmingDenial game, World world)
        {
            this.game = game;
            this.world = world;
        }

        public Tile PickNextTile(Tile lastTile)
        {
            int chanceOfEmpty = 10;
            int chanceOfDirt = 20;
            int chanceOfRock = 5;

            if (lastTile != null) {
                if (lastTile.type == TileType.EMPTY)
                {
                    chanceOfEmpty += 5;
                    chanceOfRock = 0;
                } else if (lastTile.type == TileType.ROCK)
                {
                    chanceOfRock += 3;
                }
            }

            List<TileType> tileBag = new List<TileType>();

            for (int i = 0; i < chanceOfEmpty; i++) tileBag.Add(TileType.EMPTY);
            for (int i = 0; i < chanceOfDirt; i++) tileBag.Add(TileType.DIRT);
            for (int i = 0; i < chanceOfRock; i++) tileBag.Add(TileType.ROCK);

            TileType chosenType = tileBag[rng.Next(tileBag.Count)];

            Tile tile = new Tile(game, world, chosenType);

            return tile;
        }
    }
}