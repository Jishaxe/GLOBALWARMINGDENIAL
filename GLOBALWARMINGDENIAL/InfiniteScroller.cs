using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLOBALWARMINGDENIAL
{
    // An infinitely scrolling texture
    // Textures here will scroll vertically
    public class InfiniteScroller
    {
        public Texture2D texture;
        public GlobalWarmingDenial game;

        // Position of the two textures
        Vector2 position1;
        Vector2 position2;

        // The height of the texture and the x position
        int height;

        // The x position of this scroller
        int x;

        // The parralax factor
        float factor;

        public void Reset()
        {
            position1 = new Vector2(x, 0);
            position2 = new Vector2(x, height);
        }

        public InfiniteScroller(GlobalWarmingDenial game, int x, float factor, int height)
        {
            this.game = game;
            this.height = height;
            this.factor = factor;
            this.x = x;

            position1 = new Vector2(x, 0);
            position2 = new Vector2(x, height);
        }

        public void Draw (SpriteBatch batch)
        {
            // When one of the textures reaches the top of the screen, put it at the bottom
            if (position1.Y + height + game.cameraTranslation.Y / factor < 0) position1.Y = position2.Y + height;
            if (position2.Y + height + game.cameraTranslation.Y / factor < 0) position2.Y = position1.Y + height;

            // Render both of the textures
            batch.Draw(texture, position1 + game.cameraTranslation / factor, Color.White);
            batch.Draw(texture, position2 + game.cameraTranslation / factor, Color.White);
        }
    }
}
