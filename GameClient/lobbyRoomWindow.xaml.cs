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
        
        private User currUser;
        private List<User> lobbyList;
        private IBusinessServerInterface foob;
        public lobbyRoomWindow(Lobby selectedLobby, User currUser,IBusinessServerInterface foob) 
        {
            
            InitializeComponent();
            this.currUser = currUser;
            this.foob = foob;
            messageList.Document.Blocks.Clear();
            
            lobbyList = selectedLobby.Users.ToList();
            userlistBox.ItemsSource = lobbyList;

            // Set the initial text from the passed-in variable
            Paragraph initialParagraph = new Paragraph();
            initialParagraph.Inlines.Add(new Run(selectedLobby.Name));
            messageList.Document.Blocks.Add(initialParagraph);

            //fill userList
            

            //get all users to populate user box
            //load current chat history
            

        }



        private void logOutBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            lobbyFinderWindow curWindow = new lobbyFinderWindow(currUser, foob);
            this.Close();
            curWindow.Show();
        }

        private void messageBtn_Click(object sender, RoutedEventArgs e)
        {
            addNewMessage(currUser, userMessageBox.Text.ToString());
            userMessageBox.Clear();
        }

        private void addNewMessage(User currUser, string message)
        {
            Paragraph pg = new Paragraph();

            Run userNameRun = new Run(currUser.Name + ": ")
            {
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Red
            };

            Run messageRun = new Run(message)
            {
                Foreground = Brushes.Black
            };

            pg.Inlines.Add(userNameRun);
            pg.Inlines.Add(messageRun);

            
            messageList.Document.Blocks.Add(pg);

            
            messageList.ScrollToEnd();
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
    }
}
