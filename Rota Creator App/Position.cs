using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Collections.ObjectModel;

namespace Rota_Creator_App
{
    public class Position : ISQLiteable, Rota.IPosition
    {
        public int id { get; set; }
        public int index { get; set; }
        public string name { get; set; }
        public Site site { get; set; }
        public int duration { get; set; }

        public int ID() { return id; }
        public int Index() { return index; }
        public string Name() { return name; }
        public int Duration() { return duration; }


        public Position()
        {
            Random rnd = new Random();
            id = rnd.Next();
        }
        public bool IsActive(DateTime time)
        {
            return true;
        }

        public static ObservableCollection<Position> Load()
        {
            List<Position> positionsList = SQLiteDatabase.Global?.Query<Position>("Position");

            ObservableCollection<Position> positions = new ObservableCollection<Position>();

            List<Position> sortedList = positionsList.OrderBy(p => p.Index()).ToList();

            foreach (Position pos in sortedList)
                positions.Add(pos);

            return positions;
        }

        public static void Save(ObservableCollection<Position> positions)
        {
            foreach (Position pos in positions)
            {
                if (!SQLiteDatabase.Global.Insert<Position>(pos))
                    SQLiteDatabase.Global.Update<Position>(pos);
            }
        }
        
        public void SQLiteParse(SQLiteDataReader reader)
        {
            if (reader.FieldCount != 5)
                return;

            id = reader.GetInt32(reader.GetOrdinal("ID"));
            name = reader.GetString(reader.GetOrdinal("Name"));
            // Site
            duration = reader.GetInt32(reader.GetOrdinal("Duration"));
            index = reader.GetInt32(reader.GetOrdinal("Index"));

            List<Site> sites = SQLiteDatabase.Global.Query<Site>("Site", "*", $"ID = {reader.GetInt32(reader.GetOrdinal("Site"))}");
            if (sites != null && sites.Count > 0)
                site = sites[0];
            
        }

        public string SQLiteInsertScript()
        {
            return $"INSERT INTO Position('ID', 'Name', 'Site', 'Duration', 'Index') VALUES ({id}, '{name}', {site.ID}, {duration}, {index})";
        }

        public string SQLiteUpdateScript()
        {
            return $"UPDATE Position SET 'Name'='{name}', 'Site'={site.ID}, 'Duration'={duration}, 'Index'={index} WHERE 'ID'={id}";
        }

        public string SQLiteDeleteScript()
        {
            return $"DELETE FROM Position WHERE 'ID'={id}";
        }
    }
}
