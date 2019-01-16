using System;
using System.Collections.Generic;
using System.Text;

namespace NextLevelBJJ.Core.Abstractions
{
    public abstract class Entity
    {
        public string Id { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsEnabled { get; set; }

    }
}
