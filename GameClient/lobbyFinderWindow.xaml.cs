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
        private Lobby selectedLobby;
        private System.Timers.Timer lobbyUpdateTimer;

        public lobbyFinderWindow()
        {
            InitializeComponent();
            this.foob = App.Instance.foob;
            this.currUser = App.Instance.UserName; // Assign the current user

            // Load filters as soon as the window opens
            Task.Run(async () => {
                await loadModeFilterBoxAsync();
                await loadTagFilterBoxAsync();
            });

            StartLobbyUpdateTimer(); // Start polling for lobbies
        }

        private void StartLobbyUpdateTimer()
        {
            lobbyUpdateTimer = new System.Timers.Timer(1000); // Update every second
            lobbyUpdateTimer.Elapsed += async (sender, e) =>
            {
                await LoadLobbiesAsync(); // Load lobbies asynchronously
            };
            lobbyUpdateTimer.AutoReset = true;
            lobbyUpdateTimer.Enabled = true;
        }

        private async Task LoadLobbiesAsync()
        {
            List<Lobby> lobbies;

            // Check if any filters are applied
            if (!string.IsNullOrEmpty(currentModeFilter) || !string.IsNullOrEmpty(currentTagFilter))
            {
                // Apply filters when reloading the lobby list
                lobbies = await foob.GetFilteredLobbiesListAsync(currentModeFilter, currentTagFilter);
            }
            else
            {
                // If no filters are applied, load all lobbies
                lobbies = await foob.GetAllLobbiesAsync();
            }

            // Ensure UI update is done on the main thread
            Dispatcher.Invoke(() =>
            {
                if (lobbies != null && lobbies.Any())
                {
                    lobbyList.ItemsSource = lobbies; // Update lobby list in the UI
                    updateLobbyCountLabel(lobbies.Count); // Update lobby count
                }
                else
                {
                    lobbyList.ItemsSource = null;  // Clear the lobby list if no lobbies are found
                    updateLobbyCountLabel(0); // Update lobby count to 0 if no lobbies
                }
            });
        }

        private async Task loadModeFilterBoxAsync()
        {
            List<string> modelist = new List<string> { "" };
            List<string> modes = await foob.GetUniqueModesAsync(null);
            modelist.AddRange(modes);
            Dispatcher.Invoke(() => {
                modeFilterBox.ItemsSource = modelist;
            });
        }

        private async Task loadTagFilterBoxAsync()
        {
            List<string> tagList = new List<string> { "" };
            List<string> tags = await foob.GetUniqueTagsAsync(null);
            tagList.AddRange(tags);
            Dispatcher.Invoke(() => {
                tagFilterBox.ItemsSource = tagList;
            });
        }

        private void updateLobbyCountLabel(int lobbyCount)
        {
            lobbyCountLabel.Content = $"Active Lobbies: {lobbyCount}";
        }

        private void lobbyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lobbyList.SelectedItem == null)
            {
                return;  // No selection made, so return early
            }

            // Retrieve the selected lobby and store it in the selectedLobby field
            selectedLobby = lobbyList.SelectedItem as Lobby;

            if (selectedLobby != null)
            {
                // Stop any timers or background operations related to lobbyFinderWindow
                StopLobbyUpdateTimer();

                // Open the lobby room window for the selected lobby
                lobbyRoomWindow lobbyRoom = new lobbyRoomWindow(selectedLobby.Name);

                // Safely close the lobbyFinderWindow
                this.Close();

                // Show the new window
                lobbyRoom.Show();
            }
        }

        private void StopLobbyUpdateTimer()
        {
            if (lobbyUpdateTimer != null)
            {
                lobbyUpdateTimer.Stop();
                lobbyUpdateTimer.Dispose();
                lobbyUpdateTimer = null;
            }
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

            // Reload the filters after clearing
            Task.Run(async () => {
                await loadModeFilterBoxAsync();
                await loadTagFilterBoxAsync();
            });
        }

        private async Task setLobbyListAsync()
        {
            currentList = await foob.GetFilteredLobbiesListAsync(currentModeFilter, currentTagFilter);
            lobbyList.ItemsSource = currentList;
            updateLobbyCountLabel(currentList.Count); // Update the lobby count with the filtered list
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

        private async void lobbyFinderWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(currUser) && foob != null && selectedLobby != null)
            {
                await foob.RemoveUserFromLobbyAsync(selectedLobby.Name, currUser);
            }
        }
    }
}
