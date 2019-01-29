using System;
using System.Collections.Generic;

namespace NextLevelBJJ.DataService.Models
{
    public partial class Pass
    {
        public Pass()
        {
            Attendances = new HashSet<Attendance>();
        }

        public int Id { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime ExpirationDate { get; set; }
        public int? StudentId { get; set; }
        public int Price { get; set; }
        public int TypeId { get; set; }

        public Student Student { get; set; }
        public PassType Type { get; set; }
        public ICollection<Attendance> Attendances { get; set; }
    }
}
