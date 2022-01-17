using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rota_Creator_App
{
    public class Officer
    {
        public int ID { get; protected set; }
        public string Name { get; protected set; }
        public List<Position> WorkablePositions { get; protected set; }

        public Officer()
        {
            ID = new Random().Next();
            WorkablePositions = new List<Position>();
        }

        public bool CanWorkPosition(Position pos)
        {
            foreach(Position workPos in WorkablePositions)
            {
                if (workPos == pos)
                    return true;
            }

            return false;
        }

        public bool Equals(Officer officer)
        {
            if (ID == officer.ID)
                return true;

            return false;
        }
    }
}