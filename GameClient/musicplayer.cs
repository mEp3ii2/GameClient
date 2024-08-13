using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GameClient
{
    internal class musicplayer
    {
        private static musicplayer player;
        public MediaElement media { get; private set; }

        private musicplayer()
        {
            media = new MediaElement();
            {
                media.LoadedBehavior = MediaState.Manual;
                media.UnloadedBehavior = MediaState.Stop;
                media.Volume = 1.0;
            }
        }

        public static musicplayer Instance
        {
            get
            {
                if(player == null)
                {
                    player = new musicplayer();
                }
                return player;
            }
        }

        public void PlayMusic(string filePath)
        {
            media.Source= new Uri(filePath, UriKind.Relative);
            media.Play();
        }

        public void StopMusic()
        {
            media.Stop();
        }
    }
}
