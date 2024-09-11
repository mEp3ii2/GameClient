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

namespace GameClient
{
    /// <summary>
    /// Interaction logic for lobbyRoomWindow.xaml
    /// </summary>
    public partial class lobbyRoomWindow : Window
    {   
        private User currUser, selectedUser;
        private List<User> lobbyList;
        private IBusinessServerInterface foob;
        private Message currentMessage;
        private Lobby thisLobby;
        public lobbyRoomWindow(Lobby selectedLobby, User currUser,IBusinessServerInterface foob) 
        {
            
            InitializeComponent();
            this.currUser = currUser;
            this.foob = foob;
            this.thisLobby = selectedLobby;
            messageList.Document.Blocks.Clear();
            
            updateUsers();
            MessageBox.Show(selectedLobby.Name+ selectedLobby.ID.ToString());
            Message lobbyMessage = foob.GetMessage(null, currUser, thisLobby);
            currentMessage = lobbyMessage;
            displayMsgs();

            filesList.AddHandler(Hyperlink.RequestNavigateEvent, new RoutedEventHandler(Hyperlink_Click)); // Enable hyperlink click handling for the RichTextBox

            // Load previously shared files for the lobby
            LoadSharedFiles();

            //fill userList


            //get all users to populate user box
            //load current chat history


        }

        private void logOutBtn_Click(object sender, RoutedEventArgs e)
        {
            foob.RemoveUserFromLobby(thisLobby, currUser);
            foob.RemoveUser(currUser);
            this.Close();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            foob.RemoveUserFromLobby(thisLobby, currUser);
            lobbyFinderWindow curWindow = new lobbyFinderWindow(currUser, foob);
            this.Close();
            curWindow.Show();
        }

        private void messageBtn_Click(object sender, RoutedEventArgs e)
        {
            string msg =$"{currUser.Name}: {userMessageBox.Text.ToString()}\n";
            currentMessage.MessageList.Add(msg);
            foob.UpdateMessage(currentMessage, thisLobby);
            displayMsgs();
        }

        // Upload file button click handler
        private void attachmentBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                byte[] fileData = File.ReadAllBytes(filePath);
                string fileName = System.IO.Path.GetFileName(filePath);

                // Pass the lobby name when uploading the file
                foob.UploadFile(fileData, fileName, thisLobby.Name);

                // Immediately display the uploaded file as a clickable hyperlink in the file list
                AddFileToRichTextBox(fileName);
            }
        }

        // Helper method to add the file as a hyperlink to the RichTextBox
        private void AddFileToRichTextBox(string fileName)
        {
            Paragraph paragraph = new Paragraph();
            Hyperlink link = new Hyperlink(new Run(fileName));
            link.Click += (s, args) => OpenFile(fileName);  // Set up the click event handler
            paragraph.Inlines.Add(link);

            // Add the hyperlink to the RichTextBox for file list
            filesList.Document.Blocks.Add(paragraph);
        }

        // Handle hyperlink click event to open the file
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

        // Open the file using the default application for its type
        private void OpenFile(string fileName)
        {
            try
            {
                // Download the file and save it locally before opening
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

        // Method to load shared files when re-entering the lobby
        private void LoadSharedFiles()
        {
            // Fetch the list of previously shared files for the current lobby
            List<string> uploadedFiles = foob.GetLobbyFiles(thisLobby.Name);  // Ensure thisLobby.Name is passed correctly

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

        private void DownloadFile(string fileName)
        {
            try
            {
                byte[] fileData = foob.DownloadFile(fileName);
                string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Downloads", fileName);  // Explicit reference to System.IO.Path otherwise its an ambiguous reference between that and System.Windows.Shapes.Path.
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filePath));  // Explicit reference to System.IO.Path otherwise its an ambiguous reference between that and System.Windows.Shapes.Path.
                File.WriteAllBytes(filePath, fileData);
                MessageBox.Show($"File downloaded successfully to {filePath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error downloading file: {ex.Message}");
            }
        }

        private void userList_TextChanged(object sender, TextChangedEventArgs e)
        {
            //load chat related to selected user
        }

        private void userlistBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // user has selected user or lobby
            // change message box to relect chat with said entity
            string selectedChat;
            if (userlistBox.SelectedItem == null)
            {
                selectedChat = "lobby";
            }
            else
            {
                selectedChat = userlistBox.SelectedItem.ToString();
            }
            
            selectedUser = foob.GetUser(selectedChat);

            Message selectedMessage = foob.GetMessage(currUser, selectedUser, thisLobby);
            currentMessage = selectedMessage;

            displayMsgs();
        }

        private void displayMsgs()
        {
            messageList.Document.Blocks.Clear();
            List<string> msgList = currentMessage.MessageList;

            foreach (string msgItem in msgList)
            {
                // Create a new paragraph for each message item
                var paragraph = new Paragraph(new Run(msgItem));
                messageList.Document.Blocks.Add(paragraph);
            }
        }

        private void refreshBtn_click(object sender, RoutedEventArgs e)
        {
            // Update messages from server
            currentMessage = foob.GetMessage(currUser, selectedUser, thisLobby);
            displayMsgs();

            // Update users in the server
            updateUsers();

            // Update the file list in the RichTextBox
            updateFiles();
        }

        // Helper method to update the file list
        private void updateFiles()
        {
            // Clear the existing file list from the RichTextBox
            filesList.Document.Blocks.Clear();

            // Retrieve the list of uploaded files for the current lobby
            List<string> uploadedFiles = foob.GetLobbyFiles(thisLobby.Name);

            // Display each file as a clickable hyperlink in the RichTextBox
            foreach (string fileName in uploadedFiles)
            {
                AddFileToRichTextBox(fileName);  // Reuse the existing helper method
            }
        }

        private void updateUsers()
        {
            lobbyList = foob.GetUsers(thisLobby);
            lobbyList.Remove(currUser);
            userlistBox.ItemsSource = lobbyList;
        }

    }
}
