using System;
using System.Collections.Generic;

namespace NextLevelBJJ.DataService.Models
{
    public partial class Attendance
    {
        public int Id { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }

        public int? PassId { get; set; }
        public int? StudentId { get; set; }
        public bool IsFree { get; set; }

        public Pass Pass { get; set; }
        public Student Student { get; set; }
    }
}
