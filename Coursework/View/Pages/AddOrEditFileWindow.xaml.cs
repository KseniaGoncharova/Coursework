using Coursework.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Coursework.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddOrEditFileWindow.xaml
    /// </summary>
    public partial class AddOrEditFileWindow : UserControl
    {
        public List<ComboBoxItemGroup> _groupsItems = new List<ComboBoxItemGroup>();

        public int Id { get; private set; }

        public int[] Tags { get; private set; }

        public FileData GetFileData()
        {
            var groupView = (ComboBoxItemGroup)Group.SelectedItem;
            int group = groupView != null ? groupView.Data.Id : 0;
            return new FileData() { Id = Id, Title = Title.Text, Description = Description.Text, LocalPath = FilePath.Text, Group = group, Tags = Tags };
        }

        public bool Adding { get; private set; }

        public bool IsShowing
        {
            get { return Visibility == Visibility.Visible; }
        }

        public event Action<FileData> OkClick;

        public event Action CancelClick;

        public event Action<bool> ChangedShowing;

        public AddOrEditFileWindow()
        {
            InitializeComponent();
        }

        public void ShowAdd()
        {
            ShowAdd(0);
        }

        public void ShowAdd(int parGroupId)
        {
            Adding = true;

            Id = -1;
            Title.Text =
                Description.Text =
                FilePath.Text = string.Empty;
            Tags = new int[0];

            UpdateGroups();
            SelectGroup(parGroupId);

            TitlePanel.Text = "ДОБАВЛЕНИЕ ФАЙЛА";
            OkButton.Content = "ДОБАВИТЬ";

            Visibility = Visibility.Visible;
        }

        public void ShowEdit(int parId)
        {
            //TODO: извлекаем из бд
            if (FileDataUtils.GetFileData(parId, out FileData data))
            {
                Id = data.Id;
                Title.Text = data.Title;
                Description.Text = data.Description;
                FilePath.Text = data.LocalPath;
                Tags = data.Tags ?? (new int[0]);

                UpdateGroups();
                SelectGroup(data.Group);

                Adding = false;

                TitlePanel.Text = "РЕДАКТИРОВАНИЕ ФАЙЛА";
                OkButton.Content = "ПРИМЕНИТЬ";

                Visibility = Visibility.Visible;
            }
        }
        public void UpdateGroups()
        {
            _groupsItems.Clear();
            Group.Items.Clear();

            //излекаем из бд
            List<GroupData> groups = GroupDataUtils.GetListGroupData();
            groups.Insert(0, new GroupData() { Id = 0, Title = "ОБЩЕЕ" });
            for (int i = 0; i < groups.Count; i++)
            {
                var view = new ComboBoxItemGroup(groups[i]);
                _groupsItems.Add(view);
                Group.Items.Add(view);
            }
        }

        private void SelectGroup(int parId)
        {
            var groupViewIndex = _groupsItems.FindIndex((ComboBoxItemGroup parGroupView) => parGroupView.Data.Id == parId);
            Group.SelectedIndex = groupViewIndex != -1 ? groupViewIndex : 0;
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
            if (!string.IsNullOrEmpty(FilePath.Text))
            {
                OkClick?.Invoke(GetFileData());
                Hide();
            }
        }

        private void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                Title = "Выберите файл",
                FileName = FilePath.Text,
                CheckFileExists = false
            };

            if (dialog.ShowDialog(Window.GetWindow(this)) == true)
            {
                FilePath.Text = dialog.FileName;
            }
        }

        private void _selectOrAddTagWindow_ChangedShowing(bool showing)
        {
            if (showing)
            {
                Black.Visibility = Visibility.Visible;
            }
            else
            {
                Black.Visibility = Visibility.Hidden;
                Tags = _selectOrAddTagWindow.GetSelectedTags().ToArray();
            }
        }

        private void _addTagButton_Click(object sender, RoutedEventArgs e)
        {
            _selectOrAddTagWindow.Show(Tags);
        }
    }
}
