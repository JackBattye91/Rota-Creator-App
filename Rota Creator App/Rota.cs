using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace Rota_Creator_App
{
    public class Rota
    {
        // ********************************* EXCEPTIONS ******************************
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
        protected class RotaOfficer : Officer
        {
            public Dictionary<DateTime, Position> RotaPositions; 
        }
        protected class RotaPosition : Position
        {
            public Dictionary<DateTime, Officer> RotaOfficers;
        }

        public DateTime StartTime { get; protected set; }
        public DateTime FinishTime { get; protected set; }
        public List<Position> Positions { get; protected set; }
        public List<Officer> Officers { get; protected set; }
        public List<RotaTimePosition> RotaTimePositions { get; protected set; }

        // hide constructor
        protected Rota()
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
                if (timePos.officer != null && timePos.officer.Equals(officer) && timePos.time == time)
                    return timePos.position;
            }
            return null;
        }

        protected List<Officer> GetOfficers(Position position)
        {
            List<Officer> officers = new List<Officer>();

            foreach(RotaTimePosition timePos in RotaTimePositions)
            {
                if (timePos.position.Equals(position))
                    officers.Add(timePos.officer);
            }

            return officers;
        }

        protected bool IsWorking(Officer off, DateTime time)
        {
            foreach(RotaTimePosition timePos in RotaTimePositions)
            {
                if (timePos.officer != null && timePos.officer.Equals(off) && timePos.time == time)
                    return true;
            }
            return false;
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

        public void Update(Position position, DateTime time, Officer newOfficer, bool propagateChanges = false)
        {
            // get officer that was working the position
            Officer oldOfficer = GetOfficer(Position, time);
            // get positions the new officer was working
            Position oldPosition = GetPosition(newOfficer, time);

            // place officer in new position
            foreach(RotaTimePosition timePos in RotaTimePositions)
            {
                if (timePos.Position == position && timePos.time == time)
                {
                    timePos.Officer = newOfficer;
                    break;
                }
            }

            if (oldOfficer != null && oldPosition != null)
            {
                // place old officer in old position
                foreach(RotaTimePosition timePos in RotaTimePositions)
                {
                    if (timePos.Position == oldPosition && timePos.time == time)
                    {
                        timePos.Officer = oldOfficer;
                        break;
                    }
                }
            }

            if (propagateChanges)
            {
                // clear future times
                for(DateTime clearTime = time + new TimeSpan(1, 0, 0); clearTime < FinishTime; clearTime += new TimeSpan(1, 0, 0))
                    Clear(time);
                
                for(int DateTime coverTime = time + new TimeSpan(1, 0, 0); coverTime < FinishTime; coverTime += new TimeSpan(1, 0, 0))
                    coverTime(this, coverTime);
            }
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

            // run hrough all times
            for(DateTime time = startTime; time < finishTime; time += new TimeSpan(1, 0, 0))
            {
                coverTime(rota, time);
            }

            return rota;
        }

        protected static void coverTime(Rota rota, DateTime time)
        {
            // run through all te positions
            foreach(Position pos in positions)
            {
                try
                {
                    // try cover the position
                    rota.RotaTimePositions.AddRange(coverPosition(rota, pos, time));
                }
                catch(PositionsNotActiveException notActive) // if the positions is not active at this time
                {
                    rota.RotaTimePositions.Add(new RotaTimePosition() { time = time, position = pos, officer = null, isActive = false });
                }
                catch(PositionsNotCoveredException notCovered) // if The position was not covered
                {
                    rota.RotaTimePositions.Add(new RotaTimePosition() { time = time, position = pos, isActive = true, officer = null });
                }
                catch(OfficerNotFoundException notFound) // if not officer can word the position
                {
                    rota.RotaTimePositions.Add(new RotaTimePosition() { time = time, position = pos, isActive = true, officer = null });
                }
                catch(Exception e)
                {
                }
            }
        }

        protected static List<RotaTimePosition> coverPosition(Rota rota, Position pos, DateTime time)
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

            // if no officer can work the positions then throw exception
            if(offList.Count() == 0)
                throw new OfficerNotFoundException($"There are no officers that can work: {pos.Name}");

            // while the list 
            while(offList.Count() > 0)
            {
                // get random officer
                Officer off = offList[rnd.Next(offList.Count)];

                try
                {
                    // try cover position
                    timePos.AddRange(positionOfficer(rota, off, pos, time));
                    break;
                }
                catch(OfficerAlreadyWorkingException working) // if officer is already working at this time
                {
                    offList.Remove(off);
                }
                catch(OfficerCrossoverException crossover) // if there would be a crossover 
                {
                    offList.Remove(off);
                }
                catch(OfficerPreviousWorkingException prevWork) // if the officer has already worked at this position
                {
                    offList.Remove(off);
                }
                catch(Exception e)
                {
                }
            }

            if (offList.Count() == 0)
                throw new OfficerNotFoundException($"There are no officers that can work: {pos.Name}");

            return timePos;
        }

        protected static List<RotaTimePosition> positionOfficer(Rota rota, Officer off, Position pos, DateTime time)
        {
            // if officer is already working a position
            if (rota.IsWorking(off, time))
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
                if (isCrossover(rota, off, pos, time))
                {
                    throw new OfficerCrossoverException("There is a crossover between {off.Name} and {lastOff.Name}");
                }

                // if officer has worked the position previously
                if (rota.GetOfficers(pos).Contains(off))
                {
                    throw new OfficerPreviousWorkingException($"Officer: {off.Name} has worked this position previously");
                }
            }

            List<RotaTimePosition> retValue = new List<RotaTimePosition>();

            // cover for duration of position
            for (int d = 0; d < pos.Duration; d++)
            {
                DateTime coverTime = time + new TimeSpan(d, 0, 0);

                if (coverTime > rota.FinishTime)
                    break;

                if (!pos.IsActive(coverTime))
                    retValue.Add(new RotaTimePosition() { time = coverTime, position = pos, isActive = false });
                else
                    retValue.Add(new RotaTimePosition() { time = coverTime, position = pos, officer = off });
            }

            return retValue;
        }

        protected static bool isCrossover(Rota rota, Officer off, Position pos, DateTime time)
        {
            // if there a straight swap off officers move to next officer
            Position lastPos = rota.GetPosition(off, prevTime); // the position of curr officer last hour
            Officer lastOff = rota.GetOfficer(pos, prevTime); // officer in this position last hour
            if (lastPos != null && lastOff != null)
            {
                Officer nOff = rota.GetOfficer(lastPos, time);
                Position nPos = rota.GetPosition(lastOff, time);
                if (nOff != null && nOff.Equals(lastOff) && nPos != null && nPos.Equals(lastPos))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
