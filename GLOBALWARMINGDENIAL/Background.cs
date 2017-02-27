using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLOBALWARMINGDENIAL
{
    // An infinitely scrolling background
    public class Background
    {
        public Texture2D texture;
        public GlobalWarmingDenial game;

        // Position of first background
        Vector2 position1 = new Vector2(0, 0);
        Vector2 position2 = new Vector2(0, 720);

        public Background(GlobalWarmingDenial game)
        {
            this.game = game;
        }

        public void Draw (SpriteBatch batch)
        {
            // When one of the backgrounds reaches the top of the screen, put it at the bottom
            if (position1.Y + 720 + game.camera.Y / 2 < 0) position1.Y = position2.Y + 720;
            if (position2.Y + 720 + game.camera.Y / 2 < 0) position2.Y = position1.Y + 720;

            // Render both of the backgrounds
            batch.Draw(texture, position1 + game.camera / 2, Color.White);
            batch.Draw(texture, position2 + game.camera / 2, Color.White);
        }
    }
}
