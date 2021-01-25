using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Coursework
{
  /// <summary>
  /// Логика взаимодействия для FileShortcut.xaml
  /// </summary>
  public partial class FileShortcut : UserControl
  {
    private FileData _data;

    //============================================================

    public FileData Data
    {
      get { return _data; }
      set { SetData(value);}
    }

    //============================================================

    /// <summary>
    /// Конструктор поу молчанию
    /// </summary>
    public FileShortcut()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="parData">Данные о файле</param>
    public FileShortcut(FileData parData) : this()
    {
      SetData(parData);
    }

    //============================================================

    public void SetData(FileData parData)
    {
      _data = parData;

      UpdateLastAccessedTime();

      _fileIcon.Source = _data.GetIconFile()?.ToImageSource();
      _fileName.Text = _data.GetFileName();
    }

    //============================================================

    public void UpdateLastAccessedTime()
    {
      DateTime lastAccessedTime = _data.GetLastAccessedTime();
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

    //============================================================

    public void RunFile()
    {
      if (_data.FileExists())
      {
        Process.Start(_data.LocalPath);
      }
    }

    public void EditFile()
    {
      if (_data.FileExists())
      {
        Process.Start(_data.LocalPath);
      }
    }

    public void RemuveFile()
    {

    }

    //============================================================

    private void _runButton_OnClick(object parSender, RoutedEventArgs parE)
    {
      RunFile();
    }
  }
}
