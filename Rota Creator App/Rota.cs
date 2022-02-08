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

        public DateTime StartTime { get; protected set; }
        public DateTime FinishTime { get; protected set; }
        public ObservableCollection<Position> Positions { get; protected set; }
        public ObservableCollection<Officer> Officers { get; protected set; }
        public ObservableCollection<RotaTimePosition> rotaTimePositions { get; protected set; }
        static Random rnd = new Random();

        // hide constructor
        public Rota()
        {
        }

        public Officer GetOfficer(Position position, DateTime time)
        {
            foreach (RotaTimePosition timePos in rotaTimePositions)
            {
                if (timePos.position.Equals(position) && timePos.time == time)
                    return timePos.officer;
            }
            return null;
        }
        public Position GetPosition(Officer officer, DateTime time)
        {
            foreach (RotaTimePosition timePos in rotaTimePositions)
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

            return GetOfficer(position, time) != null;
        }
        public bool IsCovered(Position Position)
        {
            for (DateTime time = StartTime; time < FinishTime; time.AddHours(1))
            {
                if (!IsCovered(Position, time))
                    return false;
            }

            return true;
        }
        public bool IsCovered(DateTime time)
        {
            foreach (Position pos in Positions)
            {
                if (!IsCovered(pos, time))
                    return false;
            }

            return true;
        }
        public void Clear(DateTime time, bool ignoreDurations = true)
        {
            if (ignoreDurations)
            {
                rotaTimePositions.RemoveAll(tp => tp.time == time);
                return;
            }

            for (int tp = 0; tp < rotaTimePositions.Count(); tp++)
            {
                RotaTimePosition timePos = rotaTimePositions[tp];
                if (timePos.time != time)
                    continue;

                if (timePos.position.Duration == 1)
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
        public void Clear(Position position)
        {
            rotaTimePositions.RemoveAll(tp => tp.position.Equals(position));
        }

        public void Generate(ObservableCollection<Officer> officers, ObservableCollection<Position> positions, DateTime startTime, DateTime finishTime)
        {
            // initialize properties
            StartTime = startTime;
            FinishTime = finishTime;
            Positions = new ObservableCollection<Position>(positions);
            Officers = new ObservableCollection<Officer>(officers);
            rotaTimePositions = new ObservableCollection<RotaTimePosition>();

            DateTime time = StartTime;

            for(DateTime time = StartTime; time < FinishTime; time.AddHours(1))
                rotaTimePositions.Add(coverTime(time));
        }

        private List<RotaTimePosition> coverTime(DateTime time)
        {
            List<RotaTimePosition> currTimePos = new List<RotaTimePosition>();

            foreach (Position pos in Positions)
            {
                try
                {
                    currTimePos.AddRange(coverPosition(time, pos));
                }
                catch(Exception e)
                {
                    
                }
            }

            return currTimePos;
        }
        private List<RotaTimePosition> coverPosition(DateTime time, Position pos)
        {
            int attempts = 0;

            while(attempts < 4)
            {
                // get all officers that can work position
                ObservableCollection<Officer> offList = new ObservableCollection<Officer>(Officers.Where(o => o.CanWorkPosition(pos)));

                if (offList.Count == 0)
                    throw new ArgumentException($"No officer can work this position: {pos.Name}");
                else if (offList.Count == 1)
                    return coverPosition(time, pos, offList[0]);

                while (offList.Count() > 0)
                {
                    // get random officer
                    Officer off = offList[rnd.Next(offList.Count)];

                    // if officer is already working a position
                    if (GetPosition(off, time) != null)
                    {
                        offList.Remove(off);
                        continue;
                    }

                    // if has previous time
                    if (time > StartTime)
                    {
                        DateTime prevTime = time - new TimeSpan(1, 0, 0);

                        // did officer work previous time at position
                        if (IsCovered(pos, prevTime) && GetOfficer(pos, prevTime).Equals(off))
                        {
                            // remove officer
                            offList.Remove(off);
                            continue;
                        }

                        if (isHereOfficerCrossover(time, pos, off))
                        {
                            offList.Remove(off);
                            continue;
                        }
                    }

                    return coverPosition(time, pos, off);
                }
                
                attempts++;
            }

            throw new Exception("Officers list exhausted");
        }

        private List<RotaTimePosition> coverPosition(DateTime time, Position pos, Officer off)
        {
            List<RotaTimePosition> currTimePos = new List<RotaTimePosition>();

            // cover for duration of position
            for (int d = 0; d < pos.Duration; d++)
            {
                DateTime coverTime = time + new TimeSpan(d, 0, 0);

                if (coverTime > FinishTime)
                    break;

                if (!pos.IsActive(coverTime))
                    currTimePos.Add(new RotaTimePosition() { time = coverTime, position = pos, isActive = false });
                else
                    currTimePos.Add(new RotaTimePosition() { time = coverTime, position = pos, officer = off });
            }

            return currTimePos;
        }

        private bool isHereOfficerCrossover(DateTime time, Position pos, Officer off)
        {
            DateTime prevTime = time - new TimeSpan(1, 0, 0);

            // if there a straight swap off officers move to next officer
            Position lastPos = GetPosition(off, prevTime); // the position of curr officer last hour
            Officer lastOff = GetOfficer(pos, prevTime); // officer in this position last hour

            if (lastPos != null && lastOff != null)
            {
                if (GetOfficer(lastPos, time).Equals(lastOff) && GetPosition(lastOff, time).Equals(lastPos))
                    return true;
            }

            return false;
        }
    }
}
