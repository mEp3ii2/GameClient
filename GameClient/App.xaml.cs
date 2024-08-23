using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Windows;

namespace GameClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SoundPlayer player = new SoundPlayer("Mortal_Kombat.wav");
            player.LoadCompleted += delegate (object sender, AsyncCompletedEventArgs er) {
                player.PlayLooping();
            };
            player.LoadAsync();

            /*var player = musicplayer.Instance;
            string musicPath = "Mortal_Kombat.mp3";
            player.PlayMusic(musicPath);*/

        }
        private void app_Exit(object sender, ExitEventArgs e)
        {
            MessageBox.Show("Goodbye");
        }
    }
}
