using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

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

        private List<Officer> GetOfficers(Position position)
        {
            List<Officer> officers = new List<Officer>();

            foreach(RotaTimePosition timePos in rotaTimePositions)
            {
                if (timePos.position.Equals(position))
                    officers.Add(timePos.officer);
            }

            return officers;
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
        public void Clear(DateTime time, bool ignoreDurations = true)
        {
            if (ignoreDurations)
                rotaTimePositions.RemoveAll(tp => tp.time == time);
            else
            {
                for(int tp = 0; tp < rotaTimePositions.Count(); tp++)
                {
                    RotaTimePosition timePos = rotaTimePositions[tp];
                    if (timePos.time == time)
                    {
                        if (timePos.duration == 1)
                            rotaTimePositions.Remove(timePos);
                        else
                        {
                            if (GetOfficer(timePos.position, time - new TimeSpan(1, 0, 0)).Equals(timePos.officer))
                                continue;
                            else
                                rotaTimePositions.Remove(timePos);
                        }
                    }
                }
            }
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
            Random rnd = new Random();

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

                    // get all officers that can work position
                    List<Officer> offList = officers.Where(o => o.CanWorkPosition(pos)).ToList();

                    while(offList.Count() > 0 && !rota.IsCovered(pos, time))
                    {
                        // get random officer
                        Officer off = offList[rnd.Next(offList.Count)];

                        // try cover position
                        if (!positionOfficer(rota, off, pos, time))
                            offList.Remove(off);
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

        private static bool positionOfficer(Rota rota, Officer off, Position pos, DateTime time)
        {
            // if officer is already working a position
            if (rota.GetPosition(off, time) != null)
            {
                return false;
            }

            // if has previous time
            if (time > rota.StartTime)
            {
                DateTime prevTime = time - new TimeSpan(1, 0, 0);

                // did officer work previous time at position
                if (rota.IsCovered(pos, prevTime) != null && rota.GetOfficer(pos, prevTime).Equals(off))
                {
                    // remove officer
                    return false;
                }

                // if there a straight swap off officers move to next officer
                Position lastPos = rota.GetPosition(off, prevTime); // the position of curr officer last hour
                Officer lastOff = rota.GetOfficer(pos, prevTime); // officer in this position last hour

                if (lastPos != null && lastOff != null)
                {
                    if (rota.GetOfficer(lastPos, time).Equals(lastOff) && rota.GetPosition(lastOff, time).Equals(lastPos))
                    {
                        return false;
                    }
                }

                // if officer has worked positions before then try another officer
                if (rota.GetOfficers(pos).Contains(off))
                {
                    offList.Remove(off);
                    continue;
                }
            }

            // cover for duration of position
            for (int d = 0; d < pos.Duration; d++)
            {
                DateTime coverTime = time + new TimeSpan(d, 0, 0);

                if (coverTime > rota.FinishTime)
                    break;

                if (!pos.IsActive(coverTime))
                    currTimePos.Add(new RotaTimePosition() { time = coverTime, position = pos, isActive = false });
                else
                    currTimePos.Add(new RotaTimePosition() { time = coverTime, position = pos, officer = off });
            }
        }
    }
}
