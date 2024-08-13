using System;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameClient
{
    /// <summary>
    /// Interaction logic for messageForm.xaml
    /// </summary>
    public partial class messageForm : UserControl
    {
        public messageForm()
        {
            InitializeComponent();
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string relativePath = @"Resources\formBackGroundImage.webp";
            string absolutePath = System.IO.Path.Combine(basePath, relativePath);

            ImageBrush brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri(absolutePath,UriKind.Absolute));
            this.Background = brush;
        }
    }
}
