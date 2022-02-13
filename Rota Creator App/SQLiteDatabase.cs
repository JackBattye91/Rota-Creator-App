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
        string SQLiteInsertScript();
        string SQLiteUpdateScript();
        string SQLiteDeleteScript();

        void SQLiteParse(SQLiteDataReader reader);
    }

    public class SQLiteDatabase
    {
        public static SQLiteDatabase Global { get; set; } = null;

        public string ConnectionString { get; protected set; }
        SQLiteConnection connection;


        public SQLiteDatabase(string connectionString)
        {
            ConnectionString = connectionString;
            connection = new SQLiteConnection(ConnectionString);
            connection.Open();

            if (Global == null)
                Global = this;
        }

        public bool RunCommand(string commandString)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = commandString;
            return command.ExecuteNonQuery() != 0;
        }

        public bool CreateTable(string tableName, string columnDefinitions)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"CREATE TABLE {tableName}({columnDefinitions})";
            return command.ExecuteNonQuery() != 0;
        }

        public bool DeleteTable(string tableName)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"DROP TABLE [IF EXISTS] {tableName}";
            return command.ExecuteNonQuery() != 0;
        }

        public bool TableExists(string tableName)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}'";
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    return true;
                }
            }
            return false;
        }

        public bool Insert<T>(T item) where T : ISQLiteable
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = item.SQLiteInsertScript();
            return (command.ExecuteNonQuery() != 0);
        }
        public bool Update<T>(T item) where T : ISQLiteable
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = item.SQLiteUpdateScript();
            return (command.ExecuteNonQuery() != 0);
        }
        public bool Delete<T>(T item) where T : ISQLiteable
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = item.SQLiteDeleteScript();
            return (command.ExecuteNonQuery() != 0);
        }

        public List<T> Query<T>(string table, string columns = "*", string condition = "") where T : ISQLiteable, new()
        {
            SQLiteCommand command = connection.CreateCommand();
            if (!condition.Equals(""))
                command.CommandText = $"SELECT {columns} FROM {table} WHERE {condition}";
            else
                command.CommandText = $"SELECT {columns} FROM {table}";

            List<T> list = new List<T>();
            T item = new T();

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                if (!reader.HasRows)
                    return list;

                while (reader.Read())
                {
                    item.SQLiteParse(reader);
                    list.Add(item);
                }
            }

            return list;
        }


        public static bool CreateDatabase(string name)
        {
            if (!File.Exists(name))
            {
                SQLiteConnection.CreateFile(name);
                return true;
            }

            return false;
        }
    }
    /*
    [AttributeUsage(AttributeTargets.Property)]  
    public class PrimaryKeyAttribute : Attribute  
    {
    }
    [AttributeUsage(AttributeTargets.Property)]  
    public class AutoIncrementAttribute : Attribute  
    {
    }

    public class SQLiteDatabase
    {
        public static SQLiteDatabase Global { get; set; } = null;

        public string ConnectionString { get; protected set; }
        SQLiteConnection connection;

        public SQLiteDatabase(string connectionString)
        {
            ConnectionString = connectionString;
            connection = new SQLiteConnection(ConnectionString);
            connection.Open();

            if (Global == null)
                Global = this;
        }

        public bool RunCommand(string commandString)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = commandString;
            return command.ExecuteNonQuery() != 0;
        }

        public bool CreateTable<T>() where T : new()
        {
            string dataDefinition = "";

            for(int p = 0; p < typeof(T).GetProperties().Count(); p++)
            {
                if (p != 0)
                    dataDefinition += ", ";

                PropertyInfo prop = typeof(T).GetProperties()[p];

                if (prop.PropertyType == typeof(byte) || prop.PropertyType == typeof(char) ||
                    prop.PropertyType == typeof(short) || prop.PropertyType == typeof(ushort) ||
                    prop.PropertyType == typeof(int) || prop.PropertyType == typeof(uint) ||
                    prop.PropertyType == typeof(long) || prop.PropertyType == typeof(ulong))
                {
                    dataDefinition += $"{prop.Name} INTEGER";
                }
                else if (prop.PropertyType == typeof(float) || prop.PropertyType == typeof(double))
                {
                    dataDefinition += $"{prop.Name} REAL";
                }
                else if (prop.PropertyType == typeof(string))
                {
                    dataDefinition += $"{prop.Name} TEXT";
                }
                else if (prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(DateTime))
                {
                    dataDefinition += $"{prop.Name} NUMERIC";
                }
                else
                {
                    dataDefinition += $"{prop.Name} BLOB";
                }
               
                if (prop.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).Count() != 0)
                {
                    dataDefinition += " PRIMARY KEY";
                }
                if (prop.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).Count() != 0)
                {
                    dataDefinition += " AUTOINCREMENT";
                }
            }

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"CREATE TABLE {typeof(T).Name} ({dataDefinition})";
            return command.ExecuteNonQuery() != 0;
        }
        public bool DeleteTable<T>(string tableName)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"DROP TABLE [IF EXISTS] {typeof(T).Name}";
            return (command.ExecuteNonQuery() != 0);
        }
        public bool TableExists<T>()
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{typeof(T).Name}'";
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    return true;
                }
            }
            return false;
        }

        public bool Insert<T>(T item)
        {
            string propertyNames = "";
            string propertyValues = "";
            string identifier = "";
            for(int p = 0; p < typeof(T).GetProperties().Count(); p++)
            {
                PropertyInfo prop = typeof(T).GetProperties()[p];

                if (p != 0)
                {
                    propertyNames += ", ";
                    propertyValues += ", ";
                }

                propertyNames += prop.Name;

                if( prop.PropertyType== typeof(byte) || prop.PropertyType == typeof(char) || 
                    prop.PropertyType == typeof(short) || prop.PropertyType == typeof(ushort) ||
                    prop.PropertyType == typeof(int)|| prop.PropertyType == typeof(uint) ||
                    prop.PropertyType == typeof(long) || prop.PropertyType == typeof(ulong) ||
                    prop.PropertyType == typeof(float) || prop.PropertyType == typeof(double) ||
                    prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(DateTime))
                {
                    propertyValues += prop.GetValue(item).ToString();

                    if (prop.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).Count() != 0)
                    {
                        identifier += $"{prop.Name} = {prop.GetValue(item).ToString()}";
                    }
                }
                else if (prop.PropertyType == typeof(string))
                {
                    propertyValues += $"'{prop.GetValue(item) as string}'";

                    if (prop.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).Count() != 0)
                    {
                        identifier += $"{prop.Name} = '{prop.GetValue(item) as string}'";
                    }
                }
                else if (prop.PropertyType == typeof(List<>))
                {
                    prop.GetValue(item).ToString();
                    propertyValues += $""
                }
                else
                {
                    propertyValues += $"'{prop.GetValue(item)}'";
                }
            }

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"INSERT INTO {typeof(T).Name} ({propertyNames}) VALUES ({propertyValues})";
            return command.ExecuteNonQuery() != 0;
        }
        public bool Update<T>(T item)
        {
            string propertyValues = "";
            string identifier = "";

            Dictionary<string, string> properties = new Dictionary<string, string>();
            for(int p = 0; p < typeof(T).GetProperties().Count(); p++)
            {
                PropertyInfo prop = typeof(T).GetProperties()[p];

                if( prop.PropertyType == typeof(byte) || prop.PropertyType == typeof(char) || 
                    prop.PropertyType == typeof(short) || prop.PropertyType == typeof(ushort) ||
                    prop.PropertyType == typeof(int)|| prop.PropertyType == typeof(uint) ||
                    prop.PropertyType == typeof(long) || prop.PropertyType == typeof(ulong) ||
                    prop.PropertyType == typeof(float) || prop.PropertyType == typeof(double) ||
                    prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(DateTime))
                {
                    if (prop.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).Count() != 0)
                        identifier += $"{prop.Name} = '{prop.GetValue(item).ToString()}'";
                    else
                        properties.Add(prop.Name, prop.GetValue(item).ToString());
                }
                else if (prop.PropertyType == typeof(string))
                {
                    if (prop.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).Count() != 0)
                    {
                        identifier += $"{prop.Name} = '{prop.GetValue(item) as string}'";
                    }
                    else
                        properties.Add(prop.Name, $"'{prop.GetValue(item)}'");
                }
                else
                {
                    string value = System.Text.Json.JsonSerializer.Serialize<object>(prop.GetValue(item).ToString());
                    properties.Add(prop.Name, $"'{value}'");
                }
            }

            foreach (KeyValuePair<string, string> keyValue in properties)
            {
                propertyValues += $"{keyValue.Key} = {keyValue.Value}, ";
            }

            if (propertyValues.EndsWith(", "))
                propertyValues = propertyValues.Remove(propertyValues.LastIndexOf(", "), 2);

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"UPDATE {typeof(T).Name} SET {propertyValues} WHERE {identifier}";
            return command.ExecuteNonQuery() != 0;
        }
        public bool Delete<T>(T item)
        {
            string identifier = "";
            for(int p = 0; p < typeof(T).GetProperties().Count(); p++)
            {
                PropertyInfo prop = typeof(T).GetProperties()[p];

                if( prop.PropertyType == typeof(byte) || prop.PropertyType == typeof(char) || 
                    prop.PropertyType == typeof(short) || prop.PropertyType == typeof(ushort) ||
                    prop.PropertyType == typeof(int)|| prop.PropertyType == typeof(uint) ||
                    prop.PropertyType == typeof(long) || prop.PropertyType == typeof(ulong) ||
                    prop.PropertyType == typeof(float) || prop.PropertyType == typeof(double) ||
                    prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(DateTime))
                {
                    if (prop.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).Count() != 0)
                    {
                        identifier += $"{prop.Name} = {prop.GetValue(item).ToString()}";
                    }
                }
                else if (prop.PropertyType == typeof(string))
                {
                    if (prop.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).Count() != 0)
                    {
                        identifier += $"{prop.Name} = '{prop.GetValue(item) as string}'";
                    }
                }
            }

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"DELETE FROM {typeof(T).Name} WHERE {identifier}";
            return command.ExecuteNonQuery() != 0;
        }

        public List<T> Query<T>(string columns = "*", string condition = "") where T : new()
        {
            List<T> items = new List<T>();
            SQLiteCommand command = connection.CreateCommand();

            if (condition.Length != 0)
                command.CommandText = $"SELECT {columns} FROM {typeof(T).Name} WHERE {condition}";
            else
                command.CommandText = $"SELECT {columns} FROM {typeof(T).Name}";

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                if (!reader.HasRows)
                    return null;

                while (reader.Read())
                {
                    T item = new T();

                    for(int p = 0; p < typeof(T).GetProperties().Count(); p++)
                    {
                        PropertyInfo prop = typeof(T).GetProperties()[p];

                        if (prop.PropertyType == typeof(byte))
                        {
                            byte value = reader.GetByte(reader.GetOrdinal(prop.Name));
                            prop.SetValue(item, value);
                        }
                        else if (prop.PropertyType == typeof(char))
                        {
                            char value = reader.GetChar(reader.GetOrdinal(prop.Name));
                            prop.SetValue(item, value);
                        }
                        else if (prop.PropertyType == typeof(short))
                        {
                            short value = reader.GetInt16(reader.GetOrdinal(prop.Name));
                            prop.SetValue(item, value);
                        }
                        else if (prop.PropertyType == typeof(ushort))
                        {
                            ushort value = (ushort)reader.GetInt16(reader.GetOrdinal(prop.Name));
                            prop.SetValue(item, value);
                        }
                        else if (prop.PropertyType == typeof(int))
                        {
                            int value = reader.GetInt32(reader.GetOrdinal(prop.Name));
                            prop.SetValue(item, value);
                        }
                        else if (prop.PropertyType == typeof(uint))
                        {
                            uint value = (uint)reader.GetInt32(reader.GetOrdinal(prop.Name));
                            prop.SetValue(item, value);
                        }
                        else if (prop.PropertyType == typeof(long))
                        {
                            long value = reader.GetInt64(reader.GetOrdinal(prop.Name));
                            prop.SetValue(item, value);
                        }
                        else if (prop.PropertyType == typeof(ulong))
                        {
                            ulong value = (ulong)reader.GetInt64(reader.GetOrdinal(prop.Name));
                            prop.SetValue(item, value);
                        }
                        else if (prop.PropertyType == typeof(float))
                        {
                            float value = reader.GetFloat(reader.GetOrdinal(prop.Name));
                            prop.SetValue(item, value);
                        }
                        else if (prop.PropertyType == typeof(double))
                        {
                            double value = reader.GetDouble(reader.GetOrdinal(prop.Name));
                            prop.SetValue(item, value);
                        }
                        else if (prop.PropertyType == typeof(decimal))
                        {
                            decimal value = reader.GetDecimal(reader.GetOrdinal(prop.Name));
                            prop.SetValue(item, value);
                        }
                        else if (prop.PropertyType == typeof(string))
                        {
                            string value = reader.GetString(reader.GetOrdinal(prop.Name));
                            prop.SetValue(item, value);
                        }
                        else if (prop.PropertyType == typeof(List<>))
                        {
                            int length = (prop.GetValue(item) as List<object>).Count;
                            object[] listItems = new object[length];

                        }
                        else
                        {
                            string data = reader.GetString(reader.GetOrdinal(prop.Name));
                            object newItem = System.Text.Json.JsonSerializer.Deserialize<string>(data);
                            prop.SetValue(item, newItem);
                        }
                    }

                    items.Add(item);
                }
            }

            return items;
        }

        public static bool CreateDatabase(string name)
        {
            if (!File.Exists(name))
            {
                SQLiteConnection.CreateFile(name);
                return true;
            }

            return false;
        }
    }*/
}