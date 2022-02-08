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
            foreach (Site s in sites)
            {
                if (!Update(s))
                    Insert(s);
            }
        }

        public static bool Update(Site site)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=rotacreator.db"))
            {
                connection.Open();

                SQLiteCommand update = connection.CreateCommand();
                update.CommandText = $"UPDATE sites SET name = '{site.Name}' WHERE id = {site.ID}";

                return (update.ExecuteNonQuery() != 0);
            }
        }

        public static bool Insert(Site site)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=rotacreator.db"))
            {
                connection.Open();

                SQLiteCommand insert = connection.CreateCommand();
                insert.CommandText = $"INSERT INTO sites (id, name) VALUES({s.ID}, '{s.Name}')";

                return (insert.ExecuteNonQuery() != 0);
            }
        }
    }
}
