using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rota_Creator_App
{
    public class Position
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public bool[] ActiveHours { get; set; }

        public Position(string name = "", int duration = 1)
        {
            Name = name;
            DefaultDuration = duration;
            ActiveHours = new bool[167];
            for(int b = 0; b < 167; b++)
                ActiveHours[b] = true;
        }

        public bool IsActive(DateTime time)
        {
            // convert Day to week from sun-sat TO mon-sun
            int day = time.DateOfWeek == DayOfWeek.Sunday ? 6 : ((int)time.DayOfWeek - 1);

            return ActiveHours[day * 24 + time.Hour];
        }
    }
}