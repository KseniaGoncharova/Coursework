using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Coursework.Model
{
    public static class GroupDataUtils
    {
        public static bool AddGroupData(ref GroupData parData)
        {
            bool res = false;

            using (SQLiteConnection connect = new SQLiteConnection($"Data Source=database.db;Version=3"))
            {
                string commandText = "INSERT INTO Groups (Title) values(@title)";

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

            return res;
        }

        public static bool EditGroupData(GroupData parData)
        {
            bool res = false;

            using (SQLiteConnection connect = new SQLiteConnection($"Data Source=database.db;Version=3"))
            {
                string commandText = "UPDATE Groups SET Title=@title WHERE Id=@id";

                SQLiteCommand command = new SQLiteCommand(commandText, connect);
                command.Parameters.AddWithValue("@id", parData.Id);
                command.Parameters.AddWithValue("@title", parData.Title);

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


        public static bool RemoveGroupData(int parId)
        {
            bool res = false;

            using (SQLiteConnection connect = new SQLiteConnection($"Data Source=database.db;Version=3"))
            {
                string commandText = $"DELETE FROM Groups WHERE Id=@id";

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

        public static List<GroupData> GetListGroupData()
        {
            List<GroupData> datas = new List<GroupData>();

            using (SQLiteConnection connect = new SQLiteConnection($"Data Source=database.db;Version=3"))
            {
                string commandText = $"SELECT * FROM Groups";

                SQLiteCommand command = new SQLiteCommand(commandText, connect);

                connect.Open();

                SQLiteDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        datas.Add(new GroupData()
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

        public static bool GetGroupData(int parId, out GroupData outData)
        {
            outData = default;
            bool res = false;

            using (SQLiteConnection connect = new SQLiteConnection($"Data Source=database.db;Version=3"))
            {
                string commandText = $"SELECT * FROM Groups WHERE Id=@id";

                SQLiteCommand command = new SQLiteCommand(commandText, connect);
                command.Parameters.AddWithValue("@id", parId);

                connect.Open();

                SQLiteDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    if (dataReader.Read())
                    {
                        outData = new GroupData()
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
