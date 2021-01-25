using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Coursework
{
    /// <summary>
    /// Данные о файле
    /// </summary>
    public struct FileData
    {
        private string _bufFileName;
        private DateTime _bufLastAccessedTime;
        private Icon _bufIcon;

        private string _localPath;

        public int Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Путь к локальному файлу
        /// </summary>
        public string LocalPath
        {
            get { return _localPath; }
            set
            {
                _localPath = value;
                UpdateFileName();
                UpdateIconFile();
                UpdateLastAccessedTime();
            }
        }

        /// <summary>
        /// Группа
        /// </summary>
        public int Group { get; set; }

        /// <summary>
        /// Теги
        /// </summary>
        public int[] Tags { get; set; }

        //============================================================

        /// <summary>
        /// Файл существует
        /// </summary>
        public bool FileExists()
        {
            return File.Exists(LocalPath);
        }

        //============================================================

        /// <summary>
        /// Название файла
        /// </summary>
        public string GetFileName()
        {
            return _bufFileName;
        }

        public void UpdateFileName()
        {
            _bufFileName = Path.GetFileName(LocalPath);
        }

        /// <summary>
        /// Иконка файла
        /// </summary>
        public Icon GetIconFile()
        {
            return _bufIcon;
        }

        public void UpdateIconFile()
        {
            _bufIcon = FileExists() ? Icon.ExtractAssociatedIcon(LocalPath) : null;
        }

        /// <summary>
        /// Дата открытия
        /// </summary>
        public DateTime GetLastAccessedTime()
        {
            return _bufLastAccessedTime;
        }

        public void UpdateLastAccessedTime()
        {
            _bufLastAccessedTime = FileExists() ? File.GetLastAccessTime(LocalPath) : DateTime.MinValue;
        }

        //============================================================

        /// <summary>
        /// Имеет тег
        /// </summary>
        /// <param name="parTag">Тег</param>
        public bool HasTag(int parTag)
        {
            return Tags.Contains(parTag);
        }
    }
}
