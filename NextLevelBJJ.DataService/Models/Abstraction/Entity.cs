using System;

namespace NextLevelBJJ.DataServices.Models.Abstraction
{
    public abstract class Entity : IAuditFields, IExistanceFields
    {
        public int Id { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        public bool IsEntityAccesible => IsEnabled && !IsDeleted;
    }
}
