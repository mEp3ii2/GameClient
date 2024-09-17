/**
Names: L Kipling (20899932), R Mackintosh (21171466), M Pontague (19126924)
Date: 17 September 2024
Class: lobbyFinderWindow
Purpose: Manages the lobby finder window where users can view and filter available gaming lobbies, create new lobbies, and join existing ones.
Notes: This class communicates with the business server to manage the retrieval, filtering, and joining of lobbies in real-time. It also periodically refreshes the lobby list using a Timer.
*/

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
using System.Threading;
using System.Xml.Serialization;

namespace GameClient
{
    /// <summary>
    /// Interaction logic for lobbyWindow.xaml
    /// </summary>
    public partial class lobbyFinderWindow : Window
    {
        private string currUser;
        private List<Lobby> currentList;  // list of lobbies retrieved from business layer
        private string currentModeFilter;
        private string currentTagFilter;
        private IBusinessServerInterface foob;
        private Timer timer;

        /**
        Method: lobbyFinderWindow (Constructor)
        Imports: None
        Exports: None
        Notes: Initializes the lobby finder window and sets up the communication with the business server. It retrieves the available lobbies and initializes the filters for modes and tags.
        Algorithm: Loads the current lobbies, sets up filtering options for modes and tags, and initializes the Timer for periodic refresh of the lobby list.
        */
        public lobbyFinderWindow()
        {
            InitializeComponent();
            this.foob = App.Instance.foob;  // connection to business layer

            this.currUser = App.Instance.UserName;
            currentList = foob.GetAllLobbies();
            lobbyList.ItemsSource = currentList;
            loadModeFilterBox();
            loadTagFilterBox();
            currentModeFilter = null;
            currentTagFilter = null;
            updateLobbyCountLabel(currentList.Count);

            timer = new Timer(Refresh);
            timer.Change(0, 250);
        }

        /**
        Method: Refresh
        Imports: stateInfo (Object)
        Exports: None
        Notes: Periodically refreshes the lobby list by fetching updated data from the server. Also updates the mode and tag filters.
        Algorithm: Retrieves the latest lobby list, available modes, and tags from the server and updates the UI accordingly.
        */
        private void Refresh(Object stateInfo)
        {
            currentList = foob.GetfilterdLobbiesList(mode: currentModeFilter, tag: currentTagFilter);

            UpdateGUI(currentList.Count);
        }

        /**
        Method: UpdateGUI
        Imports: modelist (List<string>), tagList (List<string>), lobbyCount (int)
        Exports: None
        Notes: Updates the user interface with the latest list of lobbies, modes, and tags.
        Algorithm: Uses the Dispatcher to invoke the update on the UI thread, updating the lobby list, mode filter, and tag filter.
        */
        private void UpdateGUI(int lobbyCount)
        {
            this.Dispatcher.Invoke(() =>
            {
                lobbyList.ItemsSource = currentList;
                lobbyCountLabel.Content = $"Active Lobbies: {lobbyCount}";
            });
        }

        /**
        Method: refreshBtn_click
        Imports: sender (object), e (EventArgs)
        Exports: None
        Notes: Manually refreshes the lobby list and resets the filters.
        Algorithm: Fetches the latest lobby list and reloads the mode and tag filters.
        */
        private void refreshBtn_click(object sender, EventArgs e)
        {
            currentList = foob.GetAllLobbies();
            lobbyList.ItemsSource = currentList;
            loadModeFilterBox();
            loadTagFilterBox();
            currentModeFilter = null;
            currentTagFilter = null;
            updateLobbyCountLabel(currentList.Count);
        }

        /**
        Method: updateLobbyCountLabel
        Imports: lobbyCount (int)
        Exports: None
        Notes: Updates the label displaying the number of active lobbies.
        Algorithm: Sets the content of the label to display the current lobby count.
        */
        private void updateLobbyCountLabel(int lobbyCount)
        {
            lobbyCountLabel.Content = $"Active Lobbies: {lobbyCount}";
        }

        /**
        Method: loadModeFilterBox
        Imports: None
        Exports: None
        Notes: Loads the list of available game modes into the mode filter dropdown.
        Algorithm: Retrieves the unique modes from the server and populates the mode filter box.
        */
        private void loadModeFilterBox()
        {
            List<string> modelist = new List<string> { "" };
            List<string> modes = foob.GetUniqueModes(null);
            modelist.AddRange(modes);
            modeFilterBox.ItemsSource = modelist;
        }

        /**
        Method: loadTagFilterBox
        Imports: None
        Exports: None
        Notes: Loads the list of available tags into the tag filter dropdown.
        Algorithm: Retrieves the unique tags from the server and populates the tag filter box.
        */
        private void loadTagFilterBox()
        {
            List<string> tagList = new List<string> { "" };
            List<string> tags = foob.GetUniqueTags(null);
            tagList.AddRange(tags);
            tagFilterBox.ItemsSource = tagList;
        }

