using System;
using System.Collections.Generic;
using System.Text;

namespace NextLevelBJJ.ScheduleService.Models
{
    public class TrainingDay
    {
        public DayOfWeek Day { get; set; }

        public IEnumerable<Class> Classes { get; set; }
    }
}
