using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextLevelBJJ.Api.DTO
{
    public class AttendanceDto
    {
        public int Id { get; set; }
        
        public DateTime CreatedDate { get; set; }

        public int? PassId { get; set; }
        public int? StudentId { get; set; }
        public bool IsFree { get; set; }

        public PassDto Pass { get; set; }
        public StudentDto Student { get; set; }

    }
}
