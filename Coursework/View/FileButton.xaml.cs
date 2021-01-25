using System;
using System.Windows;
using System.Windows.Controls;

namespace Coursework
{
    public partial class FileButton : UserControl
    {
        private FileData _data;

        //============================================================

        public FileData Data
        {
            get { return _data; }
            set { SetData(value); }
        }

        public string Title
        {
            get { return _fileName.Text; }
        }

        public event Action<FileButton> RunClick;
        public event Action<FileButton> EditClick;
        public event Action<FileButton> RemoveClick;

        //============================================================

        /// <summary>
        /// Конструктор поу молчанию
        /// </summary>
        public FileButton()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="parData">Данные о файле</param>
        public FileButton(FileData parData) : this()
        {
            SetData(parData);
        }

        //============================================================

        public void SetData(FileData parData)
        {
            _data = parData;

            UpdateLastAccessedTime();

            _fileIcon.Source = _data.GetIconFile()?.ToImageSource();
            _fileName.Text = string.IsNullOrEmpty(_data.Title) ? _data.GetFileName() : _data.Title;
            if(string.IsNullOrEmpty(_data.Description))
            {
                ((UIElement)_runButton.ToolTip).Visibility = Visibility.Collapsed;
            }
            else
            {
                ((UIElement)_runButton.ToolTip).Visibility = Visibility.Visible;
                _description.Text = _data.Description;
            }
            if (string.IsNullOrEmpty(_fileName.Text))
            {
                _fileName.Text = "(Файл не найден)";
            }
        }

        //============================================================

        public void UpdateLastAccessedTime()
        {
            DateTime lastAccessedTime = _data.GetLastAccessedTime();

            if(lastAccessedTime == DateTime.MinValue)
            {
                _lastAccessedTime.Text = "";
                return;
            }
            TimeSpan timeSpan = DateTime.Now - _data.GetLastAccessedTime();
            
            if (DateTime.Now.Day == lastAccessedTime.Day)
            {
              _lastAccessedTime.Text = $"{timeSpan.Hours} ч. назад";
            }
            else if (DateTime.Now.Day - 1 == lastAccessedTime.Day)
            {
              _lastAccessedTime.Text = $"вчера";
            }
            else if (timeSpan.Days < 7)
            {
              _lastAccessedTime.Text = $"{timeSpan.Days} дн. назад";
            }
            else if (timeSpan.Days / 7 < 6)
            {
              _lastAccessedTime.Text = $"{timeSpan.Days / 7} нед. назад";
            }
            else if (DateTime.Now.Year != lastAccessedTime.Year)
            {
              _lastAccessedTime.Text = $"{lastAccessedTime.Year} год";
            }
            else if (DateTime.Now.Month != lastAccessedTime.Month)
            {
              _lastAccessedTime.Text = $"{DateTime.Now.Month - lastAccessedTime.Month} мес. назад";
            }
        }

        private void _runButton_Click(object sender, RoutedEventArgs e)
        {
            RunClick?.Invoke(this);
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
            _lastAccessedTime.Visibility = _removeButton.Visibility = _editButton.Visibility = Visibility.Visible;
        }

        private void UserControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _lastAccessedTime.Visibility = _removeButton.Visibility = _editButton.Visibility = Visibility.Hidden;
        }
    }
}