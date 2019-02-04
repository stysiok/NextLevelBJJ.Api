using System;

namespace NextLevelBJJ.DataServices.Models.Abstraction
{
    public interface IAuditFields
    {
        int CreatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        int ModifiedBy { get; set; }
        DateTime ModifiedDate { get; set; }
    }
}
