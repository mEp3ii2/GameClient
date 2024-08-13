using System;
using System.Collections.Generic;
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
using GameLobbyLib;

namespace GameClient
{
    /// <summary>
    /// Interaction logic for createLobbyWindow.xaml
    /// </summary>
    public partial class createLobbyWindow : Window
    {
        private string userName;
        private string mode;
        private string desc;
        private string roomName;
        private List<string> tags;
        
        public createLobbyWindow(string userName)
        {
            InitializeComponent();
            this.userName = userName;
            
            modeSelBox.ItemsSource = Database.getAllModeTypes();
            //tagSelBox.ItemsSource = Database.getAllTagTypes();
            

            optionsBox.ItemsSource = Database.getAllTagTypes();
        }

        private void submitBtn_Click(object sender, RoutedEventArgs e)
        {
            mode = modeSelBox.SelectedItem as string;
            List<string> temp = new List<string>();
            
            tags = new List<string>();
            tags = optionsBox.SelectedItems.Cast<string>().ToList();
            
            string tagString= "";
            foreach (string tag in tags)
            {
                tagString = tagString+tag;
            }
            roomName = nameTxtBox.Text;
            desc = descTxtBox.Text;
            MessageBox.Show(mode+" "+tagString + " " +roomName + " " + desc);
        }

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            nameTxtBox.Clear();
            modeSelBox.SelectedItem = null;
            descTxtBox.Clear();
            optionsBox.SelectedItems.Clear();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            lobbyFinderWindow curWindow = new lobbyFinderWindow(userName);
            curWindow.Show();
            this.Close();
        }
    }
}
