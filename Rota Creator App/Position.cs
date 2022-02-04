using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rota_Creator_App
{
    class Position
    {
        public string Name { get; set; }
        public string Site { get; set; }
        public int Duration { get; set; }

        public Position()
        {
            Name = "";
            Site = "";
        }
    }
}
