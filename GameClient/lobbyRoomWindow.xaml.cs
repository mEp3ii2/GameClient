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

namespace GameClient
{
    /// <summary>
    /// Interaction logic for lobbyRoomWindow.xaml
    /// </summary>
    public partial class lobbyRoomWindow : Window
    {
        
        private string userName;
        public lobbyRoomWindow(Lobby selectedLobby, string userName) 
        {
            
            InitializeComponent();
            this.userName = userName;
            messageList.Document.Blocks.Clear();
            userList.Document.Blocks.Clear();

            // Set the initial text from the passed-in variable
            Paragraph initialParagraph = new Paragraph();
            initialParagraph.Inlines.Add(new Run(selectedLobby.Name));
            messageList.Document.Blocks.Add(initialParagraph);

            

            //get all users to populate user box
            //load current chat history            

        }



        private void logOutBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            lobbyFinderWindow curWindow = new lobbyFinderWindow(userName);
            this.Close();
            curWindow.Show();
        }

        private void messageBtn_Click(object sender, RoutedEventArgs e)
        {
            addNewMessage(userName, userMessageBox.Text.ToString());
            userMessageBox.Clear();
        }

        private void addNewMessage(string userName, string message)
        {
            Paragraph pg = new Paragraph();

            Run userNameRun = new Run(userName + ": ")
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
    }
}
