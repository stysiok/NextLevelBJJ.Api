using NextLevelBJJ.DataServices.Models.Abstraction;
using System;
using System.Collections.Generic;

namespace NextLevelBJJ.DataService.Models
{
    public partial class Pass : Entity
    {
        public Pass()
        {
            Attendances = new HashSet<Attendance>();
        }

        public DateTime ExpirationDate { get; set; }
        public int StudentId { get; set; }
        public int Price { get; set; }
        public int TypeId { get; set; }

        public Student Student { get; set; }
        public PassType Type { get; set; }
        public ICollection<Attendance> Attendances { get; set; }
    }
}
