﻿using System;
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

namespace GameClient
{
    /// <summary>
    /// Interaction logic for loginWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BusinessServerInterface foob;
        public MainWindow()
        {
            InitializeComponent();

            ChannelFactory<BusinessServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            string URL = "net.tcp://localhost:8100/GameService";
            foobFactory = new ChannelFactory<BusinessServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();

            userNumber.Text = "Number of Users: " + foob.GetAllUsers().Count(); 
            
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            string userName = userNameBox.Text;

            // check here is user name is unique
            bool uniqueUser = foob.UniqueUser(userName);
            
            if (uniqueUser)
            {
                // open main window and close this one
                // send across user name as well
                User currUser = new User(userName);
                foob.AddUser(currUser);
                lobbyFinderWindow curWindow = new lobbyFinderWindow(currUser,foob);
                curWindow.Show();
                this.Close();

            }
            else
            {
                userNameBox.Clear();
                MessageBox.Show("Name is taken, please try again");
                
            }
        }
    }
}
