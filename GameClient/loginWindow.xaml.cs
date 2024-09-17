/**
Names: L Kipling (20899932), R Mackintosh (21171466), M Pontague (19126924)
Date: 17 September 2024
Class: MainWindow
Purpose: This class manages the login window of the game client. It allows users to log in, checks if the username is unique, and displays the number of users currently online.
Notes: The class communicates with the business server to verify unique usernames, add users, and retrieve the user count in real-time using a Timer.
*/

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
using GameLobbyLib;
using System.Configuration;
using BusinessLayer;
using System.Threading;

namespace GameClient
{
    /// <summary>
    /// Interaction logic for loginWindow.xaml
    /// </summary>

    public delegate bool VerifyUser(string userName);  // Delegate for user verification

    public partial class MainWindow : Window
    {
        private IBusinessServerInterface foob;  // Interface for communication with the business server
        private Timer timer;  // Timer for refreshing the user count

        /**
        Method: MainWindow (Constructor)
        Imports: None
        Exports: None
        Notes: Initializes the main login window, sets up communication with the business layer, and starts a Timer to refresh the number of users online.
        Algorithm: Establishes the server connection and initializes the user count display.
        */
        public MainWindow()
        {
            InitializeComponent();

            foob = App.Instance.foob;

            // Set the initial user count
            userNumber.Text = "Number of Users: " + foob.GetUserCount();

            // Start the Timer to refresh user count every 250ms
            timer = new Timer(Refresh);
            timer.Change(0, 250);
        }

        /**
        Method: Refresh
        Imports: stateInfo (Object)
        Exports: None
        Notes: Periodically refreshes the number of users currently online by fetching the latest count from the server.
        Algorithm: Calls the server to get the user count and updates the GUI.
        */
        private void Refresh(Object stateInfo)
        {
            int NumUsers = foob.GetUserCount();  // Get the current user count from the server
            UpdateGUI(NumUsers);  // Update the GUI with the new user count
        }

        /**
        Method: UpdateGUI
        Imports: NumUsers (int)
        Exports: None
        Notes: Updates the number of users displayed in the GUI.
        Algorithm: Uses the Dispatcher to update the user count in the userNumber TextBox.
        */
        private void UpdateGUI(int NumUsers)
        {
            this.Dispatcher.Invoke(new Action(() => {
                userNumber.Text = "Number of Users: " + NumUsers;  // Update the user count text
            }));
        }

        /**
        Method: OnClosed
        Imports: e (EventArgs)
        Exports: None
        Notes: Disposes of the Timer when the window is closed to prevent memory leaks.
        Algorithm: Calls Dispose on the Timer.
        */
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            timer.Dispose();  // Dispose of the Timer to free resources
        }

        /**
        Method: loginBtn_Click
        Imports: sender (object), e (RoutedEventArgs)
        Exports: None
        Notes: Handles the login button click event. Verifies if the username is unique, adds the user if valid, and opens the lobby window.
        Algorithm: Calls the server to check for username uniqueness, adds the user if valid, and transitions to the lobby finder window.
        */
        private async void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            App.Instance.UserName = userNameBox.Text;  // Store the entered username

            bool uniqueUser = await Task.Run(() => VerifyUser(App.Instance.UserName));  // Verify if the username is unique

            if (uniqueUser)
            {
                // Add the user and open the lobby finder window
                foob.AddUser(App.Instance.UserName);
                lobbyFinderWindow curWindow = new lobbyFinderWindow();
                curWindow.Show();
                this.Close();
            }
            else
            {
                // If the username is not unique, clear the TextBox and display an error message
                userNameBox.Clear();
                MessageBox.Show("Name is taken, please try again");
            }
        }

        /**
        Method: VerifyUser
        Imports: userName (string)
        Exports: bool (indicating whether the username is unique)
        Notes: Verifies if the provided username is unique by checking with the server.
        Algorithm: Calls the server to check if the username is already in use.
        */
        private bool VerifyUser(string userName)
        {
            return foob.UniqueUser(userName);  // Call the server to check if the username is unique
        }

        /**
        Method: refreshBtn_click
        Imports: sender (object), e (RoutedEventArgs)
        Exports: None
        Notes: Refreshes the number of users displayed in the GUI by fetching the latest count from the server.
        Algorithm: Updates the userNumber TextBox with the current user count from the server.
        */
        public void refreshBtn_click(object sender, RoutedEventArgs e)
        {
            userNumber.Text = "Number of Users: " + foob.GetUserCount();  // Refresh the user count
        }
    }
}
