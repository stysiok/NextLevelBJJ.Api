using NextLevelBJJ.DataService.Models;
using NextLevelBJJ.DataServices.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelBJJ.DataServices
{
    public class StudentsService : IStudentsService
    {
        private NextLevelContext _db { get; set; }

        public StudentsService(NextLevelContext db)
        {
            _db = db;
        }

        public Task<Student> GetStudentByPassCode(string passCode)
        {
            return Task.FromResult(_db.Students.FirstOrDefault(s => s.PassCode == passCode && s.IsEnabled && !s.IsDeleted));
        }
    }
}
