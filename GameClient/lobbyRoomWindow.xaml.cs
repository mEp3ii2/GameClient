/**
Names: L Kipling (20899932), R Mackintosh (21171466), M Pontague (19126924)
Date: 17 September 2024
Class: lobbyRoomWindow
Purpose: This class manages the interactions within a specific lobby room where users can send messages, share files, and interact with other users in real-time.
Notes: This class communicates with the business server to update and retrieve messages, user lists, and shared files. It also periodically refreshes the data using a Timer.
*/

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
using System.ServiceModel;
using BusinessLayer;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using System.Threading;
using System.Timers;
using Timer = System.Threading.Timer;

namespace GameClient
{
    /// <summary>
    /// Interaction logic for lobbyRoomWindow.xaml
    /// </summary>
    public partial class lobbyRoomWindow : Window
    {
        private string currUser, selectedUser;
        private IBusinessServerInterface foob;
        private List<string> currentMessage;
        private string thisLobby;
        private Timer timer;

        /**
        Method: lobbyRoomWindow (Constructor)
        Imports: selectedLobby (string)
        Exports: None
        Notes: Initializes the lobby room window, sets up communication with the business server, and starts a Timer to refresh messages and files periodically.
        Algorithm: Sets up the initial state by loading the user list, chat history, and shared files. The Timer ensures the data is refreshed periodically.
        */
        public lobbyRoomWindow(string selectedLobby)
        {
            InitializeComponent();
            this.currUser = App.Instance.UserName;
            this.foob = App.Instance.foob;
            this.thisLobby = selectedLobby;
            messageList.Document.Blocks.Clear();

            List<string> lobbyMessages = foob.GetMessage(null, currUser, thisLobby);
            currentMessage = lobbyMessages;
            displayMsgs();

            filesList.AddHandler(Hyperlink.RequestNavigateEvent, new RoutedEventHandler(Hyperlink_Click));  // Enable hyperlink click handling for the RichTextBox

            // Load previously shared files for the lobby
            LoadSharedFiles();

            // Update the user list
            updateUsers();

            // Start the Timer to refresh the lobby data every 250ms
            timer = new Timer(Refresh);
            timer.Change(0, 250);
        }

        /**
        Method: logOutBtn_Click
        Imports: sender (object), e (RoutedEventArgs)
        Exports: None
        Notes: Logs out the current user from both the lobby and the system.
        Algorithm: Removes the user from the lobby and the system by calling the appropriate methods on the server.
        */
        private void logOutBtn_Click(object sender, RoutedEventArgs e)
        {
            foob.RemoveUserFromLobby(thisLobby, currUser);
            foob.RemoveUser(currUser);
            this.Close();
        }

        /**
        Method: backButton_Click
        Imports: sender (object), e (RoutedEventArgs)
        Exports: None
        Notes: Removes the current user from the lobby and navigates back to the lobby finder window.
        Algorithm: Calls the server to remove the user from the lobby and opens the lobbyFinderWindow.
        */
        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            foob.RemoveUserFromLobby(thisLobby, currUser);
            lobbyFinderWindow curWindow = new lobbyFinderWindow();
            this.Close();
            curWindow.Show();
        }

        /**
        Method: messageBtn_Click
        Imports: sender (object), e (RoutedEventArgs)
        Exports: None
        Notes: Sends a message from the user to the selected recipient (or the lobby) and updates the chat history.
        Algorithm: Adds the message to the current chat history and calls the server to update the messages in the lobby.
        */
        private void messageBtn_Click(object sender, RoutedEventArgs e)
        {
            string msg = $"{currUser}: {userMessageBox.Text}\n";
            currentMessage.Add(msg);
            foob.UpdateMessage(currentMessage, thisLobby, currUser, selectedUser);
            displayMsgs();
        }

        /**
        Method: attachmentBtn_Click
        Imports: sender (object), e (RoutedEventArgs)
        Exports: None
        Notes: Handles the file upload process. Opens a file dialog, uploads the selected file to the server, and updates the file list in the lobby.
        Algorithm: Retrieves the selected file from the user, uploads it to the server, and adds it to the file list.
        */
        private void attachmentBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                byte[] fileData = File.ReadAllBytes(filePath);
                string fileName = System.IO.Path.GetFileName(filePath);

                // Pass the lobby name when uploading the file
                foob.UploadFile(fileData, fileName, thisLobby);

