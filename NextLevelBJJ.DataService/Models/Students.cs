using System;
using System.Collections.Generic;

namespace NextLevelBJJ.DataService.Models
{
    public partial class Students
    {
        public Students()
        {
            Attendances = new HashSet<Attendances>();
            Passes = new HashSet<Passes>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }
        public string PassCode { get; set; }
        public int Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool HasDeclaration { get; set; }

        public ICollection<Attendances> Attendances { get; set; }
        public ICollection<Passes> Passes { get; set; }
    }
}
