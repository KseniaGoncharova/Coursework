using System;
using System.Windows;
using System.Windows.Controls;

namespace Coursework.View
{
    /// <summary>
    /// Interaction logic for SelectTag.xaml
    /// </summary>
    public partial class SelectTag : UserControl
    {
        private TagData _data;
        private bool _isSelected;


        public TagData Data
        {
            get { return _data; }
            set { SetData(value); }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set 
            {
                _isSelected = value;
                if(_isSelected)
                {
                    _selectButton.Content = "Убрать";
                    _selectButton.Style = (Style)FindResource("OnlyRedButton");
                }
                else
                {
                    _selectButton.Content = "Выбрать";
                    _selectButton.Style = (Style)FindResource("GreenButton");
                }
                IsSelectedChanged?.Invoke(this);
            }
        }

        public event Action<SelectTag> IsSelectedChanged;
        public event Action<SelectTag> RemoveClick;

        public SelectTag()
        {
            InitializeComponent();
        }

        public SelectTag(TagData parData) : this()
        {
            SetData(parData);
        }

        public void SetData(TagData parData)
        {
            _data = parData;
            _title.Text = _data.Title;
        }

        private void _selectButton_Click(object sender, RoutedEventArgs e)
        {
            IsSelected = !IsSelected;
        }

        private void _removeButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveClick?.Invoke(this);
        }
    }
}
