using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLOBALWARMINGDENIAL
{
    // Represents the heads up display in the top left corner
    public class HUD
    {
        public Texture2D tv;
        public Texture2D bar;
        GlobalWarmingDenial game;

        public HUD (GlobalWarmingDenial game)
        {
            this.game = game;
        }

        public void Draw(SpriteBatch batch)
        {
            float playerVelocity = game.player.velocity.Y;
            batch.Draw(tv, new Vector2(0, (playerVelocity / 5) - 10), Color.White);

            batch.DrawString(game.courier, game.depth + "m", new Vector2(53, 135), Color.White);
            batch.DrawString(game.courier, game.money + "$", new Vector2(140, 135), Color.White);

            // Don't draw the hull progress bar if there isn't
            if (game.hull <= 0) return;

            // Draw the hull progress bar depending on how much hull there is
            for (int x = 58; x < game.hull + 85; x += 15)
            {
                batch.Draw(bar, new Vector2(x, 79), Color.White);
            }
        }
    }
}
