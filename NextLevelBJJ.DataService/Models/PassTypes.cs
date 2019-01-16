using System;
using System.Collections.Generic;

namespace NextLevelBJJ.DataService.Models
{
    public partial class PassTypes
    {
        public PassTypes()
        {
            Passes = new HashSet<Passes>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Entries { get; set; }
        public bool IsOpen { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<Passes> Passes { get; set; }
    }
}
