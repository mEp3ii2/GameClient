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

    public delegate bool VerifyUser(string userName);
    public partial class MainWindow : Window
    {
        private IBusinessServerInterface foob;


        public MainWindow()
        {
            InitializeComponent();


            ChannelFactory<IBusinessServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            string URL = "net.tcp://localhost:8100/GameService";
            foobFactory = new ChannelFactory<IBusinessServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();

            userNumber.Text = "Number of Users: " + foob.GetUserCount(); 
            
        }

        private async void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            string userName = userNameBox.Text;
            
            bool uniqueUser = await Task.Run(()=> VerifyUser(userName));

            if (uniqueUser)
            {
                // open main window and close this one
                // send across user name as well    
                foob.AddUser(userName);
                lobbyFinderWindow curWindow = new lobbyFinderWindow(userName,foob);
                curWindow.Show();
                this.Close();

            }
            else
            {
                userNameBox.Clear();
                MessageBox.Show("Name is taken, please try again");
                
            }
        }

        private bool VerifyUser(string userName)
        {
            return foob.UniqueUser(userName);
        }


        public void refreshBtn_click(object sender, RoutedEventArgs e)
        {
            userNumber.Text = "Number of Users: " + foob.GetUserCount();
        }
    }
}
