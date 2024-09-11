using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ServiceModel;
using GameLobbyLib;
using BusinessLayer;

namespace GameClient
{
    /// <summary>
    /// Interaction logic for loginWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private IBusinessServerInterface foob;

        public MainWindow()
        {
            InitializeComponent();
            foob = App.Instance.foob;

            // Async operation to fetch user count
            Task task = LoadUserCountAsync();
        }

        private async Task LoadUserCountAsync()
        {
            if (App.Instance.foob != null)
            {
                int userCount = await App.Instance.foob.GetUserCountAsync();
                userNumber.Text = "Number of Users: " + userCount;
            }
            else
            {
                MessageBox.Show("Server connection is not established.");
            }
        }

        private async void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (App.Instance.foob != null)
            {
                App.Instance.UserName = userNameBox.Text;

                bool uniqueUser = await VerifyUserAsync(App.Instance.UserName);

                if (uniqueUser)
                {
                    // Add user asynchronously
                    await App.Instance.foob.AddUserAsync(App.Instance.UserName);

                    // Open the main lobby window
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
            else
            {
                MessageBox.Show("Server connection is not established.");
            }
        }

        private async Task<bool> VerifyUserAsync(string userName)
        {
            if (App.Instance.foob != null)
            {
                return await App.Instance.foob.UniqueUserAsync(userName);
            }
            else
            {
                MessageBox.Show("Server connection is not established.");
                return false;
            }
        }

        /*private async void refreshBtn_click(object sender, RoutedEventArgs e)
        {
            int userCount = await foob.GetUserCountAsync();
            userNumber.Text = "Number of Users: " + userCount;
        }*/
    }
}
