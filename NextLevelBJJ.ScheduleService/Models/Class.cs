using System;
using System.Collections.Generic;
using System.Text;

namespace NextLevelBJJ.ScheduleService.Models
{
    public class Class
    {
        public DayOfWeek Day { get; set; }

        public string Name { get; set; }

        public TimeSpan StartHour { get; set; }

        public TimeSpan FinishHour { get; set; }

        public bool IsKidsClass => Name.Contains("dzieci");

    }
}
