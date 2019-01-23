using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextLevelBJJ.Api.DTO
{
    public class PassDto
    {
        public int Id { get; set; }
        
        public DateTime CreatedDate { get; set; }

        public DateTime ExpirationDate { get; set; }
        public int? StudentId { get; set; }
        public int Price { get; set; }
        public int? TypeId { get; set; }

        public StudentDto Student { get; set; }
        public PassTypeDto Type { get; set; }

    }
}
