using NextLevelBJJ.DataServices.Models;
using NextLevelBJJ.DataServices.Models.Abstraction;
using System;
using System.Collections.Generic;

namespace NextLevelBJJ.DataService.Models
{
    public partial class Student : Entity
    {
        public Student()
        {
            Attendances = new HashSet<Attendance>();
            Passes = new HashSet<Pass>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PassCode { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool HasDeclaration { get; set; }

        public ICollection<Attendance> Attendances { get; set; }
        public ICollection<Pass> Passes { get; set; }
    }
}
