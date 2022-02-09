using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Collections.ObjectModel;

namespace Rota_Creator_App
{
    public class Position : ISQLiteable
    {
        public int ID { get; protected set; };
        public string Name { get; set; };
        public Site Site { get; set; };
        public int Duration { get; set; };

        public Position()
        {
        }
        public Position(string name, Site site, int duration)
        {
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

        public static override string SQLDataDefinition()
        {
            return "id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, site INTEGER, duration INTEGER";
        }
        public override bool SQLInsert(SQLiteConnection connection)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"INSERT INTO positions (name, site, duration) VALUES ('{Name}', '{Site.ID}', {Duration})";
            return command.ExecuteNonQuery() != 0;
        }
        public override bool SQLUpdate(SQLiteConnection connection)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"UPDATE positions SET name = '{Name}', site = '{Site}', duration = {Duration} WHERE id = {ID}";
            return command.ExecuteNonQuery() != 0;
        }
        public override bool SQLDelete(SQLiteConnection connection)
        {
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"DELETE FROM positions WHERE id = {ID}";
            return command.ExecuteNonQuery() != 0;
        }
        public override void SQLParse(SQLiteDataReader reader)
        {
            ID = reader.GetInt32(0);
            Name = reader.GetString(1);
            Site = reader.GetString(2);
            Duration = reader.GetInt32(3);
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
