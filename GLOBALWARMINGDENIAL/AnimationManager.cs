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
    public class AnimationManager
    {
        Dictionary<string, Animation> animations = new Dictionary<string, Animation>();
        Animation currentAnimation = null;
        public bool isPlaying = false;
        public bool isLooping = true;

        public void Load(string name, ContentManager content, int cellWidth, int cellHeight)
        {
            Animation anim = new Animation();
            anim.Load(name, content, cellWidth, cellHeight);

            animations.Add(name, anim);
        }

        public void Play(string name)
        {
            Animation anim = animations[name];

            if (anim != null)
            {
                currentAnimation = anim;
                isPlaying = true;
            }
        }

        public void Update ()
        {
            if (isPlaying && currentAnimation != null) currentAnimation.Update(isLooping);
        }
        public void DrawCurrentFrame(SpriteBatch batch, Vector2 position, Color color)
        {
            if (isPlaying && currentAnimation != null) currentAnimation.DrawCurrentFrame(batch, position, color);
        }

        public void Stop()
        {
            isPlaying = false;
            currentAnimation = null;
        }
    }
}
