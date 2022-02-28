using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rota_Creator_App
{
    public class Officer : ISQLiteable, Rota.IEmployee
    {
        public int id { get; set; }
        public string name { get; set; }
        public string abbreviation { get; set; }
        public string team { get; set; }
        public ObservableCollection<Rota.IPosition> WorkablePositions { get; set; } = new ObservableCollection<Rota.IPosition>();

        public int ID() { return id; }
        public string Name() { return name; }
        public bool CanWork(Rota.IPosition position)
        {
            return CanWorkPosition(position);
        }

        public Officer()
        {
            Random rnd = new Random();
            id = rnd.Next();
        }

        public bool CanWorkPosition(Rota.IPosition pos)
        {
            return WorkablePositions.Count(p => p.ID() == pos.ID()) > 0;
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
                id = reader.GetInt32(0);
                name = reader.GetString(1);
                abbreviation = reader.GetString(2);
                team = reader.GetString(3);

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
            return $"INSERT INTO Officer(ID, Name, Abbreviation, Team) VALUES ({id}, '{name}', '{abbreviation}', '{team}')";
        }

        public string SQLiteUpdateScript()
        {
            string script = $"UPDATE Officer SET Name='{name}', Abbreviation='{abbreviation}', Team='{team}', WorkablePositions='";

            foreach(Position pos in WorkablePositions)
            {
                script += $"{pos.ID()},";
            }

            script += $"' WHERE ID={id}";

            return script;
        }

        public string SQLiteDeleteScript()
        {
            return $"DELETE FROM Officer WHERE ID={id}";
        }
    }
}
