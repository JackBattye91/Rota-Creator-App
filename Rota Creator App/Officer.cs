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
        public ObservableCollection<Position> WorkablePositions { get; set; } = new ObservableCollection<Position>();

        public Officer()
        {
            Random rnd = new Random();
            ID = rnd.Next();
        }

        public bool CanWorkPosition(Position pos)
        {
            return WorkablePositions.Count(p => p.ID == pos.ID) > 0;
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
            if (reader.FieldCount < 5)
                return;

            try
            {
                ID = reader.GetInt32(0);
                Name = reader.GetString(1);
                Abbreviation = reader.GetString(2);
                Team = reader.GetString(3);

                if (reader.FieldCount >= 5)
                {
                    string workableString = reader.GetString(4);

                    if (workableString == null)
                        return;

                    string[] workables = reader.GetString(4).Split(',');
                    foreach (string pos in workables)
                    {
                        int id = 0;
                        if (int.TryParse(pos, out id))
                        {
                            List<Position> positions = SQLiteDatabase.Global?.Query<Position>("Position", "*", $"ID = {id}");
                            if (positions != null && positions.Count > 0)
                                WorkablePositions.Add(positions[0]);
                        }
                    }
                }
            }
            catch(Exception e)
            {

            }
        }

        public string SQLiteInsertScript()
        {
            return $"INSERT INTO Officer(ID, Name, Abbreviation, Team) VALUES ({ID}, '{Name}', '{Abbreviation}', '{Team}')";
        }

        public string SQLiteUpdateScript()
        {
            string script = $"UPDATE Officer SET Name='{Name}', Abbreviation='{Abbreviation}', Team='{Team}', WorkablePositions='";

            foreach(Position pos in WorkablePositions)
            {
                script += $"{pos.ID},";
            }

            script += $"' WHERE ID={ID}";

            return script;
        }

        public string SQLiteDeleteScript()
        {
            return $"DELETE FROM Officer WHERE ID={ID}";
        }
    }
}
