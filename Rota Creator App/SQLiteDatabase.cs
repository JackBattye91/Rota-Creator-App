using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Collections.ObjectModel;

namespace Rota_Creator_App
{
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
        public string RunQuery(string queryString)
        {
            SQLiteCommand query = connection.CreateCommand();
            query.CommandText = queryString;
            string retString = "";

            int column = 0;

            using (SQLiteDataReader reader = query.ExecuteReader())
            {
                while (reader.Read())
                {
                    retString += reader.GetName(column) + " : " + reader.GetValue(column) + ";";
                    row++;
                }
            }
        }

        public bool CreateTable<T>()
        {
            SQLiteCommand createTable = connection.CreateCommand();

            createTable.CommandText = $"CREATE TABLE {T.Name}("
            for(int p = 0; p < T.GetProperties().Count; p++)
            {
                ProperertyInfo prop = T.GetProperties()[p];

                switch(prop.GetType())
                {
                    case typeof(byte):
                    case typeof(sbyte):
                    case typeof(char):
                    case typeof(short):
                    case typeof(ushort):
                    case typeof(int):
                    case typeof(uint):
                    case typeof(long):
                    case typeof(ulong):
                        if (p == 0)
                            createTable.CommandText += $"{prop.Name} INTEGER PRIMARY KEY AUTOINCREMENT, ";
                        else
                            createTable.CommandText += $"{prop.Name} INTEGER, ";
                        break;

                    case typeof(float):
                    case typeof(double):
                        createTable.CommandText += $"{prop.Name} REAL, ";
                        break;

                    case typeof(char):
                    case typeof(string):
                        createTable.CommandText += $"{prop.Name} TEXT, ";
                        break;

                    case typeof(bool):
                    case typeof(decimal):
                    case typeof(DateTime):
                        createTable.CommandText += $"{prop.Name} NUMERIC, ";
                        break;

                    default:
                        createTable.CommandText += $"{prop.Name} BLOB, ";
                        break;
                }
            }

            createTable.CommandText += ")";

            return (createTable.ExecuteNonQuery() != 0);
        }

        public bool DeleteTable(string tableName)
        {
            SQLiteCommand deleteTable = connection.CreateCommand();
            deleteTable.CommandText = $"DROP TABLE [IF EXISTS] {tableName}";
            return (deleteTable.ExecuteNonQuery() != 0);
        }

        public List<T> Query<T>(string queryString = null)
        {
            List<T> items = new List<T>();

            if (queryString == null)
                queryString = $"SELECT * FROM {T.Name}";

            SQLiteCommand query = connection.CreateCommand();
            query.CommandText = queryString;

            using (SQLiteDataReader reader = query.ExecuteReader())
            {
                while (reader.Read())
                {
                    T newT = new T();

                    for(int p = 0; p < T.GetProperties().Count; p++)
                    {
                        ProperertyInfo prop = T.GetProperties()[p];

                        switch(prop.GetType())
                        {
                            case typeof(byte):
                                prop.SetValue(newT, reader.GetByte(p));
                                break;

                            case typeof(sbyte):
                                prop.SetValue(newT, (sbyte)reader.GetByte(p));
                                break;

                            case typeof(char):
                                prop.SetValue(newT, reader.GetChar(p));
                                break;

                            case typeof(short):
                                prop.SetValue(newT, reader.GetInt16(p));
                                break;

                            case typeof(ushort):
                                prop.SetValue(newT, (ushort)reader.GetInt16(p));
                                break;

                            case typeof(int):
                                prop.SetValue(newT, reader.GetInt32(p));
                                break;

                            case typeof(uint):
                                prop.SetValue(newT, (uint)reader.GetInt32(p));
                                break;

                            case typeof(long):
                                prop.SetValue(newT, reader.GetInt64(p));
                                break;
                                
                            case typeof(ulong):
                                prop.SetValue(newT, (ulong)reader.GetInt64(p));
                                break;

                            case typeof(float):
                                prop.SetValue(newT, reader.GetFloat(p));
                                break;

                            case typeof(double):
                                prop.SetValue(newT, reader.GetDouble(p));
                                break;

                            case typeof(string):
                                prop.SetValue(newT, reader.GetString(p));
                                break;

                            case typeof(bool):
                                prop.SetValue(newT, reader.GetBoolean(p));
                                break;

                            case typeof(decimal):
                                prop.SetValue(newT, reader.GetDecimal(p));
                                break;

                            case typeof(DateTime):
                                prop.SetValue(newT, reader.GetDateTime(p));
                                break;

                            default:
                                break;
                        }

                        row++;
                    }

                    items.Add(newT);
                }
            }

            return items;
        }

        public bool Insert<T>(T item)
        {
            SQLiteCommand insert = connection.CreateCommand();
            insert.CommandText = $"INSERT INTO {T.Name} (";
            for(int p = 0; p < T.GetProperties().Count; p++)
            {
                ProperertyInfo prop = T.GetProperties()[p];
                insert.CommandText += $"{prop.Name}, ";
            }

            insert.CommandText += ") VALUES (";

            for(int p = 0; p < T.GetProperties().Count; p++)
            {
                ProperertyInfo prop = T.GetProperties()[p];
                insert.CommandText += $"{prop.GetValue(item)}, ";
            }

            insert.CommandText += ")";

            return (insert.ExecuteNonQuery() != 0);
        }

        public bool Update<T>(T item)
        {
            SQLiteCommand update = connection.CreateCommand();
            update.CommandText = $"UPDATE {T.Name} SET ";
            for(int p = 0; p < T.GetProperties().Count; p++)
            {
                ProperertyInfo prop = T.GetProperties()[p];
                update.CommandText += $"{prop.Name} = {prop.GetValue},";
            }

            insert.CommandText += $" WHERE {T.GetProperties()[0].Name} = {T.GetProperties()[0].GetValue(item)}";

            return (insert.ExecuteNonQuery() != 0);
        }

        public bool Delete<T>(T item)
        {
            SQLiteCommand delete = connection.CreateCommand();
            delete.CommandText = $"DELETE FROM {T.Name} WHERE {T.GetProperties()[0].Name} = {T.GetProperties()[0].GetValue(item)}";
            return (delete.ExecuteNonQuery() != 0);
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