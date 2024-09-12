﻿using GameLobbyLib;
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
using System.Windows.Navigation;

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

            filesList.AddHandler(Hyperlink.RequestNavigateEvent, new RoutedEventHandler(Hyperlink_Click)); // Enable hyperlink click handling for the RichTextBox

            // Load previously shared files for the lobby
            LoadSharedFiles();

            //fill userList
            updateUsers();

            //get all users to populate user box
            //load current chat history

            timer = new Timer(Refresh);
            timer.Change(0, 250);
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
            lobbyFinderWindow curWindow = new lobbyFinderWindow();
            this.Close();
            curWindow.Show();
        }

        private void messageBtn_Click(object sender, RoutedEventArgs e)
        {
            refreshBtn_click(sender, e);
            string msg = $"{currUser}: {userMessageBox.Text.ToString()}\n";
            currentMessage.Add(msg);
            foob.UpdateMessage(currentMessage, thisLobby, currUser, selectedUser);
            displayMsgs();
        }

        // Upload file button click handler
        private void attachmentBtn_Click(object sender, RoutedEventArgs e)
        {
            const int maxFileSizeInMB = 5;
            const int maxFileSizeInBytes = maxFileSizeInMB * 1024 * 1024;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            
            if (openFileDialog.ShowDialog() == true)
            {
                if(new FileInfo(openFileDialog.FileName).Length > maxFileSizeInBytes)
                {
                    MessageBox.Show("File size exceeds 5 MB limit");
                    return;
                }
                try
                {
                    string filePath = openFileDialog.FileName;
                    string fileName = System.IO.Path.GetFileName(filePath);

                    // Open the file as a stream
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        // Upload the file using a stream
                        foob.UploadFile2(fileStream, fileName, thisLobby);
                    }

                    // Immediately display the uploaded file as a clickable hyperlink in the file list
                    AddFileToRichTextBox(fileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error uploading file: {ex.Message}");
                }

            }
        }

        // Helper method to add the file as a hyperlink to the RichTextBox
        private void AddFileToRichTextBox(string fileName)
        {
            Paragraph paragraph = new Paragraph();
            Hyperlink link = new Hyperlink(new Run(fileName))
            {
                NavigateUri = new Uri(fileName, UriKind.Relative)

            };
            //link.RequestNavigate += Hyperlink_RequestNavigate;
            link.Click += (s, args) => OpenFile(fileName);  // Set up the click event handler
            paragraph.Inlines.Add(link);

            // Add the hyperlink to the RichTextBox for file list
            filesList.Document.Blocks.Add(paragraph);
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            MessageBox.Show("It worked");
            /*string fileName = e.Uri.ToString();
            OpenFile(fileName);
            e.Handled = true;*/
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
                // Define the local path to save the file
                string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Downloads", fileName);
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filePath));  // Ensure the directory exists

                // Open the stream from the server to download the file
                using (Stream fileStream = foob.DownloadFile2(fileName))  // Download the file as a stream from the server
                {
                    // Save the incoming stream to a local file
                    using (FileStream outputFileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        fileStream.CopyTo(outputFileStream);  // Copy the data from the downloaded stream to the file
                    }
                }

                // Open the downloaded file with the default application
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
            List<string> uploadedFiles = foob.GetLobbyFiles(thisLobby);  // Ensure thisLobby.Name is passed correctly

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
            if (userlistBox.SelectedItem == null || userlistBox.SelectedItem.Equals("lobby"))
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

        private void displayMsgs()
        {
            messageList.Document.Blocks.Clear();
            List<string> msgList = currentMessage;

            foreach (string msgItem in msgList)
            {
                // Create a new paragraph for each message item
                var paragraph = new Paragraph(new Run(msgItem));
                messageList.Document.Blocks.Add(paragraph);
            }
        }

        private void UpdateGUI(List<string> users, List<string> files)
        {
            //Clear old values
            this.Dispatcher.Invoke(() => {
                messageList.Document.Blocks.Clear();
                filesList.Document.Blocks.Clear();

                //Update messages
                foreach (string msgItem in currentMessage)
                {
                    // Create a new paragraph for each message item
                    var paragraph = new Paragraph(new Run(msgItem));
                    messageList.Document.Blocks.Add(paragraph);
                }

                //update users
                userlistBox.ItemsSource = users;

                foreach (string fileName in files)
                {
                    AddFileToRichTextBox(fileName);  // Reuse the existing helper method
                }
            });
        }

        private void refreshBtn_click(object sender, RoutedEventArgs e)
        {
            //// Update messages from server
            //currentMessage = foob.GetMessage(currUser, selectedUser, thisLobby);
            //displayMsgs();

            //// Update users in the server
            //updateUsers();

            //// Update the file list in the RichTextBox
            //updateFiles();
        }

        private void Refresh(Object stateInfo)
        {
            try
            {
                // Update messages from server
                currentMessage = foob.GetMessage(currUser, selectedUser, thisLobby);
                //displayMsgs();

                // Update users in the server
                List<string> lobbyList = foob.GetUsers(thisLobby);
                lobbyList.Remove(currUser);
                lobbyList.Add("Lobby");

                // Update the files
                List<string> uploadedFiles = foob.GetLobbyFiles(thisLobby);

                //Update GUI with new figures
                UpdateGUI(lobbyList, uploadedFiles);
            }
            catch (CommunicationObjectFaultedException)
            {
                //
            }
            

        }

        // Helper method to update the file list
        private void updateFiles()
        {
            // Clear the existing file list from the RichTextBox
            filesList.Document.Blocks.Clear();

            // Retrieve the list of uploaded files for the current lobby
            List<string> uploadedFiles = foob.GetLobbyFiles(thisLobby);

            // Display each file as a clickable hyperlink in the RichTextBox
            foreach (string fileName in uploadedFiles)
            {
                AddFileToRichTextBox(fileName);  // Reuse the existing helper method
            }
        }

        private void app_Exit(object sender, CancelEventArgs e)
        {
            //foob.RemoveUserFromLobby(thisLobby,currUser);
            //foob.RemoveUser(currUser);

        }

        private void updateUsers()
        {
            List<string> lobbyList = foob.GetUsers(thisLobby);
            lobbyList.Remove(currUser);
            lobbyList.Add("Lobby");
            userlistBox.ItemsSource = lobbyList;
        }


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            timer.Dispose();
        }
    }
}



