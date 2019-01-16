using NextLevelBJJ.Core.Abstractions;
using NextLevelBJJ.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NextLevelBJJ.Core.Classes
{
    public class Student : Entity
    {
        public string Address { get; set; }

        public IEnumerable<Attendance> Attendances { get; set; }

        public DateTime BirthDate { get; set; }

        public string Email { get; set; }

        public int EntriesLeft { get; set; }

        public string FirstName { get; set; }

        public Gender Gender { get; set; }

        public bool HasDeclaration { get; set; }

        public DateTime LastAttendance { get; set; }

        public string LastName { get; set; }

        public string PassCode { get; set; }

        public IEnumerable<Pass> Passes { get; set; }

        public int PhoneNumber { get; set; }

        public Pass RecentPass { get; set; }
    }
}
