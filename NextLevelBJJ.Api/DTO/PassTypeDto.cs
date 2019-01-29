using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextLevelBJJ.Api.DTO
{
    public class PassTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Entries { get; set; }
        public bool IsOpen { get; set; }
    }
}
