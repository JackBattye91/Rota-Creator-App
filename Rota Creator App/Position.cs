using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Collections.ObjectModel;

namespace Rota_Creator_App
{
    public class Position
    {
        public int ID { get; protected set; } = -1;
        public string Name { get; set; } = "";
        public Site Site { get; set; } = new Site(-1, "");
        public int Duration { get; set; } = 0;

        public Position()
        {
            Random rnd = new Random();
            ID = rnd.Next();
        }
        public Position(string name, Site site, int duration)
        {
            Random rnd = new Random();
            ID = rnd.Next();

            Name = name;
            Site = site;
            Duration = duration;
        }
        public Position(int id, string name, Site site, int duration)
        {
            ID = id;
            Name = name;
            Site = site;
            Duration = duration;
        }

        public bool IsActive(DateTime time)
        {
            return true;
        }

        public static ObservableCollection<Position> Load()
        {
            ObservableCollection<Position> positions = new ObservableCollection<Position>();

            using (SQLiteConnection connection = new SQLiteConnection("Data Source=rotacreator.db"))
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM positions";

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        positions.Add(new Position()
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Site = new Site() { Name = reader.GetString(2) },
                            Duration = reader.GetInt32(3),
                        });
                    }
                }
            }

            return positions;
        }
    }
}
