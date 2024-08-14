using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.ServiceModel;
using GameServer;

namespace GameClient
{
    /// <summary>
    /// Interaction logic for loginWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ServerInterface foob;
        public MainWindow()
        {
            InitializeComponent();

            ChannelFactory<ServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            //Set the URL and create the connection!
            string URL = "net.tcp://localhost:8100/GameService";
            foobFactory = new ChannelFactory<ServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
            //Also, tell me how many entries are in the DB.
            userNumber.Text = "Number of Users: " + foob.getAllUsers().Count(); 
            
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            bool uniquename = false;
            string userName = userNameBox.Text;

            // check here is user name is unique
            uniquename = true;//change later to check if name is valid and then set

            if (uniquename)
            {
                // open main window and close this one
                // send across user name as well
                lobbyFinderWindow curWindow = new lobbyFinderWindow(userName);
                curWindow.Show();
                this.Close();

            }
            else
            {
                userNameBox.Clear();
                MessageBox.Show("Name is taken, please try again");
                
            }
        }
    }
}
