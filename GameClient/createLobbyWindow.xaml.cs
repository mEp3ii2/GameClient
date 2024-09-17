/**
Names: L Kipling (20899932), R Mackintosh (21171466), M Pontague (19126924)
Date: 17 September 2024
Class: createLobbyWindow
Purpose: Handles the creation of new lobby rooms in the gaming lobby client application. Users can input details for the lobby, such as name, description, and tags.
Notes: This class communicates with the business layer to create lobbies and join users to the created lobby.
*/

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

        /**
        Method: createLobbyWindow (Constructor)
        Imports: None
        Exports: None
        Notes: Initializes the createLobbyWindow class, setting up the communication with the business server and loading mode types and tag types for selection.
        Algorithm: Sets up references to the communication interface and loads available game modes and tags.
        */
        public createLobbyWindow()
        {
            InitializeComponent();

            this.foob = App.Instance.foob;
            this.currUser = App.Instance.UserName;

            modeSelBox.ItemsSource = foob.GetAllModeTypes();
            optionsBox.ItemsSource = foob.GetAllTagTypes();
        }

        /**
        Method: submitBtn_Click
        Imports: sender (object), e (RoutedEventArgs)
        Exports: None
        Notes: Handles the submission of the lobby creation form by gathering input values (room name, mode, tags) and sending a request to the server to create the lobby.
        Algorithm: The method retrieves values from the form fields, adds the lobby through the business interface, joins the user to the newly created lobby, and opens the lobby room window.
        */
        private void submitBtn_Click(object sender, RoutedEventArgs e)
        {
            mode = modeSelBox.SelectedItem as string;
            tags = optionsBox.SelectedItems.Cast<string>().ToList();

            string tagString = "";
            foreach (string tag in tags)
            {
                tagString += tag;
            }
            roomName = nameTxtBox.Text;
            desc = descTxtBox.Text;
            MessageBox.Show(mode + " " + tagString + " " + roomName + " " + desc);
            foob.AddLobby(roomName, desc, mode, tags);
            foob.joinLobby(roomName, currUser);
            lobbyRoomWindow curWindow = new lobbyRoomWindow(roomName);
            curWindow.Show();
            this.Close();
        }

        /**
        Method: clearBtn_Click
        Imports: sender (object), e (RoutedEventArgs)
        Exports: None
        Notes: Clears the input fields in the lobby creation form.
        Algorithm: The method clears the text boxes and deselects any selected options in the form.
        */
        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            nameTxtBox.Clear();
            modeSelBox.SelectedItem = null;
            descTxtBox.Clear();
            optionsBox.SelectedItems.Clear();
        }

        /**
        Method: backBtn_Click
        Imports: sender (object), e (RoutedEventArgs)
        Exports: None
        Notes: Navigates back to the lobby finder window when the user clicks the back button.
        Algorithm: Closes the current window and opens the lobbyFinderWindow.
        */
        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            lobbyFinderWindow curWindow = new lobbyFinderWindow();
            curWindow.Show();
            this.Close();
        }
    }
}
