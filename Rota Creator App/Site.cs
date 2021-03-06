using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Collections.ObjectModel;

namespace Rota_Creator_App
{
    public class Site : ISQLiteable
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public Site()
        {
            Random rnd = new Random();
            ID = rnd.Next();
        }
        public Site(Site site)
        {
            ID = site.ID;
            Name = site.Name;
        }
        public override string ToString()
        {
            return $"ID : {ID}, Name : {Name}";
        }

        public static ObservableCollection<Site> Load()
        {
            List<Site> siteList = SQLiteDatabase.Global?.Query<Site>("Site");
            ObservableCollection<Site> sites = new ObservableCollection<Site>();

            foreach (Site site in siteList)
                sites.Add(site);

            return sites;
        }

        public static void Save(ObservableCollection<Site> sites)
        {
            if (SQLiteDatabase.Global != null)
            {
                foreach(Site site in sites)
                {
                    if (!SQLiteDatabase.Global.Insert<Site>(site))
                        SQLiteDatabase.Global.Update<Site>(site);
                }
            }
        }

        public void SQLiteParse(SQLiteDataReader reader)
        {
            if (reader.FieldCount != 2)
                return;

            ID = reader.GetInt32(0);
            Name = reader.GetString(1);
        }

        public string SQLiteInsertScript()
        {
            return $"INSERT INTO Site(ID, Name) VALUES ({ID}, '{Name}')";
        }

        public string SQLiteUpdateScript()
        {
           return $"UPDATE Site SET Name='{Name}' WHERE ID={ID}";
        }

        public string SQLiteDeleteScript()
        {
            return $"DELETE FROM Site WHERE ID={ID}";
        }

        public override bool Equals(object obj)
        {
            if (obj is Site == false)
                return false;

            Site site = (Site)obj;
            return site.ID == ID;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
