using NextLevelBJJ.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NextLevelBJJ.Core.Classes
{
    public class Attendance : Entity
    {
        public bool IsFree { get; set; }

        public Pass Pass { get; set; }

        public Student Student { get; set; }
    }
}
