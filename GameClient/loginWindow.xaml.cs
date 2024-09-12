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

    public delegate bool VerifyUser(string userName);
    public partial class MainWindow : Window
    {
        private IBusinessServerInterface foob;


        public MainWindow()
        {
            InitializeComponent();

            foob = App.Instance.foob;
            

            userNumber.Text = "Number of Users: " + foob.GetUserCount();

            Timer timer = new Timer(Refresh);
            timer.Change(0, 250);
        }

        private void Refresh(Object stateInfo)
        {
            int NumUsers = foob.GetUserCount();
            UpdateGUI(NumUsers);
        }

        private void UpdateGUI(int NumUsers)
        {
            this.Dispatcher.Invoke(new Action(() => {
                userNumber.Text = "Number of Users: " + NumUsers;
            }));
        }

        private async void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            App.Instance.UserName = userNameBox.Text;
            
            bool uniqueUser = await Task.Run(()=> VerifyUser(App.Instance.UserName));

            if (uniqueUser)
            {
                // open main window and close this one
                // send across user name as well    
                foob.AddUser(App.Instance.UserName);
                lobbyFinderWindow curWindow = new lobbyFinderWindow();
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
