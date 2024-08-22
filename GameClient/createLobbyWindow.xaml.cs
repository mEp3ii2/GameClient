using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GameLobbyLib;
using BusinessLayer;

namespace GameClient
{
    /// <summary>
    /// Interaction logic for createLobbyWindow.xaml
    /// </summary>
    public partial class createLobbyWindow : Window
    {
        private User currUser;
        private string mode;
        private string desc;
        private string roomName;
        private List<string> tags;
        private BusinessServerInterface foob;
        
        public createLobbyWindow(User currUser)
        {
            InitializeComponent();

            ChannelFactory<BusinessServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            string URL = "net.tcp://localhost:8100/GameService";
            foobFactory = new ChannelFactory<BusinessServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();

            this.currUser = currUser;
            
            modeSelBox.ItemsSource = foob.GetAllModeTypes();
            //tagSelBox.ItemsSource = Database.getAllTagTypes();
            

            optionsBox.ItemsSource = foob.GetAllTagTypes();
        }

        private void submitBtn_Click(object sender, RoutedEventArgs e)
        {
            mode = modeSelBox.SelectedItem as string;
            List<string> temp = new List<string>();
            
            tags = new List<string>();
            tags = optionsBox.SelectedItems.Cast<string>().ToList();
            
            string tagString= "";
            foreach (string tag in tags)
            {
                tagString = tagString+tag;
            }
            roomName = nameTxtBox.Text;
            desc = descTxtBox.Text;
            MessageBox.Show(mode+" "+tagString + " " +roomName + " " + desc);

            foob.AddLobby(new Lobby(roomName, currUser, roomName, desc, mode, tags));
        }

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            nameTxtBox.Clear();
            modeSelBox.SelectedItem = null;
            descTxtBox.Clear();
            optionsBox.SelectedItems.Clear();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            lobbyFinderWindow curWindow = new lobbyFinderWindow(currUser);
            curWindow.Show();
            this.Close();
        }
    }
}
