using System;
using System.Data.SQLite;

namespace TrainerDatabase
{
    /// <summary>
    /// Работа с таблицей "Команды"
    /// </summary>
    public static class DatabaseTeamsTable
    {
        public const string TABLE_NAME = "Teams";
        public const string COLUMN_ID = "Id";
        public const string COLUMN_NAME = "Name";
        public const string COLUMN_EMBLEM = "Emblem";

        /// <summary>
        /// Извлечь команду
        /// </summary>
        /// <param name="parId">Айди</param>
        public static Team GetTeam(int parId)
        {
            string command = $"SELECT * FROM {TABLE_NAME} " +
                $"WHERE {COLUMN_ID}=@id";

            SQLiteDataReader dataReader = 
                Database.ExecuteReader(command, new SQLiteParameter("@id", parId));

            if(dataReader.HasRows)
            {
                if(dataReader.Read())
                {
                    Team team = new Team()
                    {
                        Id = Convert.ToInt32(dataReader[COLUMN_ID]),
                        Name = Convert.ToString(dataReader[COLUMN_NAME]),
                        Emblem = Convert.ToString(dataReader[COLUMN_EMBLEM])
                    };

                    return team;
                }
            }

            return null;
        }
    }
}
