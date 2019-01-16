using NextLevelBJJ.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NextLevelBJJ.Core.Classes
{
    public class Pass : Entity
    {
        public IEnumerable<Attendance> Attendances { get; set; }

        public DateTime ExpirationDate { get; set; }

        public decimal Price { get; set; }

        public int RemainingEntries { get; set; }

        public Student Student { get; set; }

        public PassType Type { get; set; }
    }
}
