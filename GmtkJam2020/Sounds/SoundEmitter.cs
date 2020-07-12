using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace GmtkJam2020.Sound
{
    class SoundEmitter
    {
        List<SoundData> possibleSounds;

        static List<SoundEffectInstance> soundEffectInstances;

        public static void InitSystem()
        {
            soundEffectInstances = new List<SoundEffectInstance>();
        }

        public SoundEmitter(List<SoundData> soundFiles)
        {
            possibleSounds = soundFiles;
        }

        public void PlaySound(string soundName, float volume, bool loop)
        {
            if(possibleSounds.Any(s => s.Name == soundName))
            {
                soundEffectInstances.Add(possibleSounds.Find(s => s.Name == soundName).SoundEffect.CreateInstance());
                soundEffectInstances[soundEffectInstances.Count - 1].Volume = volume;
                soundEffectInstances[soundEffectInstances.Count - 1].IsLooped = loop;
            }
        }

        public static void PlayAllSounds(bool val)
        {
            for (int i = 0; i < soundEffectInstances.Count; i++)
            {
                if (val)
                {
                    if (soundEffectInstances[i].State == SoundState.Paused)
                    {
                        soundEffectInstances[i].Play();
                    }
                }
                else
                {
                    if (soundEffectInstances[i].State == SoundState.Playing)
                    {
                        soundEffectInstances[i].Pause();
                    }
                }
            }
        }

        public static void EndSounds()
        {
            while(soundEffectInstances.Count > 0)
            {
                soundEffectInstances[0].Dispose();
                soundEffectInstances.RemoveAt(0);
            }
        }

        public static void UpdateSounds()
        {
            for (int i = 0; i < soundEffectInstances.Count; i++)
            {
                if(soundEffectInstances[i].State == SoundState.Stopped)
                {
                    soundEffectInstances[i].Dispose();
                    soundEffectInstances.RemoveAt(i);
                    i--;
                }
            }
        }

        public struct SoundData
        {
            public string Name { get; set; }

            public SoundEffect SoundEffect { get; set; }
        }
    }
}
