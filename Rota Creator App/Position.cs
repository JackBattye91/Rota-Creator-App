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
        public string Name { get; set; }
        public Site Site { get; set; }
        public int Duration { get; set; }

        public Position()
        {
            Random rnd = new Random();
            ID = rnd.Next();
        }
        public bool IsActive(DateTime time)
        {
            return true;
        }

        
        public static ObservableCollection<Position> Load()
        {
            List<Position> positionsList = SQLiteDatabase.Global?.Query<Position>("Position");
            ObservableCollection<Position> positions = new ObservableCollection<Position>();

            foreach (Position pos in positionsList)
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
            if (reader.FieldCount != 4)
                return;

            ID = reader.GetInt32(0);
            Name = reader.GetString(1);
            Duration = reader.GetInt32(3);

            List<Site> sites = SQLiteDatabase.Global.Query<Site>("Site", "*", $"ID = {reader.GetInt32(2)}");
            if (sites != null && sites.Count > 0)
                Site = sites[0];
        }

        public string SQLiteInsertScript()
        {
            return $"INSERT INTO Position(ID, Name, Site, Duration) VALUES ({ID}, '{Name}', {Site.ID}, {Duration})";
        }

        public string SQLiteUpdateScript()
        {
            return $"UPDATE Position SET Name='{Name}', Site={Site.ID}, Duration={Duration} WHERE ID={ID}";
        }

        public string SQLiteDeleteScript()
        {
            return $"DELETE FROM Position WHERE ID={ID}";
        }
    }
}
