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
        private string currUser;
        private string mode;
        private string desc;
        private string roomName;
        private List<string> tags;
        private IBusinessServerInterface foob;

        public createLobbyWindow()
        {
            InitializeComponent();
            this.foob = App.Instance.foob;
            this.currUser = App.Instance.UserName;

            // Call async method after the constructor
            Task task = InitializeLobbyCreationAsync();
        }

        // Make this method async to use await inside
        private async Task InitializeLobbyCreationAsync()
        {
            // Async calls to populate mode and tag boxes
            modeSelBox.ItemsSource = await foob.GetAllModeTypesAsync();
            optionsBox.ItemsSource = await foob.GetAllTagTypesAsync();
        }

        private async void submitBtn_Click(object sender, RoutedEventArgs e)
        {
            mode = modeSelBox.SelectedItem as string;
            tags = optionsBox.SelectedItems.Cast<string>().ToList();

            string tagString = string.Join(",", tags); // Concatenate tags with commas
            roomName = nameTxtBox.Text;
            desc = descTxtBox.Text;
            MessageBox.Show(mode + " " + tagString + " " + roomName + " " + desc);

            // Async call to add lobby
            await foob.AddLobbyAsync(roomName, desc, mode, tags);

            lobbyRoomWindow curWindow = new lobbyRoomWindow(roomName);
            curWindow.Show();
            this.Close();
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
            lobbyFinderWindow curWindow = new lobbyFinderWindow();
            curWindow.Show();
            this.Close();
        }
    }
}
