using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace Coursework.Model
{
    public static class TagDataUtils
    {
        public static event Action<int> TagRemoved;
        public static event Action<TagData> TagAdded;

        public static bool AddTagData(ref TagData parData)
        {
            bool res = false;

            using (SQLiteConnection connect = new SQLiteConnection($"Data Source=database.db;Version=3"))
            {
                string commandText = "INSERT INTO Tags (Title) values(@title)";

                SQLiteCommand command = new SQLiteCommand(commandText, connect);
                command.Parameters.AddWithValue("@title", parData.Title);

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

            if (res)
            {
                TagAdded?.Invoke(parData);
            }

            return res;
        }

        public static bool RemoveTagData(int parId)
        {
            bool res = false;

            using (SQLiteConnection connect = new SQLiteConnection($"Data Source=database.db;Version=3"))
            {
                string commandText = $"DELETE FROM Tags WHERE Id=@id";

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

            if (res)
            {
                TagRemoved?.Invoke(parId);
            }

            return res;
        }

        public static List<TagData> GetListTagData()
        {
            List<TagData> datas = new List<TagData>();

            using (SQLiteConnection connect = new SQLiteConnection($"Data Source=database.db;Version=3"))
            {
                string commandText = $"SELECT * FROM Tags";

                SQLiteCommand command = new SQLiteCommand(commandText, connect);

                connect.Open();

                SQLiteDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        datas.Add(new TagData()
                        {
                            Id = Convert.ToInt32(dataReader["Id"]),
                            Title = Convert.ToString(dataReader["Title"])
                        });
                    }
                }

                connect.Close();
            }

            return datas;
        }

        public static List<TagData> GetListTagData(HashSet<int> parIds)
        {
            List<TagData> datas = new List<TagData>();

            using (SQLiteConnection connect = new SQLiteConnection($"Data Source=database.db;Version=3"))
            {
                string commandText = $"SELECT * FROM Tags WHERE Id=@id";

                connect.Open();


                foreach (var tagId in parIds)
                {
                    SQLiteCommand command = new SQLiteCommand(commandText, connect);
                    command.Parameters.AddWithValue("@id", tagId);
                    SQLiteDataReader dataReader = command.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        if (dataReader.Read())
                        {
                            datas.Add(new TagData()
                            {
                                Id = Convert.ToInt32(dataReader["Id"]),
                                Title = Convert.ToString(dataReader["Title"])
                            });
                        }
                    }
                }

                connect.Close();
            }

            return datas;
        }

        public static bool GetTagData(int parId, out TagData outData)
        {
            outData = default;
            bool res = false;

            using (SQLiteConnection connect = new SQLiteConnection($"Data Source=database.db;Version=3"))
            {
                string commandText = $"SELECT * FROM Tags WHERE Id=@id";

                SQLiteCommand command = new SQLiteCommand(commandText, connect);
                command.Parameters.AddWithValue("@id", parId);

                connect.Open();

                SQLiteDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    if (dataReader.Read())
                    {
                        outData = new TagData()
                        {
                            Id = Convert.ToInt32(dataReader["Id"]),
                            Title = Convert.ToString(dataReader["Title"])
                        };
                        res = true;
                    }
                }

                connect.Close();
            }

            return res;
        }
    }
}
