using System;
using System.Collections.Generic;
using System.Text;

namespace NextLevelBJJ.ScheduleService.Models
{
    public class Class
    {
        public DayOfWeek Day { get; set; }

        public string Name { get; set; }

        public DateTime StartHour { get; set; }

        public DateTime FinishHour { get; set; }

    }
}
