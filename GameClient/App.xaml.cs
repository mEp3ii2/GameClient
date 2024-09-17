using BusinessLayer;
using System;
using System.ComponentModel;
using System.Media;
using System.ServiceModel;
using System.Windows;
using GameLobbyLib;

namespace GameClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static App Instance => ((App)Application.Current);  // Provides a static instance for easy access
        public string UserName { get; set; }  // Stores the current user's name
        public IBusinessServerInterface foob; // The client-side interface for communicating with the business layer


        protected override void OnStartup(StartupEventArgs e)
        {
            UserName = null;  // Initialize UserName to null
            base.OnStartup(e);  // Call the base class's OnStartup method

            // Load and play the background music in a loop
            SoundPlayer player = new SoundPlayer("Resources/Mortal_Kombat.wav");
            player.LoadCompleted += delegate (object sender, AsyncCompletedEventArgs er) {
                player.PlayLooping();  // Loop the music once it’s loaded
            };
            player.LoadAsync();  // Load the sound file asynchronously

            // Create a new NetTcpBinding using the BindingHelper to allow large file transfers
            NetTcpBinding tcp = BindingHelper.CreateLargeFileBinding();

            // Use the ChannelFactory to create a connection to the business server interface
            string URL = "net.tcp://localhost:8100/GameService";

            // Define the foobFactory only once and assign the ChannelFactory to it
            ChannelFactory<IBusinessServerInterface> foobFactory = new ChannelFactory<IBusinessServerInterface>(tcp, URL);

            // Create the communication channel for the client
            foob = foobFactory.CreateChannel();
        }

        // This method will be called when the application exits, removing the user from the system
        private void app_Exit(object sender, ExitEventArgs e)
        {
            foob.RemoveUser(UserName);  // Call the server to remove the user on exit
            MessageBox.Show("Goodbye");  // Display a goodbye message to the user
        }
    }
}
