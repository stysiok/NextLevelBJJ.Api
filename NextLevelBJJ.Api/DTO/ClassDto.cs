using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextLevelBJJ.Api.DTO
{
    public class ClassDto
    {
        public string Day { get; set; }

        public string Name { get; set; }

        public string StartHour { get; set; }

        public string FinishHour { get; set; }

        public bool IsKidsClass => Name.Contains("DZIECI");
    }
}
