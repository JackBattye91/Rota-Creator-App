using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rota_Creator_App
{
    public class Officer : ISQLiteable
    {
        //[PrimaryKey]
        public int ID { get; set; }
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

            if (SQLiteDatabase.Global != null)
            {
                List<Officer> offList = SQLiteDatabase.Global.Query<Officer>("Officer");

                if (offList == null)
                    return officers;

                foreach(Officer off in offList)
                    officers.Add(off);
            }   

            return officers;
        }

        public static void Save(ObservableCollection<Officer> officers)
        {
            if (SQLiteDatabase.Global != null)
            {
                foreach(Officer off in officers)
                {
                    if (!SQLiteDatabase.Global.Insert<Officer>(off))
                        SQLiteDatabase.Global.Update<Officer>(off);
                }
            }
        }

        public void SQLiteParse(SQLiteDataReader reader)
        {
            if (reader.FieldCount != 2)
                return;

            ID = reader.GetInt32(0);
            Name = reader.GetString(1);
            Abbreviation = reader.GetString(2);
            Team = reader.GetString(3);
        }

        public string SQLiteInsertScript()
        {
            return $"INSERT INTO Officer(ID, Name, Abbreviation, Team) VALUES ({ID}, '{Name}', '{Abbreviation}', '{Team}')";
        }

        public string SQLiteUpdateScript()
        {
            return $"UPDATE Officer SET Name='{Name}', Abbreviation='{Abbreviation}', Team='{Team}' WHERE ID={ID}";
        }

        public string SQLiteDeleteScript()
        {
            return $"DELETE FROM Officer WHERE ID={ID}";
        }
    }
}
