using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Collections.ObjectModel;

namespace Rota_Creator_App
{
    public class Site : ISQLiteable
    {
        public int ID { get; protected set; } = -1;
        public string Name { get; set; } = "";

        public Site()
        {
            Random rnd = new Random();
            ID = rnd.Next();
        }
        public Site(string name)
        {
            Random rnd = new Random();
            ID = rnd.Next();
            Name = name;
        }
        public Site(int id, string name)
        {
            ID = id;
            Name = name;
        }

        public string SQLDataDefinition()
        {
            return "id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT";
        }
        public bool SQLInsert(SQLiteConnection connection)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"INSERT INTO sites (name) VALUES ('{Name}')";
            return command.ExecuteNonQuery() != 0;
        }
        public bool SQLUpdate(SQLiteConnection connection)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"UPDATE sites SET name = '{Name}' WHERE id = {ID}";
            return command.ExecuteNonQuery() != 0;
        }
        public bool SQLDelete(SQLiteConnection connection)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"DELETE FROM sites WHERE id = {ID}";
            return command.ExecuteNonQuery() != 0;
        }
        public void SQLParse(SQLiteDataReader reader)
        {
            ID = reader.GetInt32(0);
            Name = reader.GetString(1);
        }

        public static ObservableCollection<Site> Load()
        {
            ObservableCollection<Site> sites = new ObservableCollection<Site>();
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=rotacreator.db"))
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM sites";

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sites.Add(new Site() { ID = reader.GetInt32(0), Name = reader.GetString(1) });
                    }
                }
            }

            return sites;
        }

        /*
        public static void Save(ObservableCollection<Site> sites)
        {
            foreach (Site s in sites)
            {
                if (!SQLUpdate(s))
                    SQLInsert(s);
            }
        }*/
    }
}
