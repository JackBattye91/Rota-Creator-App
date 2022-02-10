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

            if (Global == null)
                Global = this;
        }

        public bool RunCommand(string commandString)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = commandString;
            return (command.ExecuteNonQuery() != 0);
        }

        public bool CreateTable<T>() where T : new()
        {
            string dataDefinition = "";

            for(int p = 0; p < typeof(T).GetProperties().Count(); p++)
            {
                if (p != 0)
                    dataDefinition += ", ";

                PropertyInfo prop = typeof(T).GetProperties()[p];

                if( prop.GetType() == typeof(byte) || prop.GetType() == typeof(char) || 
                    prop.GetType() == typeof(short) || prop.GetType() == typeof(ushort) ||
                    prop.GetType() == typeof(int)|| prop.GetType() == typeof(uint) ||
                    rop.GetType() == typeof(long) || prop.GetType() == typeof(ulong))
                {
                    dataDefinition += $"{prop.Name} INTEGER";
                }
                else if (prop.GetType() == typeof(float) || prop.GetType() == typeof(double))
                {
                    dataDefinition += $"{prop.Name} REAL";
                }
                else if (prop.GetType() == typeof(string))
                {
                    dataDefinition += $"{prop.Name} TEXT";
                }
                else if (prop.GetType() == typeof(decimal) || prop.GetType() == typeof(DateTime))
                {
                    createTable.CommandText += $"{prop.Name} NUMERIC, ";
                }
                else
                    createTable.CommandText += $"{prop.Name} BLOB, ";

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
            return (command.ExecuteNonQuery() != 0);
        }
        public bool DeleteTable<T>(string tableName)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"DROP TABLE [IF EXISTS] {typeof(T).Name}";
            return (command.ExecuteNonQuery() != 0);
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

                if( prop.GetType() == typeof(byte) || prop.GetType() == typeof(char) || 
                    prop.GetType() == typeof(short) || prop.GetType() == typeof(ushort) ||
                    prop.GetType() == typeof(int)|| prop.GetType() == typeof(uint) ||
                    rop.GetType() == typeof(long) || prop.GetType() == typeof(ulong) ||
                    prop.GetType() == typeof(float) || prop.GetType() == typeof(double) ||
                    prop.GetType() == typeof(decimal) || prop.GetType() == typeof(DateTime))
                {
                    propertyValues += prop.GetValue(item).ToString();

                    if (prop.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).Count() != 0)
                    {
                        identifier += $"{prop.Name} = {prop.GetValue(item).ToString()}";
                    }
                }
                else if (prop.GetType() == typeof(string))
                {
                    dataDefinition += $"'{prop.GetValue(item) as string}'";

                    if (prop.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).Count() != 0)
                    {
                        identifier += $"{prop.Name} = '{prop.GetValue(item) as string}'";
                    }
                }
                else
                {
                    dataDefinition += $"";
                }
            }

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"INSERT INTO {typeof(T).Name} ({propertyNames}) VALUES ({propertyValues}) WHERE {identifier}";
            return (command.ExecuteNonQuery() != 0);
        }

        public List<T> Query<T>(string columns = "*", string condition = "") where T : new()
        {
            List<T> items = new List<T>();
            SQLiteCommand command = connection.CreateCommand();

            if (condition.Length == 0)
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

                        object value = reader[prop.Name];

                        if (value != null)
                            prop.SetValue(item, value);
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
    }
}