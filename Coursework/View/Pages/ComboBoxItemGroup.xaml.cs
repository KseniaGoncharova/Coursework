using System.Windows.Controls;

namespace Coursework.View.Pages
{
    /// <summary>
    /// Interaction logic for ComboBoxItemGroup.xaml
    /// </summary>
    public partial class ComboBoxItemGroup : ComboBoxItem
    {
        private GroupData _data;

        public GroupData Data
        {
            get { return _data; }
            set { SetData(value); }
        }

        public ComboBoxItemGroup()
        {
            InitializeComponent();
        }

        public ComboBoxItemGroup(GroupData data) : this()
        {
            SetData(data);
        }

        //============================================================

        public void SetData(GroupData parData)
        {
            _data = parData;
            Content = _data.Title;
        }
    }
}
