using System;
using System.Windows;
using System.Windows.Controls;

namespace Coursework.View
{
    /// <summary>
    /// Interaction logic for TagButton.xaml
    /// </summary>
    public partial class TagButton : UserControl
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
                    _selectButton.Style = (Style)FindResource("OnlyGreenButton");
                }
                else
                {
                    _selectButton.Style = (Style)FindResource("GreenButton");
                }
                IsSelectedChanged?.Invoke(this);
            }
        }

        public event Action<TagButton> IsSelectedChanged;

        public TagButton()
        {
            InitializeComponent();
        }

        public TagButton(TagData parData) : this()
        {
            SetData(parData);
        }

        public void SetData(TagData parData)
        {
            _data = parData;
            _selectButton.Content = _data.Title;
        }

        private void _selectButton_Click(object sender, RoutedEventArgs e)
        {
            IsSelected = !IsSelected;
        }
    }
}
