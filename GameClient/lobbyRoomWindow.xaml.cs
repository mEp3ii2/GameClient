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

        protected override void OnClosed(EventArgs e)
        {
            foob.RemoveUserFromLobby(thisLobby, currUser);
            foob.RemoveUser(currUser);
            this.Close();
        }

        private void messageBtn_Click(object sender, RoutedEventArgs e)
        {
            string msg =$"{currUser.Name}: {userMessageBox.Text.ToString()}\n";
            currentMessage.MessageList.Add(msg);
            foob.UpdateMessage(currentMessage, thisLobby);
            displayMsgs();
        }

        

        private void attachmentBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                byte[] fileData = File.ReadAllBytes(filePath);
                string fileName = System.IO.Path.GetFileName(filePath);

                //
                foob.UploadFile(fileData,fileName);
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
            //Update messages from server
            currentMessage = foob.GetMessage(currUser, selectedUser, thisLobby);
            displayMsgs();

            //Update users in server
            updateUsers();
        }

        private void updateUsers()
        {
            lobbyList = foob.GetUsers(thisLobby);
            lobbyList.Remove(currUser);
            userlistBox.ItemsSource = lobbyList;
        }

    }
}
