using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rota_Creator_App
{
    public class Position
    {
        public string Name { get; set; }
        public string Site { get; set; }
        public int Duration { get; set; }


        public bool IsActive(DateTime time)
        {
            return true;
        }

        public static List<Position> Load()
        {
            List<Position> positions = new List<Position>();

            

            return positions;
        }
    }
}
