using System.Collections.Generic;
using System.Windows.Controls;

namespace Coursework
{
    public partial class Groups : UserControl
    {
        private List<GroupFiles> _groupFiles = new List<GroupFiles>();

        public Groups()
        {
            InitializeComponent();
        }

        public GroupFiles GetGroup(int parId)
        {
            return _groupFiles.Find((parGroupFiles => parGroupFiles.Data.Id == parId));
        }

        public GroupFiles GetGroupDefault()
        {
            return _groupFiles[0];
        }

        public void AddGroup(GroupFiles parGroupFiles)
        {
            _groupFiles.Add(parGroupFiles);
            _groups.Children.Add(parGroupFiles);
        }

        public void RemoveGroup(GroupFiles parGroupFiles)
        {
            _groupFiles.Remove(parGroupFiles);
            _groups.Children.Remove(parGroupFiles);
        }

        public void RemoveGroup(int parId)
        {
            int index = _groupFiles.FindIndex((parGroupFiles => parGroupFiles.Data.Id == parId));
            if (index != -1)
            {
                _groups.Children.Remove(_groupFiles[index]);
                _groupFiles.RemoveAt(index);
            }
        }

        public void Clear()
        {
            _groupFiles.Clear();
            _groups.Children.Clear();
        }
    }
}
