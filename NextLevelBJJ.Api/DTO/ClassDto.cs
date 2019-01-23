using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextLevelBJJ.Api.DTO
{
    public class ClassDto
    {
        public DayOfWeek Day { get; set; }

        public string Name { get; set; }

        public DateTime StartHour { get; set; }

        public DateTime FinishHour { get; set; }

        public bool IsKidsClass => Name.Contains("dzieci");
    }
}
