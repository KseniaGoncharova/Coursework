using System.Data.SQLite;

namespace TrainerDatabase
{
    /// <summary>
    /// Работа с базой данных
    /// </summary>
    public static class Database
    {
        /// <summary>
        /// Путь до файла
        /// </summary>
        public static string Path { get; } = "Database.db";

        /// <summary>
        /// Версия БД
        /// </summary>
        public static string Version { get; } = "3";

        /// <summary>
        /// Соединение
        /// </summary>
        public static SQLiteConnection Connection { get; } 
            = new SQLiteConnection($"Data Source={Path};Version={Version}");

        /// <summary>
        /// Конструктор
        /// </summary>
        static Database()
        {
            Connection.Open();
        }

        /// <summary>
        /// Создание команды
        /// </summary>
        /// <param name="parCommand">Команда</param>
        /// <param name="parParameters">Параметры</param>
        public static SQLiteCommand CreateCommand(string parCommand, params SQLiteParameter[] parParameters)
        {
            SQLiteCommand command = new SQLiteCommand(parCommand, Connection);
            command.Parameters.AddRange(parParameters);
            return command;
        }

        /// <summary>
        /// Выполнить не запрос
        /// </summary>
        /// <param name="parCommand">Команда</param>
        /// <param name="parParameters">Параметры</param>
        /// <returns>Количество затронутых строк</returns>
        public static int ExecuteNonQuery(string parCommand, params SQLiteParameter[] parParameters)
        {
            SQLiteCommand command = CreateCommand(parCommand, parParameters);
            return command.ExecuteNonQuery();
        }
        
        /// <summary>
        /// Выполнить запрос
        /// </summary>
        /// <param name="parCommand">Команда</param>
        /// <param name="parParameters">Параметры</param>
        public static SQLiteDataReader ExecuteReader(string parCommand, params SQLiteParameter[] parParameters)
        {
            SQLiteCommand command = CreateCommand(parCommand, parParameters);
            return command.ExecuteReader();
        }
    }
}
