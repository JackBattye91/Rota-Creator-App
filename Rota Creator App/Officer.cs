using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Rota_Creator_App
{
    public class Officer
    {
        public int ID { get; protected set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Team { get; set; }
        public List<Position> WorkablePositions { get; set; } = new List<Position>();

        public Officer()
        {
            Random rnd = new Random();
            ID = rnd.Next();
        }

        public bool CanWorkPosition(Position pos)
        {
            return WorkablePositions.Contains(pos);
        }

        public static ObservableCollection<Officer> Load()
        {
            ObservableCollection<Officer> officers = new ObservableCollection<Officer>();

            using (SQLiteConnection connection = new SQLiteConnection("Data Source=rotacreator.db"))
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM officers";

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        officers.Add(new Officer()
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Abbreviation = reader.GetString(2),
                            Team = reader.GetString(3)
                        });
                    }
                }
            }

            return officers;
        }
    }
}
