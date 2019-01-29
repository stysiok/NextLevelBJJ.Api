﻿using NextLevelBJJ.DataService.Models;
using NextLevelBJJ.DataServices.Abstraction;
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

        public Task<List<Pass>> GetStudentPasses(int studentId)
        {
            return Task.FromResult(_db.Passes.Where(p => p.StudentId == studentId && p.IsEnabled && !p.IsDeleted).ToList());
        }

        public Task<Pass> GetRecentStudentPass(int studentId)
        {
            return Task.FromResult(_db.Passes.Last(p => p.StudentId == studentId && p.IsEnabled && !p.IsDeleted));
        }

        public Task<Pass> GetPass(int passId)
        {
            return Task.FromResult(_db.Passes.FirstOrDefault(p => p.Id == passId && p.IsEnabled && !p.IsDeleted));
        }
    }
}
