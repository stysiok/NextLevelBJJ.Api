using NextLevelBJJ.DataService.Models;
using NextLevelBJJ.DataServices.Abstraction;
using System;
using System.Collections.Generic;
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

        public Task<List<Pass>> GetPassesByStudentId(int studentId)
        {
            return Task.FromResult(new List<Pass> { _db.Passes.Last(p => p.StudentId == studentId && p.IsEnabled && !p.IsDeleted) });
        }
    }
}
