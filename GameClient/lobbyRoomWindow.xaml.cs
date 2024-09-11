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
        private System.Timers.Timer messageUpdateTimer;

        public lobbyRoomWindow(string selectedLobby)
        {
            InitializeComponent();
            this.foob = App.Instance.foob;
            this.thisLobby = selectedLobby;

            StartMessageUpdateTimer(); // Start polling for messages
        }

        private void StartMessageUpdateTimer()
        {
            messageUpdateTimer = new System.Timers.Timer(2000); // Poll every 2 seconds
            messageUpdateTimer.Elapsed += async (sender, e) => await LoadMessagesAsync();
            messageUpdateTimer.AutoReset = true;
            messageUpdateTimer.Enabled = true;
        }

        private async Task LoadMessagesAsync()
        {
            var lobbyMessages = await foob.GetMessageAsync(null, currUser, thisLobby);
            currentMessage = lobbyMessages;

            Dispatcher.Invoke(() =>
            {
                displayMsgs();
            });
        }

        private async Task LoadUsersAsync()
        {
            updateUsers();
        }

        private void logOutBtn_Click(object sender, RoutedEventArgs e)
        {
            foob.RemoveUserFromLobbyAsync(thisLobby, currUser);
            foob.RemoveUserAsync(currUser);
            this.Close();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            foob.RemoveUserFromLobbyAsync(thisLobby, currUser);
            lobbyFinderWindow curWindow = new lobbyFinderWindow();
            this.Close();
            curWindow.Show();
        }

        private async void messageBtn_Click(object sender, RoutedEventArgs e)
        {
            string msg = $"{currUser}: {userMessageBox.Text.ToString()}\n"; // Add the current user's name to the message
            currentMessage.Add(msg); // Add message with username to the current message list
            await foob.UpdateMessageAsync(currentMessage, thisLobby, currUser, selectedUser); // Send message to server
            displayMsgs(); // Display the updated message list
        }


        // Upload file button click handler
        private async void attachmentBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                byte[] fileData = File.ReadAllBytes(filePath);
                string fileName = System.IO.Path.GetFileName(filePath);

                // Pass the lobby name when uploading the file
                await foob.UploadFileAsync(fileData, fileName, thisLobby);

                // Immediately display the uploaded file as a clickable hyperlink in the file list
                AddFileToRichTextBox(fileName);
            }
        }

        // Helper method to add the file as a hyperlink to the RichTextBox
        private void AddFileToRichTextBox(string fileName)
        {
            Paragraph paragraph = new Paragraph();
            Hyperlink link = new Hyperlink(new Run(fileName));
            link.Click += (s, args) => OpenFileAsync(fileName);  // Set up the click event handler
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
                _ = OpenFileAsync(fileName);
            }
        }

        // Open the file using the default application for its type
        private async Task OpenFileAsync(string fileName)
        {
            try
            {
                // Download the file and save it locally before opening
                string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Downloads", fileName);
                byte[] fileData = await foob.DownloadFileAsync(fileName);  // Download the file from the server
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
        private async Task LoadSharedFilesAsync()
        {
            // Fetch the list of previously shared files for the current lobby
            List<string> uploadedFiles = await foob.GetLobbyFilesAsync(thisLobby);  // Ensure thisLobby.Name is passed correctly

            // Display each file as a clickable hyperlink
            foreach (string fileName in uploadedFiles)
            {
                Paragraph paragraph = new Paragraph();
                Hyperlink link = new Hyperlink(new Run(fileName));
                link.Click += (s, args) => OpenFileAsync(fileName);  // Set up the click event handler
                paragraph.Inlines.Add(link);
                filesList.Document.Blocks.Add(paragraph);  // Add the paragraph to the RichTextBox
            }
        }

        private async Task DownloadFileAsync(string fileName)
        {
            try
            {
                byte[] fileData = await foob.DownloadFileAsync(fileName);
                string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Downloads", fileName);
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filePath));  // Ensure directory exists
                File.WriteAllBytes(filePath, fileData);
                MessageBox.Show($"File downloaded successfully to {filePath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error downloading file: {ex.Message}");
            }
        }

        private async void userlistBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (userlistBox.SelectedItem == null || userlistBox.SelectedItem.Equals("lobby"))
            {
                selectedUser = null;
            }
            else
            {
                selectedUser = userlistBox.SelectedItem.ToString();
            }

            var selectedMessage = await foob.GetMessageAsync(currUser, selectedUser, thisLobby);
            currentMessage = selectedMessage;

            displayMsgs();
        }

        private void displayMsgs()
        {
            messageList.Document.Blocks.Clear(); // Clears the current message list
            List<string> msgList = currentMessage; // Current message list

            foreach (string msgItem in msgList)
            {
                var paragraph = new Paragraph(new Run(msgItem)); // Each message now contains the username and message
                messageList.Document.Blocks.Add(paragraph); // Add to RichTextBox
            }
        }

        // Helper method to update the file list
        private async Task updateFilesAsync()
        {
            // Clear the existing file list from the RichTextBox
            filesList.Document.Blocks.Clear();

            // Retrieve the list of uploaded files for the current lobby
            List<string> uploadedFiles = await foob.GetLobbyFilesAsync(thisLobby);

            // Display each file as a clickable hyperlink in the RichTextBox
            foreach (string fileName in uploadedFiles)
            {
                AddFileToRichTextBox(fileName);  // Reuse the existing helper method
            }
        }

        private async void updateUsers()
        {
            List<string> lobbyList = await foob.GetUsersAsync(thisLobby);
            lobbyList.Remove(currUser);
            lobbyList.Add("Lobby");
            userlistBox.ItemsSource = lobbyList;
        }
    }
}
