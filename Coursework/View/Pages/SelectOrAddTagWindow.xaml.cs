using Coursework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Coursework.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для SelectOrAddTagWindow .xaml
    /// </summary>
    public partial class SelectOrAddTagWindow : UserControl
    {
        public List<SelectTag> _tags = new List<SelectTag>();
        public HashSet<int> _selectedTags = new HashSet<int>();

        public List<int> GetSelectedTags()
        {
            return _selectedTags.ToList();
        }

        public bool IsShowing
        {
            get { return Visibility == Visibility.Visible; }
        }

        public event Action CloseClick;

        public event Action<bool> ChangedShowing;

        public SelectOrAddTagWindow()
        {
            InitializeComponent();
        }

        public void Show()
        {
            Show(new int[0]);
        }

        public void Show(params int[] parSelectedTags)
        {
            _selectedTags = new HashSet<int>(parSelectedTags);

            UpdateTags();

            Visibility = Visibility.Visible;
        }

        public void Hide()
        {
            Visibility = Visibility.Hidden;
        }

        public void UpdateTags()
        {
            _tags.Clear();
            _tagsList.Children.Clear();

            //TODO: извлечение из бд
            List<TagData> tags = TagDataUtils.GetListTagData();
            for (int i = 0; i < tags.Count; i++)
            {
                var view = new SelectTag(tags[i]);
                view.IsSelected = _selectedTags.Contains(tags[i].Id);
                view.IsSelectedChanged += View_IsSelectedChanged;
                view.RemoveClick += View_RemoveClick;

                _tags.Add(view);
                _tagsList.Children.Add(view);
            }
        }

        private void View_RemoveClick(SelectTag tagView)
        {
            if(TagDataUtils.RemoveTagData(tagView.Data.Id))
            {
              _tags.Remove(tagView);
              _tagsList.Children.Remove(tagView);
              _selectedTags.Remove(tagView.Data.Id);
            }
        }

        private void View_IsSelectedChanged(SelectTag tagView)
        {
            if(!tagView.IsSelected)
            {
                _selectedTags.Remove(tagView.Data.Id);
            }
            else
            {
                _selectedTags.Add(tagView.Data.Id);
            }
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ChangedShowing?.Invoke(IsShowing);
        }

        private void _closeButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            CloseClick?.Invoke();
        }

        private void _addTagButton_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(_newTagName.Text) && _newTagName.Text != _newTagName.PlaceHolder)
            {
                var data = new TagData() { Title = _newTagName.Text };
                if(TagDataUtils.AddTagData(ref data))
                {
                    UpdateTags();
                }
            }
        }
    }
}
