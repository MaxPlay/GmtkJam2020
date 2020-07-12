using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace GmtkJam2020.Sounds
{
    static class SoundManager
    {
        static List<SongData> possibleSongs;

        static List<SoundData> possibleSounds;

        static List<SoundEffectInstance> soundEffectInstances;

        static float volume = 1;

        static string[] soundFiles = new string[4]
        {
            "Step",
            "Drill",
            "StartGrapple",
            "EndGrapple"
        };

        static string[] musicFiles = new string[2]
        {
            "TitleScreen_Loop",
            "LevelMusic"
        };

        static void CheckLists()
        {
            if (possibleSounds == null)
                possibleSounds = new List<SoundData>();
            if (soundEffectInstances == null)
                soundEffectInstances = new List<SoundEffectInstance>();
            if (possibleSongs == null)
                possibleSongs = new List<SongData>();
        }

        public static void LoadFiles()
        {
            MediaPlayer.IsRepeating = true;
            for (int i = 0; i < soundFiles.Length; i++)
            {
                LoadSound(soundFiles[i]);
            }
            for (int i = 0; i < musicFiles.Length; i++)
            {
                LoadMusic(musicFiles[i]);
            }
        }

        static void LoadMusic(string name)
        {
            CheckLists();
            if (!possibleSounds.Any(s => s.Name == name))
            {
                SongData soundData = new SongData();
                soundData.Song = GameCore.Instance.Content.Load<Song>("Music/" + name);
                soundData.Name = name;
                possibleSongs.Add(soundData);
            }
        }

        public static void PlayMusic(string name)
        {
            CheckLists();
            if (MediaPlayer.State == MediaState.Playing)
                MediaPlayer.Stop();
            if (possibleSongs.Any(s => s.Name == name))
            {
                MediaPlayer.Play(possibleSongs.Find(s => s.Name == name).Song);
            }
        }

        static void LoadSound(string name)
        {
            CheckLists();
            if (!possibleSounds.Any(s => s.Name == name))
            {
                SoundData soundData = new SoundData();
                soundData.SoundEffect = GameCore.Instance.Content.Load<SoundEffect>("Sfx/" + name);
                soundData.Name = name;
                possibleSounds.Add(soundData);
            }
        }

        public static void PlaySound(string soundName, bool loop)
        {
            CheckLists();
            if (possibleSounds.Any(s => s.Name == soundName))
            {
                soundEffectInstances.Add(possibleSounds.Find(s => s.Name == soundName).SoundEffect.CreateInstance());
                soundEffectInstances[soundEffectInstances.Count - 1].Volume = volume;
                soundEffectInstances[soundEffectInstances.Count - 1].IsLooped = loop;
                soundEffectInstances[soundEffectInstances.Count - 1].Play();
            }
        }

        public static void PlayAllSounds(bool val)
        {
            CheckLists();
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

        public static void SetVolume(float newVolume)
        {
            CheckLists();
            if (volume != newVolume)
            {
                for (int i = 0; i < soundEffectInstances.Count; i++)
                {
                    soundEffectInstances[i].Volume = newVolume;
                }
                MediaPlayer.Volume = newVolume;
                volume = newVolume;
            }
        }

        public static void EndSounds()
        {
            CheckLists();
            while (soundEffectInstances.Count > 0)
            {
                soundEffectInstances[0].Dispose();
                soundEffectInstances.RemoveAt(0);
            }
        }

        public static void UpdateSounds()
        {
            CheckLists();
            for (int i = 0; i < soundEffectInstances.Count; i++)
            {
                if (soundEffectInstances[i].State == SoundState.Stopped)
                {
                    soundEffectInstances[i].Dispose();
                    soundEffectInstances.RemoveAt(i);
                    i--;
                }
            }
        }

        public struct SongData
        {
            public string Name { get; set; }

            public Song Song { get; set; }
        }

        public struct SoundData
        {
            public string Name { get; set; }

            public SoundEffect SoundEffect { get; set; }
        }
    }
}
