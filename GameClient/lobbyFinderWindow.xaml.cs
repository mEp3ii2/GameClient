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
        
        private List<Lobby> currentList;//list of lobbies retrieved from business layer
        private string currentModeFilter;
        private string currentTagFilter;
        private IBusinessServerInterface foob;

        public lobbyFinderWindow(string currUser, IBusinessServerInterface foob)
        {
            InitializeComponent();
            this.foob = foob;// connection to business layer

            this.currUser = currUser; 
            currentList = foob.GetAllLobbies();
            lobbyList.ItemsSource = currentList;
            loadModeFilterBox();
            loadTagFilterBox();
            currentModeFilter = null;
            currentTagFilter = null;
            updateLobbyCountLabel(currentList.Count);
        }

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

        private void updateLobbyCountLabel(int lobbyCount)
        {
            lobbyCountLabel.Content =$"Active Lobbies: {lobbyCount}";
        }

        //loads list of unique modes plus a blank option for user to be able to unselect mode filter
        private void loadModeFilterBox()
        {
            List<string> modelist = new List<string>();
            modelist.Add("");
            List<string> modes = foob.GetUniqueModes(null);
            modelist.AddRange(modes);
            modeFilterBox.ItemsSource = modelist;
        }

        //loads list of unique modes plus a blank option for user to be able to unselect mode filter
        private void loadTagFilterBox()
        {
            List<string> tagList = new List<string>();
            tagList.Add("");
            List<string> tags = foob.GetUniqueTags(null);
            tagList.AddRange(tags);
            tagFilterBox.ItemsSource = tagList;
        }

        // user has double clicked on lobby
        // send user to lobby
        private void lobbyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            string selectedLobby = ((Lobby) lobbyList.SelectedItem).Name;
            foob.joinLobby(selectedLobby, currUser);
            
            lobbyRoomWindow curWindow = new lobbyRoomWindow(selectedLobby, currUser, foob);
            curWindow.Show();
            this.Close();
        }

        // option window for creating new lobby
        private void createBtn_Click(object sender, RoutedEventArgs e)
        {
            // user creating room
            createLobbyWindow curWindow = new createLobbyWindow(currUser,foob);
            curWindow.Show();
            this.Close();
            
        }

        // user had select a mode to filter on
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

        private void tagFilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(tagFilterBox.SelectedItem.ToString() == "")
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

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            currentList = foob.GetAllLobbies();
            lobbyList.ItemsSource = currentList;
            modeFilterBox.SelectedItem = "";
            tagFilterBox.SelectedItem = "";
        }

        private void app_Exit(object sender, CancelEventArgs e)
        {
            //foob.RemoveUser(currUser);
            
        }

        private void setLobbyList()
        {
            currentList = foob.GetfilterdLobbiesList(mode: currentModeFilter, tag: currentTagFilter);
            lobbyList.ItemsSource = currentList;
        }

        //update the other filter list when one is selected
        //e.g is deathmatch is the selected mode the tag list will be refreshed to only show tags
        // that are present on deathmatch lobbies
        private void updateFilterList(int i)
        {
            //mode selected update tags
            if(i == 1)
            {
                List<string> tagList = new List<string>();
                tagList.Add("");
                List<string> tags = foob.GetUniqueTags(currentList);
                tagList.AddRange(tags);
                tagFilterBox.ItemsSource = tagList;
            }
            else//tag selected update mode
            {
                List<string> modelist = new List<string>();
                modelist.Add("");
                List<string> modes = foob.GetUniqueModes(currentList);
                modelist.AddRange(modes);
                modeFilterBox.ItemsSource = modelist;
            }
        }

        private void logOutBtn_Click(object sender, EventArgs e)
        {
            //foob.RemoveUser(currUser);
            this.Close();
        }

    }
}
