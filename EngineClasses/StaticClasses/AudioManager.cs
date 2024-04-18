using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphicsArchitecture.EngineClasses.StaticClasses
{
    static internal class AudioManager
    {

        static Dictionary<string,Song> BackgroundSongs = new Dictionary<string,Song>();
        static Dictionary<string,SoundEffect> SFX= new Dictionary<string,SoundEffect>();
        
        public static void AddBGMusic(string location)
        {
            Song bg = FileReader.ReadContent<Song>(location);
            if (BackgroundSongs.ContainsKey(location))
                BackgroundSongs[location] = bg;
            else
                BackgroundSongs.Add(location, bg);
        }

        public static void RemoveBGMusic(string location)
        {
            if (BackgroundSongs.ContainsKey(location))
                BackgroundSongs.Remove(location);
        }

        public static void AddSFX(string location)
        {
            SoundEffect sfx = FileReader.ReadContent<SoundEffect>(location);
            if (SFX.ContainsKey(location))
                SFX[location] = sfx;
            else
                SFX.Add(location, sfx);
        }

        public static void PlayBGMusic(string location,bool isRepeating)
        {
            if (!BackgroundSongs.ContainsKey(location))
                return;
            else
            {
                try
                {
                    MediaPlayer.IsRepeating = isRepeating;
                    MediaPlayer.Play(BackgroundSongs[location]);
                }
                catch { }
               
               
            }
        }

        public static void PlaySFX(string location)
        {
            if (!SFX.ContainsKey(location))
                return;
            else
                SFX[location].Play();
        }
    }
}
