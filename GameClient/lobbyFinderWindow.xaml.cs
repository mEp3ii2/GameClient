using System;
using System.Collections.Generic;
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
using System.ComponentModel;
using GameLobbyLib;
using BusinessLayer;
using System.ServiceModel;

namespace GameClient
{
    /// <summary>
    /// Interaction logic for lobbyWindow.xaml
    /// </summary>
    public partial class lobbyFinderWindow : Window
    {
        private string currUser;
        private List<Lobby> currentList; // list of lobbies retrieved from business layer
        private string currentModeFilter;
        private string currentTagFilter;
        private IBusinessServerInterface foob;

        public lobbyFinderWindow()
        {
            InitializeComponent();
            this.foob = App.Instance.foob; // connection to business layer

            this.currUser = App.Instance.UserName;
            Task task = LoadLobbiesAsync();
            Task task1 = loadModeFilterBoxAsync();
            Task task2 = loadTagFilterBoxAsync();
            currentModeFilter = null;
            currentTagFilter = null;
        }

        private async Task LoadLobbiesAsync()
        {
            currentList = await foob.GetAllLobbiesAsync();
            lobbyList.ItemsSource = currentList;
            updateLobbyCountLabel(currentList.Count);
        }

        private async Task loadModeFilterBoxAsync()
        {
            List<string> modelist = new List<string>();
            modelist.Add("");
            List<string> modes = await foob.GetUniqueModesAsync(null);
            modelist.AddRange(modes);
            modeFilterBox.ItemsSource = modelist;
        }

        private async Task loadTagFilterBoxAsync()
        {
            List<string> tagList = new List<string>();
            tagList.Add("");
            List<string> tags = await foob.GetUniqueTagsAsync(null);
            tagList.AddRange(tags);
            tagFilterBox.ItemsSource = tagList;
        }

        /*private void refreshBtn_click(object sender, EventArgs e)
        {
            LoadLobbiesAsync();
            loadModeFilterBoxAsync();
            loadTagFilterBoxAsync();
            currentModeFilter = null;
            currentTagFilter = null;
        }*/

        private void updateLobbyCountLabel(int lobbyCount)
        {
            lobbyCountLabel.Content = $"Active Lobbies: {lobbyCount}";
        }

        private void lobbyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedLobby = ((Lobby)lobbyList.SelectedItem).Name;
            foob.JoinLobbyAsync(selectedLobby, currUser);  // Correct method name
            lobbyRoomWindow curWindow = new lobbyRoomWindow(selectedLobby);
            curWindow.Show();
            this.Close();
        }

        private void createBtn_Click(object sender, RoutedEventArgs e)
        {
            createLobbyWindow curWindow = new createLobbyWindow();
            curWindow.Show();
            this.Close();
        }

        private async void modeFilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (modeFilterBox.SelectedItem.ToString() == "")
            {
                currentModeFilter = null;
            }
            else
            {
                currentModeFilter = modeFilterBox.SelectedItem.ToString();
            }
            await setLobbyListAsync();
            await updateFilterListAsync(1);
        }

        private async void tagFilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tagFilterBox.SelectedItem.ToString() == "")
            {
                currentTagFilter = null;
            }
            else
            {
                currentTagFilter = tagFilterBox.SelectedItem.ToString();
            }
            await setLobbyListAsync();
            await updateFilterListAsync(2);
        }

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            Task task = LoadLobbiesAsync();
            modeFilterBox.SelectedItem = "";
            tagFilterBox.SelectedItem = "";
        }

        private async Task setLobbyListAsync()
        {
            currentList = await foob.GetFilteredLobbiesListAsync(currentModeFilter, currentTagFilter);
            lobbyList.ItemsSource = currentList;
        }

        private async Task updateFilterListAsync(int i)
        {
            if (i == 1)
            {
                List<string> tagList = new List<string>();
                tagList.Add("");
                List<string> tags = await foob.GetUniqueTagsAsync(currentList);
                tagList.AddRange(tags);
                tagFilterBox.ItemsSource = tagList;
            }
            else
            {
                List<string> modelist = new List<string>();
                modelist.Add("");
                List<string> modes = await foob.GetUniqueModesAsync(currentList);
                modelist.AddRange(modes);
                modeFilterBox.ItemsSource = modelist;
            }
        }

        private void logOutBtn_Click(object sender, EventArgs e)
        {
            foob.RemoveUserAsync(currUser);
            this.Close();
        }

        private void lobbyFinderWindow_Closing(object sender, CancelEventArgs e)
        {
            App.Instance.app_Exit(sender, null);  // Call the App-level exit handling.
        }
    }
}
