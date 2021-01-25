using Coursework.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Coursework.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddOrEditGroupWindow.xaml
    /// </summary>
    public partial class AddOrEditGroupWindow : UserControl
    {
        public List<ComboBoxItemGroup> _groupsItems = new List<ComboBoxItemGroup>();

        public int Id { get; private set; }

        public GroupData GetGroupData()
        {
            return new GroupData() { Id = Id, Title = Title.Text };
        }

        public bool Adding { get; private set; }

        public bool IsShowing
        {
            get { return Visibility == Visibility.Visible; }
        }

        public event Action<GroupData> OkClick;

        public event Action CancelClick;

        public event Action<bool> ChangedShowing;

        public AddOrEditGroupWindow()
        {
            InitializeComponent();
        }

        public void ShowAdd()
        {
            Id = -1;
            Title.Text = string.Empty;

            Adding = true;

            TitlePanel.Text =  "ДОБАВЛЕНИЕ ГРУППЫ";
            OkButton.Content = "ДОБАВИТЬ";

            Visibility = Visibility.Visible;
        }

        public void ShowEdit(int parId)
        {
            //TODO: извлекаем из бд
            if (GroupDataUtils.GetGroupData(parId, out GroupData data))
            {
                Id = data.Id;
                Title.Text = data.Title;

                Adding = false;

                TitlePanel.Text = "РЕДАКТИРОВАНИЕ ГРУППЫ";
                OkButton.Content = "ПРИМЕНИТЬ";

                Visibility = Visibility.Visible;
            }
        }

        public void Hide()
        {
            Visibility = Visibility.Hidden;
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CancelClick?.Invoke();
            Hide();
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ChangedShowing?.Invoke(IsShowing);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            OkClick?.Invoke(GetGroupData());
            Hide();
        }
    }
}
