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


        // Pciks a random tile
        public Tile PickNextTile(Tile lastTile)
        {
            int chanceOfEmpty = 4;
            int chanceOfDirt = 40;
            int chanceOfRock = 2;
            int chanceOfGold = 1;
            int chanceOfCopper = 2;
            int chanceOfCeramic = 3;

            // More chance of empty above a near empty and same for rock
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

            // Pick one from our bag of possible tiles
            List<TileType> tileBag = new List<TileType>();

            for (int i = 0; i < chanceOfEmpty; i++) tileBag.Add(TileType.EMPTY);
            for (int i = 0; i < chanceOfDirt; i++) tileBag.Add(TileType.DIRT);
            for (int i = 0; i < chanceOfRock; i++) tileBag.Add(TileType.ROCK);
            for (int i = 0; i < chanceOfGold; i++) tileBag.Add(TileType.GOLD);
            for (int i = 0; i < chanceOfCopper; i++) tileBag.Add(TileType.COPPER);
            for (int i = 0; i < chanceOfCeramic; i++) tileBag.Add(TileType.CERAMIC);

            TileType chosenType = tileBag[rng.Next(tileBag.Count)];

            Tile tile = new Tile(game, world, chosenType);

            return tile;
        }
    }
}