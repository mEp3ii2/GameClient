using BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Media;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;

namespace GameClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static App Instance => ((App)Application.Current);
        public string UserName { get; set; }
        public IBusinessServerInterface foob;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            await InitializeConnectionAsync();
        }

        private async Task InitializeConnectionAsync()
        {
            ChannelFactory<IBusinessServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            string URL = "net.tcp://localhost:8100/GameService";
            foobFactory = new ChannelFactory<IBusinessServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();

            // Add a simple awaitable test call to check the connection
            await foob.GetUserCountAsync();
        }

        private async Task InitializeSoundAsync()
        {
            SoundPlayer player = new SoundPlayer("Resources/Mortal_Kombat.wav");
            player.LoadCompleted += delegate (object sender, AsyncCompletedEventArgs er)
            {
                player.PlayLooping();
            };
            await Task.Run(() => player.LoadAsync());
        }

        private void app_Exit(object sender, ExitEventArgs e)
        {
            if (!string.IsNullOrEmpty(UserName))
            {
                foob.RemoveUserAsync(UserName);  // Make sure this only runs on actual app exit
            }
            MessageBox.Show("Goodbye");
        }
    }
}
