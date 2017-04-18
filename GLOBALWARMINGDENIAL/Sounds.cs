using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLOBALWARMINGDENIAL
{
    public class Sounds
    {
        public SoundEffect fireSound;
        public SoundEffectInstance fireSoundInstance;

        public List<SoundEffect> tileBreakSounds = new List<SoundEffect>();
        public List<SoundEffect> dirtDigSounds = new List<SoundEffect>();
        public List<SoundEffect> metalDigSounds = new List<SoundEffect>();
        Random rng = new Random();

        public enum SoundType
        {
            TILE_BREAKING, DIRT_DIG, METAL_DIG
        }

        // Play a random sound from the sound type specified
        public void PlaySound(SoundType sound)
        {
            switch (sound)
            {
                case SoundType.TILE_BREAKING:
                    ChooseRandomSound(tileBreakSounds).Play();
                    break;
                case SoundType.DIRT_DIG:
                    ChooseRandomSound(dirtDigSounds).Play();
                    break;
                case SoundType.METAL_DIG:
                    SoundEffectInstance inst = ChooseRandomSound(metalDigSounds).CreateInstance();
                    float vol = (float)rng.NextDouble() - 0.5f;
                    if (vol < 0.1f) vol = 0.1f;
                    inst.Volume = vol;
                    inst.Play();
                    break;
            }
        }

        // Update the fire sound volume based on how close the fire is
        public void AdjustFireSoundVolume(float volume)
        {
            if (volume > 1) volume = 1f;
            fireSoundInstance.Volume = volume;
        }

        // Pick a random sound from the specified list
        SoundEffect ChooseRandomSound(List<SoundEffect> soundList)
        {
            int choice = rng.Next(soundList.Count);
            return soundList[choice];
        }

        public void Load(ContentManager content)
        {
            // Set up the continous fire sound effect
            fireSound = content.Load<SoundEffect>("sounds/fire");
            fireSound.CreateInstance().IsLooped = true;
            fireSoundInstance = fireSound.CreateInstance();
            fireSoundInstance.IsLooped = true;
            fireSoundInstance.Play();

            dirtDigSounds.Add(content.Load<SoundEffect>("sounds/digdirt1"));
            dirtDigSounds.Add(content.Load<SoundEffect>("sounds/digdirt2"));
            dirtDigSounds.Add(content.Load<SoundEffect>("sounds/digdirt3"));

            tileBreakSounds.Add(content.Load<SoundEffect>("sounds/break1"));
            tileBreakSounds.Add(content.Load<SoundEffect>("sounds/break2"));
            tileBreakSounds.Add(content.Load<SoundEffect>("sounds/break3"));

            metalDigSounds.Add(content.Load<SoundEffect>("sounds/metal1"));
            metalDigSounds.Add(content.Load<SoundEffect>("sounds/metal2"));
            metalDigSounds.Add(content.Load<SoundEffect>("sounds/metal3"));
        }
    }
}