        /**
        Method: lobbyList_SelectionChanged
        Imports: sender (object), e (SelectionChangedEventArgs)
        Exports: None
        Notes: Handles the event when the user selects a lobby from the list. The user is then joined to the selected lobby.
        Algorithm: Joins the selected lobby and opens the lobby room window.
        */
        private void lobbyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lobbyList.SelectedItem != null)
            {
                string selectedLobby = ((Lobby)lobbyList.SelectedItem).Name;
                foob.joinLobby(selectedLobby, currUser);

                lobbyRoomWindow curWindow = new lobbyRoomWindow(selectedLobby);
                curWindow.Show();
                this.Close();
            }
        }

        /**
        Method: createBtn_Click
        Imports: sender (object), e (EventArgs)
        Exports: None
        Notes: Opens the window for creating a new lobby when the user clicks the create button.
        Algorithm: Closes the current window and opens the createLobbyWindow.
        */
        private void createBtn_Click(object sender, RoutedEventArgs e)
        {
            createLobbyWindow curWindow = new createLobbyWindow();
            curWindow.Show();
            this.Close();
        }

        /**
        Method: modeFilterBox_SelectionChanged
        Imports: sender (object), e (SelectionChangedEventArgs)
        Exports: None
        Notes: Handles the selection of a mode filter. Updates the lobby list based on the selected mode.
        Algorithm: Sets the current mode filter and updates the lobby list accordingly.
        */
        private void modeFilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (modeFilterBox.SelectedItem.ToString() == "")
            {
                currentModeFilter = null;
            }
            else
            {
                currentModeFilter = modeFilterBox.SelectedItem.ToString();
            }
            setLobbyList();
            updateFilterList(1);
        }

        /**
        Method: tagFilterBox_SelectionChanged
        Imports: sender (object), e (SelectionChangedEventArgs)
        Exports: None
        Notes: Handles the selection of a tag filter. Updates the lobby list based on the selected tag.
        Algorithm: Sets the current tag filter and updates the lobby list accordingly.
        */
        private void tagFilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tagFilterBox.SelectedItem.ToString() == "")
            {
                currentTagFilter = null;
            }
            else
            {
                currentTagFilter = tagFilterBox.SelectedItem.ToString();
            }
            setLobbyList();
            updateFilterList(2);
        }

        /**
        Method: clearBtn_Click
        Imports: sender (object), e (EventArgs)
        Exports: None
        Notes: Clears the mode and tag filters and resets the lobby list.
        Algorithm: Fetches the full list of lobbies from the server and clears the mode and tag selections.
        */
        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            currentList = foob.GetAllLobbies();
            lobbyList.ItemsSource = currentList;
            modeFilterBox.SelectedItem = "";
            tagFilterBox.SelectedItem = "";
        }

        /**
        Method: app_Exit
        Imports: sender (object), e (CancelEventArgs)
        Exports: None
        Notes: Handles the exit event for the window.
        */
        private void app_Exit(object sender, CancelEventArgs e)
        {
            //foob.RemoveUser(currUser);
        }

        /**
        Method: setLobbyList
        Imports: None
        Exports: None
        Notes: Filters the lobby list based on the current mode and tag filters.
        Algorithm: Fetches the filtered list of lobbies from the server and updates the lobby list view.
        */
        private void setLobbyList()
        {
            currentList = foob.GetfilterdLobbiesList(mode: currentModeFilter, tag: currentTagFilter);
            lobbyList.ItemsSource = currentList;
        }

        /**
        Method: updateFilterList
        Imports: i (int)
        Exports: None
        Notes: Updates the filter lists based on the selected mode or tag.
        Algorithm: If the mode is selected, the tag filter is updated, and vice versa.
        */
        private void updateFilterList(int i)
        {
            if (i == 1)  // mode selected, update tags
            {
                List<string> tagList = new List<string> { "" };
                List<string> tags = foob.GetUniqueTags(currentList);
                tagList.AddRange(tags);
                tagFilterBox.ItemsSource = tagList;
            }
            else  // tag selected, update mode
            {
                List<string> modelist = new List<string> { "" };
                List<string> modes = foob.GetUniqueModes(currentList);
                modelist.AddRange(modes);
                modeFilterBox.ItemsSource = modelist;
            }
        }

        /**
        Method: logOutBtn_Click
        Imports: sender (object), e (EventArgs)
        Exports: None
        Notes: Logs out the current user and closes the window.
        Algorithm: Removes the user from the server and closes the current window.
        */
        private void logOutBtn_Click(object sender, EventArgs e)
        {
            foob.RemoveUser(currUser);
            this.Close();
        }

        /**
        Method: OnClosed
        Imports: e (EventArgs)
        Exports: None
        Notes: Handles the closing event of the window and disposes of the timer.
        Algorithm: Disposes of the timer when the window is closed to prevent memory leaks.
        */
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            timer.Dispose();
        }
    }
}
