using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Coursework
{
    /// <summary>
    /// Логика взаимодействия для TreeGroups.xaml
    /// </summary>
    public partial class TreeGroups : UserControl
    {
        private List<TreeGroupFiles> _groupFiles = new List<TreeGroupFiles>();

        public IReadOnlyList<TreeGroupFiles> GroupFiles { get { return _groupFiles; } }

        public TreeGroups()
        {
            InitializeComponent();
        }

        public TreeGroupFiles GetGroup(int parId)
        {
            return _groupFiles.Find((parGroupFiles => parGroupFiles.Data.Id == parId));
        }

        public TreeGroupFiles GetGroupDefault()
        {
            return _groupFiles[0];
        }

        public void AddGroup(TreeGroupFiles parGroupFiles)
        {
            _groupFiles.Add(parGroupFiles);
            _groups.Children.Add(parGroupFiles);
        }

        public void RemoveGroup(TreeGroupFiles parGroupFiles)
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
