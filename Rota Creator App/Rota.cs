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

        public interface IEmployee
        {
            int ID();
            string Name();
            bool CanWork(IPosition position);
        }
        public interface IPosition
        {
            int ID();
            int Index();
            string Name();
            int Duration();

            bool IsActive(DateTime time);
        }

        public class RotaTimePosition
        {
            public IPosition position;
            public DateTime time;
            public IEmployee employee;
            public bool isActive = true;
        }

        public DateTime StartTime { get; protected set; }
        public DateTime FinishTime { get; protected set; }
        public List<IPosition> Positions { get; protected set; }
        public List<IEmployee> Employees { get; protected set; }
        public List<RotaTimePosition> RotaTimePositions { get; protected set; }

        // hide constructor
        protected Rota()
        {
        }

        public IEmployee GetEmployee(IPosition position, DateTime time)
        {
            foreach(RotaTimePosition timePos in RotaTimePositions)
            {
                if (timePos.position.Equals(position) && timePos.time == time)
                    return timePos.employee;
            }
            return null;
        }
        public IPosition GetPosition(IEmployee employee, DateTime time)
        {
            foreach(RotaTimePosition timePos in RotaTimePositions)
            {
                if (timePos.employee != null && timePos.employee.Equals(employee) && timePos.time == time)
                    return timePos.position;
            }
            return null;
        }

        protected List<IEmployee> GetEmployees(IPosition position)
        {
            List<IEmployee> officers = new List<IEmployee>();

            foreach(RotaTimePosition timePos in RotaTimePositions)
            {
                if (timePos.position.Equals(position))
                    officers.Add(timePos.employee);
            }

            return officers;
        }

        protected bool IsWorking(IEmployee employee, DateTime time)
        {
            foreach(RotaTimePosition timePos in RotaTimePositions)
            {
                if (timePos.employee != null && timePos.employee.Equals(employee) && timePos.time == time)
                    return true;
            }
            return false;
        }
        
        public bool IsCovered(IPosition position, DateTime time)
        {
            if (!position.IsActive(time))
                return true;
            
            return GetEmployee(position, time) != null;
        }
        public bool IsCovered(IPosition Position)
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
            foreach(IPosition pos in Positions)
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
                        if (timePos.position.Duration() == 1)
                            RotaTimePositions.Remove(timePos);
                        else
                        {
                            if (GetEmployee(timePos.position, time - new TimeSpan(1, 0, 0)).Equals(timePos.employee))
                                continue;
                            else
                                RotaTimePositions.Remove(timePos);
                        }
                    }
                }
            }
        }
        public void Clear(IPosition position)
        {
            RotaTimePositions.RemoveAll(tp => tp.position.Equals(position));
        }

        public void Update(IPosition position, DateTime time, IEmployee newEmployee, bool propagateChanges = false)
        {
            // get officer that was working the position
            IEmployee oldOfficer = GetEmployee(position, time);
            // get positions the new officer was working
            IPosition oldPosition = GetPosition(newEmployee, time);

            // place officer in new position
            foreach(RotaTimePosition timePos in RotaTimePositions)
            {
                if (timePos.position == position && timePos.time == time)
                {
                    timePos.employee = newEmployee;
                    break;
                }
            }

            if (oldOfficer != null && oldPosition != null)
            {
                // place old officer in old position
                foreach(RotaTimePosition timePos in RotaTimePositions)
                {
                    if (timePos.position == oldPosition && timePos.time == time)
                    {
                        timePos.employee = oldOfficer;
                        break;
                    }
                }
            }

            if (propagateChanges)
            {
                // clear future times
                for(DateTime clearTime = time + new TimeSpan(1, 0, 0); clearTime < FinishTime; clearTime += new TimeSpan(1, 0, 0))
                    Clear(time);
                
                for(DateTime coverTime = time + new TimeSpan(1, 0, 0); coverTime < FinishTime; coverTime += new TimeSpan(1, 0, 0))
                    Rota.coverTime(this, coverTime);
            }
        }

        public static Rota Create(List<IEmployee> employees, List<IPosition> positions, DateTime startTime, DateTime finishTime)
        {
            // initialize properties
            Rota rota = new Rota
            {
                RotaTimePositions = new List<RotaTimePosition>(),
                Employees = new List<IEmployee>(employees),
                Positions = new List<IPosition>(positions.OrderBy(p => p.Index())),
                StartTime = startTime,
                FinishTime = finishTime
            };

            // run hrough all times
            for (DateTime time = startTime; time < finishTime; time += new TimeSpan(1, 0, 0))
            {
                coverTime(rota, time);
            }

            return rota;
        }

        protected static void coverTime(Rota rota, DateTime time)
        {
            // run through all te positions
            foreach(IPosition pos in rota.Positions)
            {
                try
                {
                    // try cover the position
                    rota.RotaTimePositions.AddRange(coverPosition(rota, pos, time));
                }
                catch(PositionsNotActiveException notActive) // if the positions is not active at this time
                {
                    rota.RotaTimePositions.Add(new RotaTimePosition() { time = time, position = pos, employee = null, isActive = false });
                }
                catch(PositionsNotCoveredException notCovered) // if The position was not covered
                {
                    rota.RotaTimePositions.Add(new RotaTimePosition() { time = time, position = pos, isActive = true, employee = null });
                }
                catch(OfficerNotFoundException notFound) // if not officer can word the position
                {
                    rota.RotaTimePositions.Add(new RotaTimePosition() { time = time, position = pos, isActive = true, employee = null });
                }
                catch(Exception e)
                {
                }
            }
        }

        protected static List<RotaTimePosition> coverPosition(Rota rota, IPosition pos, DateTime time)
        {
            List<RotaTimePosition> timePos = new List<RotaTimePosition>();
            Random rnd = new Random();

            // is position covered
            if (rota.IsCovered(pos, time))
                throw new PositionsAlreadyCoveredException($"Position: {pos.Name()} already covered at {time}");

            // is not active then add as inactive and move to next position
            if (!pos.IsActive(time))
                throw new PositionsNotActiveException($"Positions: {pos.Name()} is not active at {time}");

            // get all officers that can work position
            List<IEmployee> employList = rota.Employees.Where(e => e.CanWork(pos)).ToList();

            // if no officer can work the positions then throw exception
            if(employList.Count() == 0)
                throw new OfficerNotFoundException($"There are no officers that can work: {pos.Name()}");

            // while the list 
            while(employList.Count() > 0)
            {
                // get random officer
                IEmployee employee = employList[rnd.Next(employList.Count)];

                try
                {
                    // try cover position
                    timePos.AddRange(positionOfficer(rota, employee, pos, time));
                    break;
                }
                catch(OfficerAlreadyWorkingException working) // if officer is already working at this time
                {
                    employList.Remove(employee);
                }
                catch(OfficerCrossoverException crossover) // if there would be a crossover 
                {
                    employList.Remove(employee);
                }
                catch(OfficerPreviousWorkingException prevWork) // if the officer has already worked at this position
                {
                    employList.Remove(employee);
                }
                catch(Exception e)
                {
                }
            }

            if (employList.Count() == 0)
                throw new OfficerNotFoundException($"There are no officers that can work: {pos.Name()}");

            return timePos;
        }

        protected static List<RotaTimePosition> positionOfficer(Rota rota, IEmployee employee, IPosition pos, DateTime time)
        {
            // if officer is already working a position
            if (rota.IsWorking(employee, time))
            {
                throw new OfficerAlreadyWorkingException($"Officer: {employee.Name()} is already working at {pos.Name()} at {time}");
            }

            // if has previous time
            if (time > rota.StartTime)
            {
                DateTime prevTime = time - new TimeSpan(1, 0, 0);

                // did officer work previous time at position
                if (rota.IsCovered(pos, prevTime) && rota.GetEmployee(pos, prevTime).Equals(employee))
                {
                    throw new OfficerPreviousWorkingException($"Officer: {employee.Name()} worked at {pos.Name()} in the last hour");
                }

                // if there a straight swap off officers move to next officer
                if (isCrossover(rota, employee, pos, time))
                {
                    throw new OfficerCrossoverException("There is a crossover between {off.Name} and {lastOff.Name}");
                }

                // if officer has worked the position previously
                if (rota.GetEmployees(pos).Contains(employee))
                {
                    throw new OfficerPreviousWorkingException($"Officer: {employee.Name()} has worked this position previously");
                }
            }

            List<RotaTimePosition> retValue = new List<RotaTimePosition>();

            // cover for duration of position
            for (int d = 0; d < pos.Duration(); d++)
            {
                DateTime coverTime = time + new TimeSpan(d, 0, 0);

                if (coverTime > rota.FinishTime)
                    break;

                if (!pos.IsActive(coverTime))
                    retValue.Add(new RotaTimePosition() { time = coverTime, position = pos, isActive = false });
                else
                    retValue.Add(new RotaTimePosition() { time = coverTime, position = pos, employee = employee });
            }

            return retValue;
        }

        protected static bool isCrossover(Rota rota, IEmployee off, IPosition pos, DateTime time)
        {
            DateTime prevTime = time - new TimeSpan(1, 0, 0);

            // if there a straight swap off officers move to next officer
            IPosition lastPos = rota.GetPosition(off, prevTime); // the position of curr officer last hour
            IEmployee lastOff = rota.GetEmployee(pos, prevTime); // officer in this position last hour
            if (lastPos != null && lastOff != null)
            {
                IEmployee nOff = rota.GetEmployee(lastPos, time);
                IPosition nPos = rota.GetPosition(lastOff, time);
                if (nOff != null && nOff.Equals(lastOff) && nPos != null && nPos.Equals(lastPos))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
