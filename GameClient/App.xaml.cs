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


        protected override void OnStartup(StartupEventArgs e)
        {
            UserName = null;
            base.OnStartup(e);
            SoundPlayer player = new SoundPlayer("Resources/Mortal_Kombat.wav");
            player.LoadCompleted += delegate (object sender, AsyncCompletedEventArgs er) {
             //   player.PlayLooping();
            };
            player.LoadAsync();

            ChannelFactory<IBusinessServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            // Set the message and buffer size limits for larger file transfers
            
            tcp.MaxReceivedMessageSize = 52428800;  // 50 MB
            tcp.MaxBufferSize = 52428800;
            tcp.MaxBufferPoolSize = 52428800;

            // Set reader quotas for large messages
            tcp.ReaderQuotas.MaxArrayLength = 52428800;
            tcp.ReaderQuotas.MaxStringContentLength = 52428800;
            tcp.ReaderQuotas.MaxBytesPerRead = 8192;
            tcp.ReaderQuotas.MaxDepth = 64;


            // Set timeouts to handle long-running operations
            tcp.OpenTimeout = TimeSpan.FromMinutes(1);
            tcp.CloseTimeout = TimeSpan.FromMinutes(1);
            tcp.ReceiveTimeout = TimeSpan.FromMinutes(1); 
            tcp.SendTimeout = TimeSpan.FromMinutes(1);    

            string URL = "net.tcp://localhost:8100/GameService";
            foobFactory = new ChannelFactory<IBusinessServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();


        }
        private void app_Exit(object sender, ExitEventArgs e)
        {
            foob.RemoveUser(UserName);
            MessageBox.Show("Goodbye");
        }
    }
}
