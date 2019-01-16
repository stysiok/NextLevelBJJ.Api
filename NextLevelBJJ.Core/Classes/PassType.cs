using NextLevelBJJ.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NextLevelBJJ.Core.Classes
{
    public class PassType : Entity
    {
        public int Entries { get; set; }

        public bool IsOpen { get; set; }

        public bool IsSingle { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}
