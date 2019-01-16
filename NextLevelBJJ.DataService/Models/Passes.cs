using System;
using System.Collections.Generic;

namespace NextLevelBJJ.DataService.Models
{
    public partial class Passes
    {
        public Passes()
        {
            Attendances = new HashSet<Attendances>();
        }

        public int Id { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }
        public int? StudentId { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }
        public int? TypeId { get; set; }

        public Students Student { get; set; }
        public PassTypes Type { get; set; }
        public ICollection<Attendances> Attendances { get; set; }
    }
}
