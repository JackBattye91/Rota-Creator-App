using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rota_Creator_App
{
    public class Rota
    {
        public class RotaTimePosition
        {
            public Position position;
            public DateTime time;
            public Officer officer;
            public bool isActive = true;
        }

        DateTime StartTime { get; protected set; }
        DateTime FinishTime { get; protected set; }
        List<Position> Positions { get; protected set; }
        List<Officer> Officers { get; protected set; }
        List<RotaTimePosition> rotaTimePositions { get; protected set; }

        // hide constructor
        private Rota()
        {
        }

        public Officer GetOfficer(Position position, DateTime time)
        {
            foreach(RotaTimePosition timePos in rotaTimePositions)
            {
                if (timePos.position.Equals(position) && timePos.time == time)
                    return timePos.officer;
            }
            return null;
        }
        public Position GetPosition(Officer officer, DateTime time)
        {
            foreach(RotaTimePosition timePos in rotaTimePositions)
            {
                if (timePos.officer.Equals(officer) && timePos.time == time)
                    return timePos.position;
            }

            return null;
        }

        public bool IsCovered(Position position, DateTime time)
        {
            if (!position.IsActive(time))
                return true;
            
            return GetOfficer(Position, time) != null;
        }
        public bool IsCovered(Position Position)
        {
            foreach(DateTime time = StartTime; time < FinishTime; time.AddHours(1))
            {
                if (!IsCovered(Position, time))
                    return false;
            }

            return true;
        }
        public bool IsCovered(DateTime time)
        {
            foreach(Position pos in Positions)
            {
                if (!IsCovered(pos, time))
                    return false;
            }

            return true;
        }
        public void Clear(DateTime time)
        {
            rotaTimePositions.RemoveAll(tp => tp.time == time);
        }
        public void Clear(Position position)
        {
            rotaTimePositions.RemoveAll(tp => tp.position.Equals(position));
        }

        static Rota Create(List<Officer> officers, List<Position> positions, DateTime startTime, DateTime finishTime)
        {
            // initialize properties
            Rota rota = new Rota();
            rota.rotaTimePositions = new List<RotaTimePosition>();
            rota.Officers = new List<Officer>(officers);
            rota.Positions = new List<Position>(positions);
            rota.Start = startTime;
            rota.FinishTime = finishTime;

            DateTime time = startTime;
            int attempts = 0;

            while (time < finishTime)
            {
                List<RotaTimePosition> currTimePos = new List<RotaTimePosition>();

                foreach(Position pos in positions)
                {
                    // is position covered
                    if (rota.IsCovered(pos, time))
                        continue;

                    // is not active then add as inactive and move to next position
                    if (!pos.IsActive(time))
                    {
                        currTimePos.Add(new RotaTimePosition() { time = time, position = pos, isActive = false });
                        continue;
                    }

                    // get all officers then can work position
                    List<Officer> offList = officers.Where(o => o.CanWorkPosition(pos)).ToList();
                    Random rnd = new Random();
                    while(offList.Count() > 0 && !positionCovered)
                    {
                        // get random officer
                        Officer off = offList[rnd.Next(offList.Count)];

                        // if officer is already working a position
                        if (rota.GetPosition(off, time) != null)
                        {
                            offList.Remove(off);
                            continue;   
                        }

                        // if has previous time
                        if (time - new TimeSpan(1, 0, 0) >= startTime)
                        {
                            // did officer work previous time at position
                            if (GetOfficer(pos, time - new TimeSpan(1, 0, 0)).Equals(off))
                            {
                                // remove officer
                                offList.Remove(off);
                                continue;
                            }
                        }

                        // cover for duration of position
                        for (int d = 0; d < pos.Duration; d++)
                        {
                            DateTime coverTime = time + new TimeSpan(d, 0, 0);

                            if (coverTime > finishTime)
                                break;

                            if (!pos.IsActive(coverTime))
                                currTimePos.Add(new RotaTimePosition() { time = coverTime, position = pos, isActive = false });
                            else
                                currTimePos.Add(new RotaTimePosition() { time = coverTime, position = pos, officer = off });
                        }
                    }

                    // if not covered then add to rota empty
                    if (!currTimePos.IsCovered(pos))
                        currTimePos.Add(new RotaTimePosition() { time = time, position = pos });
                }

                if (rota.IsCovered(time) || attempts >= 4)
                {
                    attempts = 0;
                    time.AddHours(1);
                    rota.rotaTimePositions.AddRange(currTimePos);
                }
                else
                    attempts++;
            }

            return rota;
        }
    }
}