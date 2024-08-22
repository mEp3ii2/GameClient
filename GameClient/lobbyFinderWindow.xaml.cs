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
        private User currUser;
        
        private List<Lobby> currentList;
        private string currentModeFilter;
        private string currentTagFilter;
        private BusinessServerInterface foob;

        public lobbyFinderWindow(User currUser)
        {
            InitializeComponent();

            ChannelFactory<BusinessServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            string URL = "net.tcp://localhost:8100/GameService";
            foobFactory = new ChannelFactory<BusinessServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();

            this.currUser = currUser;
            currentList = foob.GetAllLobbies();
            lobbyList.ItemsSource = foob.GetAllLobbies();
            loadModeFilterBox();
            loadTagFilterBox();
            currentModeFilter = null;
            currentTagFilter = null;
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

        private void loadTagFilterBox()
        {
            List<string> tagList = new List<string>();
            tagList.Add("");
            List<string> tags = foob.GetUniqueTags(null);
            tagList.AddRange(tags);
            tagFilterBox.ItemsSource = tagList;
        }

        private void lobbyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // user has double clicked on lobby
            // send user to lobby
            Lobby selectedLobby = (Lobby) lobbyList.SelectedItem;
            lobbyRoomWindow curWindow = new lobbyRoomWindow(selectedLobby, currUser);
            curWindow.Show();
            this.Close();
            //need to modify lobby to reflect new user
        }

        private void createBtn_Click(object sender, RoutedEventArgs e)
        {
            // user creating room
            createLobbyWindow curWindow = new createLobbyWindow(currUser);
            curWindow.Show();
            this.Close();
            
        }

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


        //temp fucn will be changed later on so that on closing message is passed to server
        // to remove user name
        private void app_Exit(object sender, CancelEventArgs e)
        {
            string username = "loggedout";
            MessageBox.Show(username);
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
    }
}
