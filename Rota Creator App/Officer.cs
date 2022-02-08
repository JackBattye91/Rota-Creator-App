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
        public int ID { get; protected set; } = -1;
        public string Name { get; set; } = "";
        public string Abbreviation { get; set; } = "";
        public string Team { get; set; } = "";
        public ObservableCollection<Position> WorkablePositions { get; set; } = new ObservableCollection<Position>();

        public Officer()
        {
            Random rnd = new Random();
            ID = rnd.Next();
        }
        public Officer(string name, string abbreviation, string team)
        {
            Random rnd = new Random();
            ID = rnd.Next();
            
            Name = name;
            Abbreviation = abbreviation;
            Team = team;
        }
        public Officer(int id, string name, string abbreviation, string team)
        {
            ID = id;
            Name = name;
            Abbreviation = abbreviation;
            Team = team;
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
        public static bool Save(ObservableCollection<Officer> officers)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=rotacreator.db"))
            {
                connection.Open();

                SQLiteCommand update = connection.CreateCommand();
                foreach (Officer off in officers)
                {
                    update.CommandText = $"UPDATE officers SET name = '{off.Name}', abbreviation = '{off.Abbreviation}', team = '{off.Team}' WHERE id = {off.ID}";
                    if (update.ExecuteNonQuery() == 0)
                    {
                        SQLiteCommand create = connection.CreateCommand();
                        create.CommandText = $"INSERT INTO officers (id, name, abbreviation, team) VALUES({off.ID}, '{off.Name}', '{off.Abbreviation}', '{off.Team}')";
                        create.ExecuteNonQuery();
                    }
                }
            }

            return officers;
        }
    }
}
