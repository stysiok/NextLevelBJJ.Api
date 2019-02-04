using System;
using System.Collections.Generic;
using System.Text;

namespace NextLevelBJJ.WebContentServices.Models
{
    public class TrainingDay
    {
        public DayOfWeek Day { get; set; }

        public IEnumerable<Class> Classes { get; set; }
    }
}
