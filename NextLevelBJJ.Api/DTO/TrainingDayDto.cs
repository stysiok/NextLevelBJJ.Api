﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextLevelBJJ.Api.DTO
{
    public class TrainingDayDto
    {
        public string Day { get; set; }

        public IEnumerable<ClassDto> Classes { get; set; }
    }
}
