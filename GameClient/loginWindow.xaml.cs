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
using GameLobbyLib;
using System.Configuration;

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
            userNumber.Text = "Number of Users: " + foob.GetAllUsers().Count(); 
            
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            bool uniquename = true;
            string userName = userNameBox.Text;

            // check here is user name is unique
            List<User> users = foob.GetAllUsers();
            foreach (User user in users) { }
            {
                foreach (User user in users)
                {
                    if (user.Name == userName)
                    {
                        uniquename = false;
                    }
                }
            }

            if (uniquename)
            {
                // open main window and close this one
                // send across user name as well
                foob.AddUser(new User(userName));
                lobbyFinderWindow curWindow = new lobbyFinderWindow(userName, foob);
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
