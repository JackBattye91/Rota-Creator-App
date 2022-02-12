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
        [PrimaryKey]
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
            ObservableCollection<Position> positions = new ObservableCollection<Position>();

            if (SQLiteDatabase.Global != null)
            {
                List<Position> positionList = SQLiteDatabase.Global.Query<Position>();

                if (positionList == null)
                    return positions;

                foreach(Position pos in positionList)
                {
                    positions.Add(pos);
                }
            }

            return positions;
        }

        public static void Save(ObservableCollection<Position> positions)
        {
            if (SQLiteDatabase.Global != null)
            {
                foreach(Position pos in positions)
                {
                    if (!SQLiteDatabase.Global.Insert<Position>(pos))
                        SQLiteDatabase.Global.Update<Position>(pos);
                }
            }
        }
    }
}
