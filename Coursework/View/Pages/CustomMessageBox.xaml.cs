using System;
using System.Windows;
using System.Windows.Controls;

namespace Coursework.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : UserControl
    {
        public bool IsShowing
        {
            get { return Visibility == Visibility.Visible; }
        }

        public event RoutedEventHandler OkClick
        {
            add { OkButton.Click += value; }
            remove { OkButton.Click -= value; }
        }

        public event RoutedEventHandler CancelClick
        {
            add { CancelButton.Click += value; }
            remove { CancelButton.Click -= value; }
        }

        public event Action<bool> ChangedShowing;

        public CustomMessageBox()
        {
            InitializeComponent();
        }

        public void Show(string parText, string parOkText = "Удалить", string parCancelText = "ОТМЕНА")
        {
            Visibility = Visibility.Visible;
            Text.Text = parText;
            OkButton.Content = parOkText;
            CancelButton.Content = parCancelText;
        }

        public void Hide()
        {
            Visibility = Visibility.Hidden;
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ChangedShowing?.Invoke(IsShowing);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
