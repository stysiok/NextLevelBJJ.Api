using NextLevelBJJ.DataServices.Models.Abstraction;
using System;

namespace NextLevelBJJ.DataService.Models
{
    public partial class Attendance : Entity
    {
        public int? PassId { get; set; }
        public int? StudentId { get; set; }
        public bool IsFree { get; set; }

        public Pass Pass { get; set; }
        public Student Student { get; set; }
    }
}
