using NextLevelBJJ.DataService.Models;
using NextLevelBJJ.DataServices.Abstraction;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NextLevelBJJ.DataServices
{
    public class PassesService : IPassesService
    {
        private NextLevelContext _db { get; set; }

        public PassesService(NextLevelContext db)
        {
            _db = db;
        }

        public Task<Pass> GetPassByStudentId(int studentId)
        {
            return Task.FromResult(_db.Passes.Last(p => p.StudentId == studentId && p.IsEnabled && !p.IsDeleted));
        }
    }
}
