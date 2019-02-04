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

        public Task<List<Pass>> GetStudentPasses(int studentId)
        {
            try
            {
                return Task.FromResult(_db.Passes.Where(p => p.StudentId == studentId && p.IsEntityAccesible)
                    .OrderByDescending(p => p.CreatedDate)
                    .ToList());
            }
            catch (Exception ex)
            {
                throw new Exception("Błąd podczas pobierania karnetów klubowicza. Dodatkowa informacja: " + ex.Message);
            }
        }

        public Task<Pass> GetRecentStudentPass(int studentId)
        {
            try
            {
                return Task.FromResult(_db.Passes.Last(p => p.StudentId == studentId && p.IsEntityAccesible));
            }
            catch (Exception ex)
            {
                throw new Exception("Błąd podczas pobierania ostatniego karnetu klubowicza. Dodatkowa informacja: " + ex.Message);
            }
        }

        public Task<Pass> GetPass(int passId)
        {
            try
            {
                return Task.FromResult(_db.Passes.FirstOrDefault(p => p.Id == passId && p.IsEntityAccesible));
            }
            catch (Exception ex)
            {
                throw new Exception("Błąd podczas pobierania karnetu. Dodatkowa informacja: " + ex.Message);
            }
        }
    }
}