                // Immediately display the uploaded file as a clickable hyperlink in the file list
                AddFileToRichTextBox(fileName);
            }
        }

        /**
        Method: AddFileToRichTextBox
        Imports: fileName (string)
        Exports: None
        Notes: Adds the uploaded file as a clickable hyperlink in the RichTextBox.
        Algorithm: Creates a new hyperlink for the file and adds it to the file list.
        */
        private void AddFileToRichTextBox(string fileName)
        {
            Paragraph paragraph = new Paragraph();
            Hyperlink link = new Hyperlink(new Run(fileName));
            link.Click += (s, args) => OpenFile(fileName);  // Set up the click event handler
            paragraph.Inlines.Add(link);

            // Add the hyperlink to the RichTextBox for file list
            filesList.Document.Blocks.Add(paragraph);
        }

        /**
        Method: Hyperlink_Click
        Imports: sender (object), e (RoutedEventArgs)
        Exports: None
        Notes: Handles the event when a user clicks a file hyperlink in the file list.
        Algorithm: Retrieves the file name from the clicked hyperlink and downloads and opens the file.
        */
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Hyperlink link)
            {
                // Retrieve the file name from the hyperlink
                string fileName = link.NavigateUri.ToString();

                // Download and open the file
                OpenFile(fileName);
            }
        }

        /**
        Method: OpenFile
        Imports: fileName (string)
        Exports: None
        Notes: Downloads the file from the server and opens it using the default application for its type.
        Algorithm: Downloads the file from the server and writes it to the local disk, then opens it using the default system application.
        */
        private void OpenFile(string fileName)
        {
            try
            {
                string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Downloads", fileName);
                byte[] fileData = foob.DownloadFile(fileName);  // Download the file from the server
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filePath));  // Ensure directory exists
                File.WriteAllBytes(filePath, fileData);  // Save the file locally

                // Now open the file with the default application for its type
                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening file: {ex.Message}");
            }
        }

        /**
        Method: LoadSharedFiles
        Imports: None
        Exports: None
        Notes: Loads the list of files previously shared in the lobby.
        Algorithm: Fetches the shared files from the server and displays them as clickable hyperlinks in the file list.
        */
        private void LoadSharedFiles()
        {
            // Fetch the list of previously shared files for the current lobby
            List<string> uploadedFiles = foob.GetLobbyFiles(thisLobby);

            // Display each file as a clickable hyperlink
            foreach (string fileName in uploadedFiles)
            {
                Paragraph paragraph = new Paragraph();
                Hyperlink link = new Hyperlink(new Run(fileName));
                link.Click += (s, args) => OpenFile(fileName);  // Set up the click event handler
                paragraph.Inlines.Add(link);
                filesList.Document.Blocks.Add(paragraph);  // Add the paragraph to the RichTextBox
            }
        }

        /**
        Method: DownloadFile
        Imports: fileName (string)
        Exports: None
        Notes: Downloads the specified file from the server and saves it locally.
        Algorithm: Fetches the file from the server and writes it to the local Downloads directory.
        */
        private void DownloadFile(string fileName)
        {
            try
            {
                byte[] fileData = foob.DownloadFile(fileName);
                string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Downloads", fileName);
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filePath));
                File.WriteAllBytes(filePath, fileData);
                MessageBox.Show($"File downloaded successfully to {filePath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error downloading file: {ex.Message}");
            }
        }

        /**
        Method: userlistBox_SelectionChanged
        Imports: sender (object), e (SelectionChangedEventArgs)
        Exports: None
        Notes: Handles the event when a user selects a different user or the lobby from the user list.
        Algorithm: Updates the current chat history based on the selected user or the lobby.
        */
        private void userlistBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (userlistBox.SelectedItem == null || userlistBox.SelectedItem.Equals("Lobby"))
            {
                selectedUser = null;
            }
            else
            {
                selectedUser = userlistBox.SelectedItem.ToString();
            }

            List<string> selectedMessage = foob.GetMessage(currUser, selectedUser, thisLobby);
            currentMessage = selectedMessage;

            displayMsgs();
        }

        /**
        Method: displayMsgs
        Imports: None
        Exports: None
        Notes: Displays the current messages in the RichTextBox.
        Algorithm: Clears the RichTextBox and adds each message in the current message list as a paragraph.
        */
        private void displayMsgs()
        {
            messageList.Document.Blocks.Clear();
            foreach (string msgItem in currentMessage)
            {
                var paragraph = new Paragraph(new Run(msgItem));
                messageList.Document.Blocks.Add(paragraph);
            }
        }

        /**
        Method: Refresh
        Imports: stateInfo (Object)
        Exports: None
        Notes: Periodically refreshes the messages, user list, and file list from the server.
        Algorithm: Retrieves the latest messages, users, and files from the server and updates the GUI accordingly.
        */
        private void Refresh(Object stateInfo)
        {
            // Update messages from the server
            currentMessage = foob.GetMessage(currUser, selectedUser, thisLobby);

            // Update users in the server
            List<string> lobbyList = foob.GetUsers(thisLobby);
            lobbyList.Remove(currUser);
            lobbyList.Add("Lobby");

            // Update the files
            List<string> uploadedFiles = foob.GetLobbyFiles(thisLobby);

            // Update the GUI with the new data
            UpdateGUI(lobbyList, uploadedFiles);
        }

        /**
        Method: UpdateGUI
        Imports: users (List<string>), files (List<string>)
        Exports: None
        Notes: Updates the user list, message list, and file list in the GUI.
        Algorithm: Invokes the UI thread to update the displayed user list, messages, and files.
        */
        private void UpdateGUI(List<string> users, List<string> files)
        {
            // Clear old values and update the GUI
            this.Dispatcher.Invoke(() =>
            {
                messageList.Document.Blocks.Clear();
                filesList.Document.Blocks.Clear();

                // Update messages
                foreach (string msgItem in currentMessage)
                {
                    var paragraph = new Paragraph(new Run(msgItem));
                    messageList.Document.Blocks.Add(paragraph);
                }

                // Update users
                userlistBox.ItemsSource = users;

                // Update files
                foreach (string fileName in files)
                {
                    AddFileToRichTextBox(fileName);  // Reuse the existing helper method
                }
            });
        }

        /**
        Method: updateUsers
        Imports: None
        Exports: None
        Notes: Updates the user list from the server and removes the current user from the list.
        Algorithm: Fetches the user list from the server and updates the displayed user list.
        */
        private void updateUsers()
        {
            List<string> lobbyList = foob.GetUsers(thisLobby);
            lobbyList.Remove(currUser);
            lobbyList.Add("Lobby");
            userlistBox.ItemsSource = lobbyList;
        }

        /**
        Method: OnClosed
        Imports: e (EventArgs)
        Exports: None
        Notes: Disposes of the Timer when the window is closed to prevent memory leaks.
        Algorithm: Calls the Dispose method on the Timer when the window is closed.
        */
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            timer.Dispose();
        }
    }
}
