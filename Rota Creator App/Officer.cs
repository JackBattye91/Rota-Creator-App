using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rota_Creator_App
{
    public class Officer
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Team { get; set; }
        public List<Position> WorkablePositions { get; set; }

        public bool CanWorkPosition(Position pos)
        {
            return WorkablePositions.Contains(pos);
        }
    }
}
