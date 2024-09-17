/**
Names: L Kipling (20899932), R Mackintosh (21171466), M Pontague (19126924)
Date: 17 September 2024
Class: messageForm
Purpose: This class defines the message form user control, which includes setting a background image dynamically from the Resources folder.
Notes: The class loads a webp image as the background from a relative path in the Resources folder and applies it to the control using an ImageBrush.
*/

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
        /**
        Method: messageForm (Constructor)
        Imports: None
        Exports: None
        Notes: Initializes the message form control and sets a background image using a relative file path.
        Algorithm: Loads an image from the Resources folder and applies it as a background using an ImageBrush.
        */
        public messageForm()
        {
            InitializeComponent();

            // Get the base directory path of the application
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            // Specify the relative path to the background image in the Resources folder
            string relativePath = @"Resources\formBackGroundImage.webp";

            // Combine the base path and the relative path to create the absolute path
            string absolutePath = System.IO.Path.Combine(basePath, relativePath);

            // Create a new ImageBrush and set the background image
            ImageBrush brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri(absolutePath, UriKind.Absolute));

            // Apply the ImageBrush to the background of the message form
            this.Background = brush;
        }
    }
}
