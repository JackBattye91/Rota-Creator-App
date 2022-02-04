using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackB
{
    public class Time
    {
        public int Hours { get { return (int)time / 86400; } }
        public int Minutes { get { return (int)time / 3600; } }
        public int Seconds { get { return (int)time % 60; } }

        protected uint time;

        public Time(int hours = 0, int minutes = 0, int seconds = 0)
        {
            time += ((uint)hours * 3600 + (uint)minutes * 60 + (uint)seconds) % 86400;
        }

        public void AddTime(Time time)
        {
            this.time = (this.time + time.time) % 86400;
        }
        public void AddHours(int hours)
        {
            time += (uint)hours * 3600;
            time %= 86400;
        }
        public void AddMinutes(int minutes)
        {
            time += (uint)minutes * 60;
            time %= 86400;
        }
        public void AddSeconds(int seconds)
        {
            time += (uint)seconds;
            time %= 86400;
        }

        public bool Equals(Time obj)
        {
            return time == obj.time;
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as Time);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return Hours.ToString("00") + ":" + Minutes.ToString("00") + ":" + Seconds.ToString("00");
        }

        public static Time operator+(Time time1, Time time2)
        {
            return new Time() { time = (time1.time + time2.time) % 86400 };
        }

    }
}
