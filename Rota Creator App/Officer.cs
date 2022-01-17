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
        public string Name 
        { 
            get { return Name; }
            set
            {
                if (value != "")
                    Name = value;
            }
        }
        public string Team { get; set; }
        public List<Position> WorkablePositions { get; protected set; }

        privatee Officer()
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

        static List<Officer> Load()
        {
            List<Officer> officers = new List<Officer>();

            FirestoreDb db = FirestoreDb.Create("rotacreator-d84f6");
            CollctionReference = collection = db.Collection("officers");
            
            QuerySnapshot allOfficers = await collection.GetSnapshotAsync();
            foreach(DocumentSnapshot document in allOfficers.Documents)
            {
                Officer officer = document.ConvertTo<Officer>();
                officers.Add(officer);
            }

            return officers;
        }
        static Officer Create(string name, string team, List<Position> workablePositions)
        {
            Officer officer = new Officer();
            officer.Name = name;
            officer.Team = team;
            officer.WorkablePositions = new List<Position>(workablePositions);

            return officer;
        }
    }
}