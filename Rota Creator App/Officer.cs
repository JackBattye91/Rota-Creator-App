using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rota_Creator_App
{
    class Officer
    {
        public string Name { get; set; }
        public string AbbriAbbreviation { get; set; }
        public string Team { get; set; }
        List<Position> WorkablePositions { get; set; }
    }
}
