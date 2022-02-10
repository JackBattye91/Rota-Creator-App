using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Collections.ObjectModel;

namespace Rota_Creator_App
{
    public class Site
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

        public static bool SQLInsert(Site site)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=rotacreator.db"))
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = $"INSERT INTO sites (id, name) VALUES ({site.ID}, '{site.Name}')";
            }
        }
        public static bool SQLUpdate(Site site)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=rotacreator.db"))
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = $"UPDATE sites SET name = '{site.Name}' WHERE id = {site.ID}";
                return command.ExecuteNonQuery() != 0;
            }
        }
        public static bool SQLDelete(Site site)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=rotacreator.db"))
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = $"DELETE FROM sites WHERE id = {site.ID}";
                return command.ExecuteNonQuery() != 0;
            }
        }
        public static Site SQLLoad(int id)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=rotacreator.db"))
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM sites WHERE id = {id}";

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return new Site(reader.GetInt32(0), reader.GetString(1));
                    }
                }
            }
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

        public static void Save(ObservableCollection<Site> sites)
        {
            foreach(Site s in sites)
            {
                if (!SQLInsert(s))
                {
                    SQLUpdate(s);
                }
            }
        }
    }
}
