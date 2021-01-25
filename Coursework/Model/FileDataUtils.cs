using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace Coursework.Model
{
    public static class FileDataUtils
    {
        public static event Action<FileData> FileAdded;
        public static event Action<FileData> FileEdited;

        public static bool AddFileData(ref FileData parData)
        {
            bool res = false;

            using (SQLiteConnection connect = new SQLiteConnection($"Data Source=database.db;Version=3"))
            {
                string commandText = "INSERT INTO Files (LocalPath, Title, Description, GroupId, Tags) values(@localPath, @title, @description, @group, @tags)";

                SQLiteCommand command = new SQLiteCommand(commandText, connect);
                command.Parameters.AddWithValue("@localPath", parData.LocalPath);
                command.Parameters.AddWithValue("@title", parData.Title);
                command.Parameters.AddWithValue("@description", parData.Description);
                command.Parameters.AddWithValue("@group", parData.Group);

                string tags = "";
                if (parData.Tags.Length > 0)
                {
                    for (int i = 0; i < parData.Tags.Length - 1; i++)
                    {
                        tags += $"{parData.Tags[i]},";
                    }
                    tags += parData.Tags[parData.Tags.Length - 1];
                }
                command.Parameters.AddWithValue("@tags", tags);

                connect.Open();        
                try
                {
                    res = command.ExecuteNonQuery() == 1;
                    //TODO: получить id
                    parData.Id = (int)connect.LastInsertRowId;
                }
                catch { }

                connect.Close();
            }

            if( res)
            {
                FileAdded?.Invoke(parData);
            }

            return res;
        }

        public static bool EditFileData(FileData parData)
        {
            bool res = false;

            using (SQLiteConnection connect = new SQLiteConnection($"Data Source=database.db;Version=3"))
            {
                string commandText = "UPDATE Files SET LocalPath=@localPath, Title=@title, Description=@description, GroupId=@group, Tags=@tags WHERE Id=@id";

                SQLiteCommand command = new SQLiteCommand(commandText, connect);
                command.Parameters.AddWithValue("@id", parData.Id);
                command.Parameters.AddWithValue("@localPath", parData.LocalPath);
                command.Parameters.AddWithValue("@title", parData.Title);
                command.Parameters.AddWithValue("@description", parData.Description);
                command.Parameters.AddWithValue("@group", parData.Group);

                string tags = "";
                if (parData.Tags.Length > 0)
                {
                    for (int i = 0; i < parData.Tags.Length - 1; i++)
                    {
                        tags += $"{parData.Tags[i]},";
                    }
                    tags += parData.Tags[parData.Tags.Length - 1];
                }
                command.Parameters.AddWithValue("@tags", tags);

                connect.Open();      
                try
                {
                    res = command.ExecuteNonQuery() == 1;
                }
                catch { }
                connect.Close();
            }
            
            if( res)
            {
                FileEdited?.Invoke(parData);
            }

            return res;
        }


        public static bool RemoveFileData(int parId)
        {
            bool res = false;

            using (SQLiteConnection connect = new SQLiteConnection($"Data Source=database.db;Version=3"))
            {
                string commandText = $"DELETE FROM Files WHERE Id=@id";

                SQLiteCommand command = new SQLiteCommand(commandText, connect);
                command.Parameters.AddWithValue("@id", parId);

                connect.Open();   
                try
                {
                    res = command.ExecuteNonQuery() == 1;
                }
                catch { }
                connect.Close();
            }

            return res;
        }

        public static List<FileData> GetListFileData()
        {
            List<FileData> datas = new List<FileData>();

            using (SQLiteConnection connect = new SQLiteConnection($"Data Source=database.db;Version=3"))
            {
                string commandText = $"SELECT * FROM Files";

                SQLiteCommand command = new SQLiteCommand(commandText, connect);

                connect.Open();

                SQLiteDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        datas.Add(GetFileData(dataReader));
                    }
                }

                connect.Close();
            }

            return datas;
        }

        public static List<FileData> GetListFileData(int parGroup)
        {
            List<FileData> datas = new List<FileData>();

            using (SQLiteConnection connect = new SQLiteConnection($"Data Source=database.db;Version=3"))
            {
                string commandText = $"SELECT * FROM Files WHERE Group=@group";

                SQLiteCommand command = new SQLiteCommand(commandText, connect);
                command.Parameters.AddWithValue("@group", parGroup);

                connect.Open();

                SQLiteDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        datas.Add(GetFileData(dataReader));
                    }
                }

                connect.Close();
            }

            return datas;
        }

        public static bool GetFileData(int parId, out FileData outData)
        {
            outData = default;
            bool res = false;

            using (SQLiteConnection connect = new SQLiteConnection($"Data Source=database.db;Version=3"))
            {
                string commandText = $"SELECT * FROM Files WHERE Id=@id";

                SQLiteCommand command = new SQLiteCommand(commandText, connect);
                command.Parameters.AddWithValue("@id", parId);

                connect.Open();

                SQLiteDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    if (dataReader.Read())
                    {
                        outData = GetFileData(dataReader);
                        res = true;
                    }
                }

                connect.Close();
            }

            return res;
        }


        public static FileData GetFileData(SQLiteDataReader parDataReader)
        {
            List<int> tags = new List<int>();
            foreach (var tag in parDataReader["Tags"].ToString().Split(','))
            {
                if (!string.IsNullOrEmpty(tag))
                {
                    tags.Add(Convert.ToInt32(tag));
                }
            }
            return new FileData()
            {
                Id = Convert.ToInt32(parDataReader["Id"]),
                LocalPath = Convert.ToString(parDataReader["LocalPath"]),
                Title = Convert.ToString(parDataReader["Title"]),
                Description = Convert.ToString(parDataReader["Description"]),
                Group = Convert.ToInt32(parDataReader["GroupId"]),
                Tags = tags.ToArray()
            };
        }

    }
}
