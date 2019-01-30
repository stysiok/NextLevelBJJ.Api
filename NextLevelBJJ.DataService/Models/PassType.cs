using NextLevelBJJ.DataServices.Models.Abstraction;
using System.Collections.Generic;

namespace NextLevelBJJ.DataService.Models
{
    public partial class PassType : Entity
    {
        public PassType()
        {
            Passes = new HashSet<Pass>();
        }
        
        public string Name { get; set; }
        public int Price { get; set; }
        public int Entries { get; set; }
        public bool IsOpen { get; set; }

        public ICollection<Pass> Passes { get; set; }
    }
}
