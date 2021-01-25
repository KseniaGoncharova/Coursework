using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Coursework
{
    public partial class TreeGroupFiles : UserControl
    {
        private GroupData _data;
        private bool _isExpanded = false;

        private List<TreeFileButton> _filesList = new List<TreeFileButton>();

        public event Action<TreeGroupFiles> AddFileClick;
        public event Action<TreeGroupFiles> EditClick;
        public event Action<TreeGroupFiles> RemoveClick;

        //============================================================

        public GroupData Data
        {
            get { return _data; }
            set { SetData(value); }
        }

        public int Count
        {
            get { return _filesList.Count; }
        }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { SetIsExpanded(value); }
        }

        public IReadOnlyList<TreeFileButton> FilesList
        {
            get { return _filesList; }
        }

        //============================================================

        public TreeGroupFiles()
        {
            InitializeComponent();
        }

        public TreeGroupFiles(GroupData parData) : this()
        {
            SetData(parData);
        }

        //============================================================

        public void SetData(GroupData parData)
        {
            _data = parData;
            _title.Text = _data.Title;

            if (_data.Id == 0)
            {
                _removeButton.Visibility = _editButton.Visibility = Visibility.Collapsed;
            }
        }

        //============================================================

        public TreeFileButton GetFile(int parId)
        {
            foreach (TreeFileButton fileButton in _files.Children)
            {
                if (fileButton.Data.Id == parId)
                {
                    return fileButton;
                }
            }
            return null;
        }

        public void AddFiles(TreeFileButton[] parFiles)
        {
            for (int i = 0; i < parFiles.Length; i++)
            {
                Internal_AddFile(parFiles[i]);
            }
            _count.Text = $"({_filesList.Count})";
        }

        public void AddFile(TreeFileButton parFileButton)
        {
            Internal_AddFile(parFileButton);
            _count.Text = $"({_filesList.Count})";
        }

        private void Internal_AddFile(TreeFileButton parFileButton)
        {
            _filesList.Add(parFileButton);
            _files.Children.Insert(_filesList.Count - 1, parFileButton);
        }

        public void RemoveFile(int parId)
        {
            foreach (TreeFileButton fileButton in _files.Children)
            {
                if (fileButton.Data.Id == parId)
                {
                    RemoveFile(fileButton);
                    return;
                }
            }
        }
        public void RemoveFile(TreeFileButton parFile)
        {
            _files.Children.Remove(parFile);
            _filesList.Remove(parFile);
            _count.Text = $"({_filesList.Count})";
        }

        public void ClearFiles()
        {
            _filesList.Clear();
            _files.Children.Clear();
            _count.Text = "(0)";
        }

        //============================================================

        public void SetIsExpanded(bool parValue)
        {
            _isExpanded = parValue;
            if (_isExpanded)
            {
                _contentSize.Height = new GridLength(0);
                _collapsArrow.Visibility = Visibility.Collapsed;
                _expandArrow.Visibility = Visibility.Visible;
            }
            else
            {
                _contentSize.Height = GridLength.Auto;
                _collapsArrow.Visibility = Visibility.Visible;
                _expandArrow.Visibility = Visibility.Collapsed;
            }
        }

        //============================================================

        private void _expandButton_OnClick(object parSender, RoutedEventArgs parE)
        {
            IsExpanded = !IsExpanded;
        }

        private void _editButton_Click(object sender, RoutedEventArgs e)
        {
            EditClick?.Invoke(this);
        }

        private void _removeButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveClick?.Invoke(this);
        }

        private void UserControl_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _addFileButton.Visibility = _removeButton.Visibility = _editButton.Visibility = Visibility.Visible;

            if (_data.Id == 0)
            {
              _removeButton.Visibility = _editButton.Visibility = Visibility.Collapsed;
            }
        }

        private void UserControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _addFileButton.Visibility = _removeButton.Visibility = _editButton.Visibility = Visibility.Hidden;

            if (_data.Id == 0)
            {
              _removeButton.Visibility = _editButton.Visibility = Visibility.Collapsed;
            }
        }

        private void _addFileButton_Click(object sender, RoutedEventArgs e)
        {
            AddFileClick?.Invoke(this);
        }
    }
}
