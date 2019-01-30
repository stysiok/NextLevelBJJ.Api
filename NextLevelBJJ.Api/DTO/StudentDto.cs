using System;
using System.Collections.Generic;

namespace NextLevelBJJ.Api.DTO
{
    public class StudentDto
    {
        public int Id { get; set; }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PassCode { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool HasDeclaration { get; set; }
        public AttendanceDto LastAttendance { get; set; }

        public ICollection<AttendanceDto> Attendances { get; set; }
        public ICollection<PassDto> Passes { get; set; }
        public PassDto RecentPass { get; set; }
    }
}
