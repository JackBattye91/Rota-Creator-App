using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Collections.ObjectModel;

namespace Rota_Creator_App
{
    public class Site
    {
        [PrimaryKey]
        public int ID { get; protected set; }
        public string Name { get; set; }

        public Site()
        {
            Random rnd = new Random();
            ID = rnd.Next();
        }

        public static ObservableCollection<Site> Load()
        {
            ObservableCollection<Site> sites = new ObservableCollection<Site>();
            if (SQLiteDatabase.Global != null)
            {
                List<Site> siteList = SQLiteDatabase.Global.Query<Site>();
                foreach(Site site in siteList)
                {
                    sites.Add(site);
                }
            }

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
    }
}
