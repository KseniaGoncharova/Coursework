
using Coursework.Model;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;

namespace Coursework
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

      var data = new GroupData() { Id = 0, Title = "ОБЩЕЕ" };
      AddGroup(data);

      List<GroupData> groups = GroupDataUtils.GetListGroupData();
              for (int i = 0; i < groups.Count; i++)
              {
                AddGroup(groups[i]);
              }

            List<FileData> files = FileDataUtils.GetListFileData();
            for (int i = 0; i < files.Count; i++)
            {
                AddFile(files[i]);
            }

            _search.TextChanged += _search_TextChanged;
        }

        #region Group

        public void EditGroup( GroupData newData)
        {
            _groups.GetGroup(newData.Id).Data = newData;
            _treeGroups.GetGroup(newData.Id).Data = newData;
        }
        public void AddGroup(GroupData data)
        {
            _groups.AddGroup(CreateGroupFiles(data));
            _treeGroups.AddGroup(CreateTreeGroupFiles(data));
        }
        public void RemoveGroup(int group)
        {
            var groupFiles = _groups.GetGroup(group);
            var treeGroupFiles = _treeGroups.GetGroup(group);

            FileButton[] fileButtons = groupFiles.FilesList.ToArray();
            groupFiles.ClearFiles();
            TreeFileButton[] treeFileButtons = treeGroupFiles.FilesList.ToArray();
            treeGroupFiles.ClearFiles();

            _groups.RemoveGroup(groupFiles);
            _treeGroups.RemoveGroup(treeGroupFiles);

            //перемещение файлов в общую группу
            groupFiles = _groups.GetGroupDefault();
            treeGroupFiles = _treeGroups.GetGroupDefault();

            foreach (var file in fileButtons)
            {
                groupFiles.AddFile(file);
            }
            foreach (var file in treeFileButtons)
            {
                treeGroupFiles.AddFile(file);
            }
        }

        public TreeGroupFiles CreateTreeGroupFiles(GroupData data)
        {
            var retGroupFiles = new TreeGroupFiles(data);
            retGroupFiles.AddFileClick += TreeGroupFiles_AddFileClick;
            retGroupFiles.EditClick += TreeGroupFiles_EditClick;
            retGroupFiles.RemoveClick += TreeGroupFiles_RemoveClick;
            return retGroupFiles;
        }
        public GroupFiles CreateGroupFiles(GroupData data)
        {
            var retGroupFiles = new GroupFiles(data);
            retGroupFiles.AddFileButtonClick += GroupFiles_AddFileButtonClick;
            return retGroupFiles;
        }

        #region RemoveGroup

        GroupData _currentRemoveGroupData;
        private void TreeGroupFiles_RemoveClick(TreeGroupFiles group)
        {
            _currentRemoveGroupData = group.Data;
            _removeMessageBox.Show($"Вы уверены, что хотите удалить группу \"{_currentRemoveGroupData.Title}\"?");
            _removeMessageBox.OkClick += _removeGroupMessageBox_OkClick;
        }

        private void _removeGroupMessageBox_OkClick(object sender, RoutedEventArgs e)
        {
            //удаление из базы данных
            if (GroupDataUtils.RemoveGroupData(_currentRemoveGroupData.Id))
            {
                RemoveGroup(_currentRemoveGroupData.Id);
            }

            _removeMessageBox.OkClick -= _removeGroupMessageBox_OkClick;
        }

        #endregion

        #region EditFile

        private void TreeGroupFiles_EditClick(TreeGroupFiles group)
        {
            _addOrEditGroupWindow.ShowEdit(group.Data.Id);
        }

        private void _addOrEditGroupWindow_OkClick(GroupData data)
        {
            if (_addOrEditGroupWindow.Adding)
            {
                //добавление в бд
                if (GroupDataUtils.AddGroupData(ref data))
                {
                    AddGroup(data);
                }
            }
            else
            {
                //редактирование в бд
                if (GroupDataUtils.EditGroupData(data))
                {
                    EditGroup(data);
                }
            }
        }

        #endregion

        private void TreeGroupFiles_AddFileClick(TreeGroupFiles group)
        {
            _addOrEditFileWindow.ShowAdd(group.Data.Id);
        }

        private void GroupFiles_AddFileButtonClick(GroupFiles group)
        {
            _addOrEditFileWindow.ShowAdd(group.Data.Id);
        }

        private void _addGroupButton_Click(object sender, RoutedEventArgs e)
        {
            _addOrEditGroupWindow.ShowAdd();
        }

        #endregion


        #region File

        public void EditFile(int group, FileData newData)
        {
            var groupFiles = _groups.GetGroup(group);
            if (groupFiles != null)
            {
                groupFiles.GetFile(newData.Id).Data = newData;
            }
            else
            {
                var fileButton = _groups.GetGroupDefault().GetFile(newData.Id);
                if (fileButton != null)
                {
                    fileButton.Data = newData;
                }
            }
            var treeGroupFiles = _treeGroups.GetGroup(group);
            if (treeGroupFiles != null)
            {
                treeGroupFiles.GetFile(newData.Id).Data = newData;
            }
            else
            {
                var treeFileButton = _treeGroups.GetGroupDefault().GetFile(newData.Id);
                if (treeFileButton != null)
                {
                    treeFileButton.Data = newData;
                }
            }

            UpdateSearch();
        }
        public void AddFile(FileData data)
        {
            GroupFiles groupFiles = _groups.GetGroup(data.Group);
            if(groupFiles == null)
            {
                groupFiles = _groups.GetGroupDefault();
            }

            TreeGroupFiles treeGroupFiles = _treeGroups.GetGroup(data.Group);
            if (treeGroupFiles == null)
            {
                treeGroupFiles = _treeGroups.GetGroupDefault();
            }

            groupFiles.AddFile(CreateFileButton(data));
            treeGroupFiles.AddFile(CreateTreeFileButton(data));
            UpdateSearch();
        }
        public void RemoveFile(int group, int id)
        {
            var groupFiles = _groups.GetGroup(group);
            if (groupFiles != null)
            {
                groupFiles.RemoveFile(id);
            }
            else
            {
                _groups.GetGroupDefault().RemoveFile(id);
            }

            var treeGroupFiles = _treeGroups.GetGroup(group);
            if (treeGroupFiles != null)
            {
                treeGroupFiles.RemoveFile(id);
            }
            else
            {
                _treeGroups.GetGroupDefault().RemoveFile(id);
            }
        }

        public TreeFileButton CreateTreeFileButton(FileData data)
        {
            var retButton = new TreeFileButton(data);
            retButton.RunClick += TreeFileButton_RunClick;
            retButton.EditClick += TreeFileButton_EditClick;
            retButton.RemoveClick += TreeFileButton_RemoveClick;
            return retButton;
        }
        public FileButton CreateFileButton(FileData data)
        {
            var retButton = new FileButton(data);
            retButton.RunClick += FileButton_RunClick;
            retButton.EditClick += FileButton_EditClick;
            retButton.RemoveClick += FileButton_RemoveClick;
            return retButton;
        }

        private void FileButton_RunClick(FileButton fileButton)
        {
            if (fileButton.Data.FileExists())
            {
                Process.Start(fileButton.Data.LocalPath);
            }
        }
        private void TreeFileButton_RunClick(TreeFileButton fileButton)
        {
            if (fileButton.Data.FileExists())
            {
                Process.Start(fileButton.Data.LocalPath);
            }
        }

        #region EditFile

        FileData _currentEditFileData;
        private void TreeFileButton_EditClick(TreeFileButton fileButton)
        {
            _currentEditFileData = fileButton.Data;
            _addOrEditFileWindow.ShowEdit(_currentEditFileData.Id);
        }
        private void FileButton_EditClick(FileButton fileButton)
        {
            _currentEditFileData = fileButton.Data;
            _addOrEditFileWindow.ShowEdit(_currentEditFileData.Id);
        }

        private void _addOrEditFileWindow_OkClick(FileData data)
        {
            if (_addOrEditFileWindow.Adding)
            {
                //добавление в бд
                if (FileDataUtils.AddFileData(ref data))
                {
                    AddFile(data);
                }
            }
            else
            {
                //редактирование в бд
                if (data.Group != _currentEditFileData.Group)
                {
                    if (FileDataUtils.EditFileData(data))
                    {
                        RemoveFile(_currentEditFileData.Group, _currentEditFileData.Id);
                        AddFile(data);
                    }
                }
                else
                {
                    if (FileDataUtils.EditFileData(data))
                    {
                        EditFile(_currentEditFileData.Group, data);
                    }
                }
            }
        }

        #endregion

        #region RemoveFile

        FileData _currentRemoveFileData;
        private void TreeFileButton_RemoveClick(TreeFileButton fileButton)
        {
            _currentRemoveFileData = fileButton.Data;
            _removeMessageBox.Show($"Вы уверены, что хотите удалить файл \"{_currentRemoveFileData.GetFileName()}\"?");
            _removeMessageBox.OkClick += _removeFileMessageBox_OkClick;
        }

        private void FileButton_RemoveClick(FileButton fileButton)
        {
            _currentRemoveFileData = fileButton.Data;
            _removeMessageBox.Show($"Вы уверены, что хотите удалить файл \"{_currentRemoveFileData.GetFileName()}\"?");
            _removeMessageBox.OkClick += _removeFileMessageBox_OkClick;
        }

        private void _removeFileMessageBox_OkClick(object sender, RoutedEventArgs e)
        {
            //удаление из базы данных
            if (FileDataUtils.RemoveFileData(_currentRemoveFileData.Id))
            {
                RemoveFile(_currentRemoveFileData.Group, _currentRemoveFileData.Id);
            }

            _removeMessageBox.OkClick -= _removeFileMessageBox_OkClick;
        }

        #endregion

        #endregion

        private void _removeMessageBox_ChangedShowing(bool showing)
        {
            Black.Visibility = showing ? Visibility.Visible : Visibility.Hidden;
        }
        private void _addOrEditFileWindow_ChangedShowing(bool showing)
        {
            Black.Visibility = showing ? Visibility.Visible : Visibility.Hidden;
        }
        private void _addOrEditGroupWindow_ChangedShowing(bool showing)
        {
            Black.Visibility = showing ? Visibility.Visible : Visibility.Hidden;
        }

        private void _search_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            UpdateSearch();
        }

        private void UpdateSearch()
        {
            if (string.IsNullOrEmpty(_search.Text) || _search.Text == _search.PlaceHolder)
            {
                foreach (var group in _treeGroups.GroupFiles)
                {
                    foreach (var file in group.FilesList)
                    {
                        file.Visibility = Visibility.Visible;
                    }
                }
            }
            else
            {
                string text = _search.Text.ToUpper();
                foreach (var group in _treeGroups.GroupFiles)
                {
                    foreach (var file in group.FilesList)
                    {
                        file.Visibility = (file.Title.ToUpper().Contains(text)) ? Visibility.Visible : Visibility.Collapsed;
                    }
                }
            }
        }
    }
}
