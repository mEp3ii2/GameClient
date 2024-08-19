using GameLobbyLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

namespace GameClient
{
    /// <summary>
    /// Interaction logic for lobbyRoomWindow.xaml
    /// </summary>
    public partial class lobbyRoomWindow : Window
    {
        private ObservableCollection<string> messages;//makes it so that the list box is notified when changes happen
        private string userName;
        public lobbyRoomWindow(Lobby selectedLobby, string userName) 
        {
            
            InitializeComponent();
            this.userName = userName;
            //get all users to populate user box
            //load current chat history
            messages = new ObservableCollection<string>();
            messageBox.ItemsSource = messages;
            
        }

       

        private void logOutBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            lobbyFinderWindow curWindow = new lobbyFinderWindow(userName);
            this.Close();
        }

        private void messageBtn_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(userName);
            //MessageBox.Show(userMessageBox.Text.ToString());
            //messages.Append(userName + ": " + userMessageBox.Text.ToString());
            messages.Add(userName + ": " + userMessageBox.Text.ToString());
            userMessageBox.Clear();
        }
    }
}
