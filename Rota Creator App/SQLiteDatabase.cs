using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Collections.ObjectModel;
using System.Reflection;
using System.IO;

namespace Rota_Creator_App
{
    public interface ISQLiteable
    {
        string SQLDataDefinition();
        bool SQLInsert(SQLiteConnection connection);
        bool SQLUpdate(SQLiteConnection connection);
        bool SQLDelete(SQLiteConnection connection);
        void SQLParse(SQLiteDataReader reader);
    }

    public class SQLiteDatabase
    {
        public static SQLiteDatabase Global { get; set; }

        public string ConnectionString { get; protected set; }
        SQLiteConnection connection;

        public SQLiteDatabase(string connectionString)
        {
            ConnectionString = connectionString;
            connection = new SQLiteConnection(ConnectionString);

            if (Global == null)
                Global = this;
        }

        public bool RunCommand(string commandString)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = commandString;
            return (command.ExecuteNonQuery() != 0);
        }

        public bool CreateTable<T>(string tableName) where T : ISQLiteable
        {
            SQLiteCommand command = connection.CreateCommand();
            //command.CommandText = $"CREATE TABLE {tableName} ({ T.SQLDataDefinition() })";
            return (command.ExecuteNonQuery() != 0);
        }
        public bool DeleteTable(string tableName)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"DROP TABLE [IF EXISTS] {tableName}";
            return (command.ExecuteNonQuery() != 0);
        }
        
        public List<T> Query<T>(string table, string rows = "*", string condition = "") where T : ISQLiteable, new()
        {
            List<T> items = new List<T>();
            SQLiteCommand command = connection.CreateCommand();

            if (condition.Length == 0)
                command.CommandText = $"SELECT {rows} FROM {table} WHERE {condition}";
            else
                command.CommandText = $"SELECT {rows} FROM {table}";

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                if (!reader.HasRows)
                    return items;

                while (reader.Read())
                {
                    T item = new T();
                    item.SQLParse(reader);
                    items.Add(item);
                }
            }

            return items;
        }
        public bool Insert(ISQLiteable item)
        {
            return item.SQLInsert(connection);
        }
        public bool Update(ISQLiteable item)
        {
            return item.SQLUpdate(connection);
        }
        public bool Delete(ISQLiteable item)
        {
            return item.SQLDelete(connection);
        }

        public static bool CreateDatabase(string name)
        {
            if (!File.Exists(name))
            {
                SQLiteConnection.CreateFile("rotacreator.db");
                return true;
            }

            return false;
        }
    }
}