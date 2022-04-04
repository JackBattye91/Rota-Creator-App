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
        public int ID { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public Site Site { get; set; }
        public int Duration { get; set; }
        public List<int> ActiveTimes { get; set; }

        public Position()
        {
            Random rnd = new Random();
            ID = rnd.Next();
        }
        public Position(Position pos)
        {
            ID = pos.ID;
            Index = pos.Index;
            Name = pos.Name;
            Site = new Site(pos.Site);
            Duration = pos.Duration;
            ActiveTimes = new List<int>(pos.ActiveTimes);
        }
        public bool IsActive(DateTime time)
        {
            return true;
        }

        public static ObservableCollection<Position> Load()
        {
            List<Position> positionsList = SQLiteDatabase.Global?.Query<Position>("Position");

            ObservableCollection<Position> positions = new ObservableCollection<Position>();

            List<Position> sortedList = positionsList.OrderBy(p => p.Index).ToList();

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

            ID = reader.GetInt32(reader.GetOrdinal("ID"));
            Name = reader.GetString(reader.GetOrdinal("Name"));
            // Site
            Duration = reader.GetInt32(reader.GetOrdinal("Duration"));
            Index = reader.GetInt32(reader.GetOrdinal("Index"));

            List<Site> sites = SQLiteDatabase.Global.Query<Site>("Site", "*", $"ID = {reader.GetInt32(reader.GetOrdinal("Site"))}");
            if (sites != null && sites.Count > 0)
                Site = sites[0];
        }

        public string SQLiteInsertScript()
        {
            return $"INSERT INTO Position(ID, Name, Site, Duration, 'Index') VALUES ({ID}, '{Name}', {Site.ID}, {Duration}, {Index})";
        }

        public string SQLiteUpdateScript()
        {
            return $"UPDATE Position SET Name='{Name}', Site={Site.ID}, Duration={Duration}, 'Index'={Index} WHERE ID={ID}";        
        }

        public string SQLiteDeleteScript()
        {
            return $"DELETE FROM Position WHERE 'ID'={ID}";
        }

        public override bool Equals(object obj)
        {
            if (obj is Position == false)
                return false;

            Position pos = (Position)obj;
            return pos.ID == ID;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
