using Coursework.Model;
using Coursework.View;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Coursework
{
    public partial class GroupFiles : UserControl
    {
        public delegate void SortTypeHandler(SortType parType);

        private GroupData _data;
        private SortType _sortType = SortType.Alphabetically;
        private bool _isExpanded = false;

        private HashSet<int> _selectedtags = new HashSet<int>();
        private List<FileButton> _filesList = new List<FileButton>();

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

        public SortType SortType
        {
            get { return _sortType; }
            set { SetSortType(value); }
        }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { SetIsExpanded(value); }
        }

        public IReadOnlyList<FileButton> FilesList
        {
            get { return _filesList; }
        }

        public Visibility AddFileVisibility
        {
            get { return _addFileButton.Visibility; }
            set { _addFileButton.Visibility = value; }
        }

        public event Action<GroupFiles> AddFileButtonClick;

        public event SortTypeHandler SortTypeChanged;

        //============================================================

        public GroupFiles()
        {
            InitializeComponent();

            TagDataUtils.TagRemoved += TagDataUtils_TagRemoved;
            FileDataUtils.FileEdited += FileDataUtils_FileEdited;
        }

        private void TagDataUtils_TagRemoved(int id)
        {
            _selectedtags.Remove(id);
            for (int i = 0; i < _sortingTags.Children.Count; i++)
            {
                if(((TagButton)_sortingTags.Children[i]).Data.Id == id)
                {
                    _sortingTags.Children.RemoveAt(i);
                    break;
                }
            }

        }
        private void FileDataUtils_FileEdited(FileData data)
        {
            if(data.Group == _data.Id)
            {
                UpdateTags();
            }
        }

        public GroupFiles(GroupData parData) : this()
        {
            SetData(parData);
        }

        //============================================================

        public void SetData(GroupData parData)
        {
            _data = parData;
            _title.Text = _data.Title;
        }

        //============================================================

        public FileButton GetFile(int parId)
        {
            foreach (FileButton fileButton in _filesList)
            {
                if (fileButton.Data.Id == parId)
                {
                    return fileButton;
                }
            }
            return null;
        }

        public void AddFiles(FileButton[] parFiles)
        {
            for (int i = 0; i < parFiles.Length; i++)
            {
                Internal_AddFile(parFiles[i]);
            }
            _count.Text = $"({_filesList.Count})";
            UpdateTags();
            Sort();
        }

        public void AddFile(FileButton parFileButton)
        {
            Internal_AddFile(parFileButton);
            _count.Text = $"({_filesList.Count})";
            UpdateTags();
            Sort();
        }

        private void Internal_AddFile(FileButton parFileButton)
        {
            _filesList.Add(parFileButton);
            _files.Children.Insert(_files.Children.Count - 1, parFileButton);
        }

        public void RemoveFile(int parId)
        {
            foreach (FileButton fileButton in _filesList)
            {
                if(fileButton.Data.Id == parId)
                {
                    RemoveFile(fileButton);
                    return;
                }
            }
        }
        public void RemoveFile(FileButton parFile)
        {
            _files.Children.Remove(parFile);
            _filesList.Remove(parFile);
            _count.Text = $"({_filesList.Count})";
        }

        public void ClearFiles()
        {
            _filesList.Clear();
            _files.Children.RemoveRange(0, _files.Children.Count - 1);
            _count.Text = "(0)";
        }

        //============================================================

        public void SetSortType(SortType parType)
        {
            _sortType = parType;

            Sort();

            SortTypeChanged?.Invoke(_sortType);
        }

        public void UpdateTags()
        {
            _sortingTags.Children.Clear();

            HashSet<int> tags = new HashSet<int>();
            foreach (var file in _filesList)
            {
                if (FileDataUtils.GetFileData(file.Data.Id, out var data))
                {
                    var t = data.Tags;
                    for (int i = 0; i < t.Length; i++)
                    {
                      tags.Add(t[i]);
                    }
                }
            }
            //TODO: извлечение из бд
            List<TagData> tagsData = TagDataUtils.GetListTagData(tags);
            for (int i = 0; i < tagsData.Count; i++)
            {
                var view = new TagButton(tagsData[i]);
                view.IsSelected = _selectedtags.Contains(tagsData[i].Id);
                view.IsSelectedChanged += TagButton_IsSelectedChanged; ;

                _sortingTags.Children.Add(view);
            }

            SearchTags();
        }

        public void SearchTags()
        {
            if(_selectedtags.Count == 0)
            {
                foreach (var file in _filesList)
                {
                    file.Visibility = Visibility.Visible;
                }
                return;
            }
            foreach (var file in _filesList)
            {
                var data = file.Data;
                foreach (var tag in _selectedtags)
                {
                    file.Visibility = (data.HasTag(tag)) ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        private void TagButton_IsSelectedChanged(TagButton tagButton)
        {
            int id = tagButton.Data.Id;
            if (tagButton.IsSelected)
            {
                _selectedtags.Add(id);
            }
            else
            {
                _selectedtags.Remove(id);
            }
            SearchTags();
        }

        public void Sort()
        {
            switch (_sortType)
            {
                case SortType.Alphabetically:
                    for (int i = 0; i < _filesList.Count; i++)
                    {
                        for (int j = 0; j < _filesList.Count - 1; j++)
                        {
                            if (_filesList[j].Title.CompareTo(_filesList[j + 1].Title) > 0)
                            {
                                var buf = _filesList[j];
                                _filesList[j] = _filesList[j + 1];
                                _filesList[j + 1] = buf;
                            }
                        }
                    }
                    break;
                case SortType.LastAccessedTime:
                    for (int i = 0; i < _filesList.Count; i++)
                    {
                        for (int j = 0; j < _filesList.Count - 1; j++)
                        {
                            if (_filesList[j].Data.GetLastAccessedTime().CompareTo(_filesList[j + 1].Data.GetLastAccessedTime()) < 0)
                            {
                                var buf = _filesList[j];
                                _filesList[j] = _filesList[j + 1];
                                _filesList[j + 1] = buf;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }

            if (_files != null)
            {
                _files.Children.RemoveRange(0, _files.Children.Count - 1);
                foreach (var file in _filesList)
                {
                    _files.Children.Insert(_files.Children.Count - 1, file);
                }
            }
        }

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

        private void _addFileButton_Click(object sender, RoutedEventArgs e)
        {
            AddFileButtonClick?.Invoke(this);
        }

        private void _sortingType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetSortType((SortType)_sortingType.SelectedIndex);
        }
    }
}
