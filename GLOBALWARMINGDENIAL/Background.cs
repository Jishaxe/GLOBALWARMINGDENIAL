using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLOBALWARMINGDENIAL
{
    public class Background
    {
        public Texture2D repeat;

        Vector2 position1 = new Vector2();
        Vector2 position2 = new Vector2();

        public void Draw (SpriteBatch batch, GraphicsDevice graphics)
        {
            position1.Y += 1;
            position2.Y += 1;

            if (position1.Y > graphics.Viewport.Height) position1.Y = -graphics.Viewport.Height;
            if (position2.Y > graphics.Viewport.Height) position2.Y = -graphics.Viewport.Height;

            for (int x = 0; x < graphics.Viewport.Width; x += repeat.Width)
            {
                for (int y = 0; y < graphics.Viewport.Height; y += repeat.Height)
                {
                    batch.Draw(repeat, new Vector2(position1.X + x, position1.Y + y), Color.White);
                    batch.Draw(repeat, new Vector2(position2.X + x, position2.Y + y), Color.White);
                }
            }
        }
    }
}
