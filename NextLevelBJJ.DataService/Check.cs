using NextLevelBJJ.DataService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NextLevelBJJ.DataService
{
    public class Check
    {
        AttendanceTrackingContext context = new AttendanceTrackingContext();

        public void check()
        {
            var student = context.Students.First();
        }
    }
}
