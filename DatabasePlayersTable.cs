using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace TrainerDatabase
{
    /// <summary>
    /// Работа с таблицей "Команды"
    /// </summary>
    public static class DatabasePlayersTable
    {
        public const string TABLE_NAME = "Players";
        public const string COLUMN_ID = "Id";
        public const string COLUMN_NUMBER = "Number";
        public const string COLUMN_NAME = "Name";
        public const string COLUMN_SECONDNAME = "SecondName";
        public const string COLUMN_MIDDLENAME = "MiddleName";
        public const string COLUMN_YEARSOLD = "YearsOld";

        public const string COLUMN_TEAMID = "TeamId";
        public const string COLUMN_POSITION = "Position";
        public const string COLUMN_KICK = "Kick";
        public const string COLUMN_PASS = "Pass";
        public const string COLUMN_STROKE = "Stroke";
        public const string COLUMN_SELECTION = "Selection";
        public const string COLUMN_CATCHING = "Catching";

        /// <summary>
        /// Добавить игрока
        /// </summary>
        /// <param name="parPlayer">Игрок</param>
        public static bool AddPlayer(Player parPlayer)
        {
            string command = $"INSERT INTO {TABLE_NAME} " +
                $"({COLUMN_NUMBER}, {COLUMN_NAME}, {COLUMN_SECONDNAME}, {COLUMN_MIDDLENAME}, {COLUMN_YEARSOLD}, " +
                $"{COLUMN_TEAMID}, {COLUMN_POSITION}, {COLUMN_KICK}, {COLUMN_PASS}, " +
                $"{COLUMN_STROKE}, {COLUMN_SELECTION}, {COLUMN_CATCHING}) " +
                $"values(@number, @name, @secondname, @middlename, @yearsold, " +
                $"@teamid, @position, @kick, @pass, @stroke, @selection, @catching)";

            SQLiteParameter[] parameters = new SQLiteParameter[]
            {
                new SQLiteParameter("@number", parPlayer.Number),
                new SQLiteParameter("@name", parPlayer.Name),
                new SQLiteParameter("@secondname", parPlayer.SecondName),
                new SQLiteParameter("@middlename", parPlayer.MiddleName),
                new SQLiteParameter("@yearsold", parPlayer.YearsOld),
                new SQLiteParameter("@teamid", parPlayer.TeamId),

                new SQLiteParameter("@position", parPlayer.Position),
                new SQLiteParameter("@kick", parPlayer.Kick),
                new SQLiteParameter("@pass", parPlayer.Pass),
                new SQLiteParameter("@stroke", parPlayer.Stroke),
                new SQLiteParameter("@selection", parPlayer.Selection),
                new SQLiteParameter("@catching", parPlayer.Catching),
            };

            return Database.ExecuteNonQuery(command, parameters) == 1;
        }

        /// <summary>
        /// Удалить игрока
        /// </summary>
        /// <param name="parId">Айди</param>
        public static bool RemovePlayer(int parId)
        {
            string command = $"DELETE FROM {TABLE_NAME} WHERE {COLUMN_ID}=@id";
            return Database.ExecuteNonQuery(command, new SQLiteParameter("@id", parId)) == 1;
        }

        /// <summary>
        /// Изменить данные об игроке (имя, фамилия, отчество, возраст)
        /// </summary>
        /// <param name="parPlayer">Игрок</param>
        public static bool EditPlayerData(Player parPlayer)
        {
            string command = $"UPDATE {TABLE_NAME} SET " +
                $"{COLUMN_NUMBER}=@number, {COLUMN_NAME}=@name, {COLUMN_SECONDNAME}=@secondname, " +
                $"{COLUMN_MIDDLENAME}=@middlename, {COLUMN_YEARSOLD}=@yearsold, " +
                $"{COLUMN_TEAMID}=@teamid " +
                $"WHERE {COLUMN_ID}=@id";

            SQLiteParameter[] parameters = new SQLiteParameter[]
            {
                new SQLiteParameter("@id", parPlayer.Id),
                new SQLiteParameter("@number", parPlayer.Number),
                new SQLiteParameter("@name", parPlayer.Name),
                new SQLiteParameter("@secondname", parPlayer.SecondName),
                new SQLiteParameter("@middlename", parPlayer.MiddleName),
                new SQLiteParameter("@yearsold", parPlayer.YearsOld),
                new SQLiteParameter("@teamid", parPlayer.TeamId),
            };

            return Database.ExecuteNonQuery(command, parameters) == 1;
        }

        /// <summary>
        /// Изменить характеристики игрока
        /// </summary>
        /// <param name="parPlayer">Игрок</param>
        public static bool EditPlayerCharacteristics(Player parPlayer)
        {
            string command = $"UPDATE {TABLE_NAME} SET " +
                $"{COLUMN_POSITION}=@position, {COLUMN_KICK}=@kick, " +
                $"{COLUMN_PASS}=@pass, {COLUMN_STROKE}=@stroke, " +
                $"{COLUMN_SELECTION}=@selection, {COLUMN_CATCHING}=@catching " +
                $"WHERE {COLUMN_ID}=@id";

            SQLiteParameter[] parameters = new SQLiteParameter[]
            {
                new SQLiteParameter("@id", parPlayer.Id),
                new SQLiteParameter("@position", parPlayer.Position),
                new SQLiteParameter("@kick", parPlayer.Kick),
                new SQLiteParameter("@pass", parPlayer.Pass),
                new SQLiteParameter("@stroke", parPlayer.Stroke),
                new SQLiteParameter("@selection", parPlayer.Selection),
                new SQLiteParameter("@catching", parPlayer.Catching),
            };

            return Database.ExecuteNonQuery(command, parameters) == 1;
        }

        /// <summary>
        /// Извлечь игроков команды
        /// </summary>
        /// <param name="parTeamId">Айди команды</param>
        public static Player[] GetPlayers(int parTeamId)
        {
            string command = $"SELECT * FROM {TABLE_NAME} " +
                $"WHERE {parTeamId}=@teamid";

            SQLiteDataReader dataReader =
                Database.ExecuteReader(command, new SQLiteParameter("@teamid", parTeamId));

            if (dataReader.HasRows)
            {
                List<Player> players = new List<Player>();
                while (dataReader.Read())
                {
                    players.Add(new Player()
                    {
                        Id = Convert.ToInt32(dataReader[COLUMN_ID]),
                        Number = Convert.ToInt32(dataReader[COLUMN_NUMBER]),
                        Name = Convert.ToString(dataReader[COLUMN_NAME]),
                        SecondName = Convert.ToString(dataReader[COLUMN_SECONDNAME]),
                        MiddleName = Convert.ToString(dataReader[COLUMN_MIDDLENAME]),
                        YearsOld = Convert.ToInt32(dataReader[COLUMN_YEARSOLD]),
                        TeamId = Convert.ToInt32(dataReader[COLUMN_TEAMID]),

                        Position = (Position)Convert.ToInt32(dataReader[COLUMN_POSITION]),
                        Kick = Convert.ToInt32(dataReader[COLUMN_KICK]),
                        Pass = Convert.ToInt32(dataReader[COLUMN_PASS]),
                        Stroke = Convert.ToInt32(dataReader[COLUMN_STROKE]),
                        Selection = Convert.ToInt32(dataReader[COLUMN_SELECTION]),
                        Catching = Convert.ToInt32(dataReader[COLUMN_CATCHING]),
                    });

                }

                return players.ToArray();
            }

            return new Player[0];
        }
        /// <summary>
        /// Извлечь игрока
        /// </summary>
        /// <param name="parId">Айди</param>
        public static Player GetPlayer(int parId)
        {
            string command = $"SELECT * FROM {TABLE_NAME} " +
                $"WHERE {COLUMN_ID}=@id";

            SQLiteDataReader dataReader = 
                Database.ExecuteReader(command, new SQLiteParameter("@id", parId));

            if(dataReader.HasRows)
            {
                if(dataReader.Read())
                {
                    Player player = new Player()
                    {
                        Id = Convert.ToInt32(dataReader[COLUMN_ID]),
                        Number = Convert.ToInt32(dataReader[COLUMN_NUMBER]),
                        Name = Convert.ToString(dataReader[COLUMN_NAME]),
                        SecondName = Convert.ToString(dataReader[COLUMN_SECONDNAME]),
                        MiddleName = Convert.ToString(dataReader[COLUMN_MIDDLENAME]),
                        YearsOld = Convert.ToInt32(dataReader[COLUMN_YEARSOLD]),
                        TeamId = Convert.ToInt32(dataReader[COLUMN_TEAMID]),

                        Position = (Position)Convert.ToInt32(dataReader[COLUMN_POSITION]),
                        Kick = Convert.ToInt32(dataReader[COLUMN_KICK]),
                        Pass = Convert.ToInt32(dataReader[COLUMN_PASS]),
                        Stroke = Convert.ToInt32(dataReader[COLUMN_STROKE]),
                        Selection = Convert.ToInt32(dataReader[COLUMN_SELECTION]),
                        Catching = Convert.ToInt32(dataReader[COLUMN_CATCHING]),
                    };

                    return player;
                }
            }

            return null;
        }
    }
}
