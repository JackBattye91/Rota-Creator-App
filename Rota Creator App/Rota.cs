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
        protected class OfficerException : Exception
        {
            public OfficerException()
            {
            }
            public OfficerException(string message) : base(message)
            {
            }
            public OfficerException(string message, Exception inner) : base(message, inner)
            {
            }
        }
        protected class OfficerCrossoverException : Exception
        {
            public OfficerCrossoverException()
            {
            }
            public OfficerCrossoverException(string message) : base(message)
            {
            }
            public OfficerCrossoverException(string message, Exception inner) : base(message, inner)
            {
            }
        }
        protected class OfficerPreviousWorkingException : Exception
        {
            public OfficerPreviousWorkingException()
            {
            }
            public OfficerPreviousWorkingException(string message) : base(message)
            {
            }
            public OfficerPreviousWorkingException(string message, Exception inner) : base(message, inner)
            {
            }
        }
        protected class OfficerAlreadyWorkingException : Exception
        {
            public OfficerAlreadyWorkingException()
            {
            }
            public OfficerAlreadyWorkingException(string message) : base(message)
            {
            }
            public OfficerAlreadyWorkingException(string message, Exception inner) : base(message, inner)
            {
            }
        }
        protected class OfficerNotFoundException : Exception
        {
            public OfficerNotFoundException()
            {
            }
            public OfficerNotFoundException(string message) : base(message)
            {
            }
            public OfficerNotFoundException(string message, Exception inner) : base(message, inner)
            {
            }
        }
        protected class PositionsAlreadyCoveredException : Exception
        {
            public PositionsAlreadyCoveredException()
            {
            }
            public PositionsAlreadyCoveredException(string message) : base(message)
            {
            }
            public PositionsAlreadyCoveredException(string message, Exception inner) : base(message, inner)
            {
            }
        }
        protected class PositionsNotActiveException : Exception
        {
            public PositionsNotActiveException()
            {
            }
            public PositionsNotActiveException(string message) : base(message)
            {
            }
            public PositionsNotActiveException(string message, Exception inner) : base(message, inner)
            {
            }
        }
        protected class PositionsNotCoveredException : Exception
        {
            public PositionsNotCoveredException()
            {
            }
            public PositionsNotCoveredException(string message) : base(message)
            {
            }
            public PositionsNotCoveredException(string message, Exception inner) : base(message, inner)
            {
            }
        }

        public class RotaTimePosition
        {
            public Position position;
            public DateTime time;
            public Officer officer;
            public bool isActive = true;
        }

        public DateTime StartTime { get; protected set; }
        public DateTime FinishTime { get; protected set; }
        public List<Position> Positions { get; protected set; }
        public List<Officer> Officers { get; protected set; }
        public List<RotaTimePosition> RotaTimePositions { get; protected set; }

        // hide constructor
        private Rota()
        {
        }

        public Officer GetOfficer(Position position, DateTime time)
        {
            foreach(RotaTimePosition timePos in RotaTimePositions)
            {
                if (timePos.position.Equals(position) && timePos.time == time)
                    return timePos.officer;
            }
            return null;
        }
        public Position GetPosition(Officer officer, DateTime time)
        {
            foreach(RotaTimePosition timePos in RotaTimePositions)
            {
                if (timePos.officer.Equals(officer) && timePos.time == time)
                    return timePos.position;
            }
            return null;
        }

        private List<Officer> GetOfficers(Position position)
        {
            List<Officer> officers = new List<Officer>();

            foreach(RotaTimePosition timePos in RotaTimePositions)
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
            
            return GetOfficer(position, time) != null;
        }
        public bool IsCovered(Position Position)
        {
            for(DateTime time = StartTime; time < FinishTime; time.AddHours(1))
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
                RotaTimePositions.RemoveAll(tp => tp.time == time);
            else
            {
                for(int tp = 0; tp < RotaTimePositions.Count(); tp++)
                {
                    RotaTimePosition timePos = RotaTimePositions[tp];
                    if (timePos.time == time)
                    {
                        if (timePos.position.Duration == 1)
                            RotaTimePositions.Remove(timePos);
                        else
                        {
                            if (GetOfficer(timePos.position, time - new TimeSpan(1, 0, 0)).Equals(timePos.officer))
                                continue;
                            else
                                RotaTimePositions.Remove(timePos);
                        }
                    }
                }
            }
        }
        public void Clear(Position position)
        {
            RotaTimePositions.RemoveAll(tp => tp.position.Equals(position));
        }

        public static Rota Create(List<Officer> officers, List<Position> positions, DateTime startTime, DateTime finishTime)
        {
            // initialize properties
            Rota rota = new Rota();
            rota.RotaTimePositions = new List<RotaTimePosition>();
            rota.Officers = new List<Officer>(officers);
            rota.Positions = new List<Position>(positions.OrderBy(p => p.Index));
            rota.StartTime = startTime;
            rota.FinishTime = finishTime;

            DateTime time = startTime;
            int attempts = 0;
            
            while (time < finishTime)
            {
                List<RotaTimePosition> currTimePos = new List<RotaTimePosition>();

                bool retry = false;
                foreach(Position pos in positions)
                {
                    try
                    {
                        currTimePos.AddRange(coverPosition(rota, pos, time));
                    }
                    catch(PositionsNotActiveException notActive)
                    {
                        currTimePos.Add(new RotaTimePosition() { time = time, position = pos, isActive = false });
                        //SystemLog.Add(notActive);
                    }
                    catch(PositionsNotCoveredException notCovered)
                    {
                        retry = true;
                        break;
                    }
                    catch(OfficerNotFoundException notFound)
                    {
                        //SystemLog.Add(notFound);

                        // add blank positions as not officer can work it
                        for (int d = 0; d < pos.Duration; d++)
                        {
                            DateTime coverTime = time + new TimeSpan(d, 0, 0);

                            if (coverTime > rota.FinishTime)
                                break;
                            
                            currTimePos.Add(new RotaTimePosition() { time = coverTime, position = pos, isActive = true, officer = null });
                        }
                    }
                    catch(Exception e)
                    {
                        //SystemLog.Add(e);
                    }
                }

                if (!retry || attempts >= 4)
                {
                    attempts = 0;
                    time.AddHours(1);
                    rota.RotaTimePositions.AddRange(currTimePos);
                }
                else
                    attempts++;
            }

            return rota;
        }

        private static List<RotaTimePosition> coverPosition(Rota rota, Position pos, DateTime time)
        {
            List<RotaTimePosition> timePos = new List<RotaTimePosition>();
            Random rnd = new Random();

            // is position covered
            if (rota.IsCovered(pos, time))
                throw new PositionsAlreadyCoveredException($"Position: {pos.Name} already covered at {time}");

            // is not active then add as inactive and move to next position
            if (!pos.IsActive(time))
                throw new PositionsNotActiveException($"Positions: {pos.Name} is not active at {time}");

            // get all officers that can work position
            List<Officer> offList = rota.Officers.Where(o => o.CanWorkPosition(pos)).ToList();

            if(offList.Count() == 0)
                throw new OfficerNotFoundException($"There are no officers that can work: {pos.Name}");

            while(offList.Count() > 0 && !rota.IsCovered(pos, time))
            {
                // get random officer
                Officer off = offList[rnd.Next(offList.Count)];

                try
                {
                    // try cover position
                    timePos.AddRange(positionOfficer(rota, off, pos, time));
                }
                catch(OfficerAlreadyWorkingException working)
                {
                    //SystemLog.Add(working);
                    offList.Remove(off);
                }
                catch(OfficerCrossoverException crossover)
                {
                    //SystemLog.Add(crossover);
                    offList.Remove(off);
                }
                catch(Exception e)
                {
                    //SystemLog.Add(e);
                }
            }

            /*// if not covered then add to rota empty
            if (!currTimePos.IsCovered(pos))
            {
                throw new PositionsNotCoveredException($"Positions: {pos.Name} could not be covered at {time}");
            }*/

            return timePos;
        }

        private static List<RotaTimePosition> positionOfficer(Rota rota, Officer off, Position pos, DateTime time)
        {
            // if officer is already working a position
            if (rota.GetPosition(off, time) != null)
            {
                throw new OfficerAlreadyWorkingException($"Officer: {off.Name} is already working at {pos.Name} at {time}");
            }

            // if has previous time
            if (time > rota.StartTime)
            {
                DateTime prevTime = time - new TimeSpan(1, 0, 0);

                // did officer work previous time at position
                if (rota.IsCovered(pos, prevTime) && rota.GetOfficer(pos, prevTime).Equals(off))
                {
                    throw new OfficerPreviousWorkingException($"Officer: {off.Name} worked at {pos.Name} in the last hour");
                }

                // if there a straight swap off officers move to next officer
                Position lastPos = rota.GetPosition(off, prevTime); // the position of curr officer last hour
                Officer lastOff = rota.GetOfficer(pos, prevTime); // officer in this position last hour

                if (lastPos != null && lastOff != null)
                {
                    if (rota.GetOfficer(lastPos, time).Equals(lastOff) && rota.GetPosition(lastOff, time).Equals(lastPos))
                    {
                        throw new OfficerCrossoverException("There is a crossover between {off.Name} and {lastOff.Name}");
                    }
                }

                // if officer has worked positions before then try another officer
                if (rota.GetOfficers(pos).Contains(off))
                {
                    throw new OfficerPreviousWorkingException($"Officer: {off.Name} has worked this position previously");
                }
            }

            // cover for duration of position
            for (int d = 0; d < pos.Duration; d++)
            {
                DateTime coverTime = time + new TimeSpan(d, 0, 0);

                if (coverTime > rota.FinishTime)
                    break;

                if (!pos.IsActive(coverTime))
                    return new List<RotaTimePosition>(new RotaTimePosition[] { new RotaTimePosition() { time = coverTime, position = pos, isActive = false } });
                else
                    return new List<RotaTimePosition>(new RotaTimePosition[] { new RotaTimePosition() { time = coverTime, position = pos, officer = off } });
            }

            return new List<RotaTimePosition>(new RotaTimePosition[] { new RotaTimePosition() });
        }
    }
}
