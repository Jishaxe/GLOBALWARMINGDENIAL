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
    // Represented an animation loaded from a single-row spritesheet.
    public class Animation
    {
        string name;
        int frame = 0;
        List<Rectangle> frames = new List<Rectangle>();
        Texture2D spritesheet;
        int cellWidth;
        int cellHeight;

        public void Load(string name, ContentManager content, int cellWidth, int cellHeight)
        {
            this.name = name;
            spritesheet = content.Load<Texture2D>(name);

            this.cellWidth = cellWidth;
            this.cellHeight = cellHeight;

            for (int x = 0; x < spritesheet.Width; x += cellWidth) frames.Add(new Rectangle(x, 0, cellWidth, cellHeight));
        }

        public void Update (bool isLooping)
        {
            // Should probably compensate for variable time step at some point

            frame++;
            if (frame == frames.Count)
            {
                if (isLooping) frame = 0;
                else frame--;
            }
        }
        
        public void DrawCurrentFrame(SpriteBatch batch, Vector2 position, Color color)
        {
            Rectangle currentFrame = frames[frame];
            batch.Draw(spritesheet, new Rectangle((int)position.X, (int)position.Y, cellWidth, cellHeight), currentFrame, color);
        }
    }
}
