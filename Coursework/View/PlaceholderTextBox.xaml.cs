using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;

namespace Coursework.View
{
    /// <summary>
    /// Interaction logic for PlaceholderTextBox.xaml
    /// </summary>
    public partial class PlaceholderTextBox : TextBox
    {
        private string _placeHolder;
        public string PlaceHolder
        {
            get { return _placeHolder; }
            set
            {
                if (string.IsNullOrEmpty(Text))
                {
                    _placeHolder = Text = value;
                    Foreground.Opacity = 0.3f;
                }
                else
                {
                    _placeHolder = value;
                    Foreground.Opacity = 1f;
                }
            }
        }

        public PlaceholderTextBox()
        {
            InitializeComponent();

            Foreground = Foreground.Clone();
        }
        
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Text))
            {
                Text = PlaceHolder;
                Foreground.Opacity = 0.3f;
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Text.Equals(PlaceHolder, StringComparison.OrdinalIgnoreCase)) 
            { 
                Text = string.Empty;
                Foreground.Opacity = 1f;
            }
        }
    }
}
