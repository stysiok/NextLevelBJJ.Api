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
        private IPassTypesService _passTypesService;
        private IAttendancesService _attendancesService;

        public PassesService(NextLevelContext db, IPassTypesService passTypesService, IAttendancesService attendancesService)
        {
            _db = db;
            _passTypesService = passTypesService;
            _attendancesService = attendancesService;
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
                return Task.FromResult(_db.Passes.OrderByDescending(p => p.CreatedDate)
                                                 .First(p => p.StudentId == studentId && p.IsEntityAccesible));
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

        public Task<int> GetRemainingEntriesOnPass(int passId)
        {
            try
            {
                return Task<int>.Factory.StartNew(() =>
                {
                    var pass = GetPass(passId).Result;

                    var entries = _passTypesService.GetPassTypeEntriesById(pass.TypeId).Result;
                    
                    var attendancesCount = _attendancesService.GetAttendancesAmountTrackedOnPass(passId).Result;

                    return entries - attendancesCount;
                });
            }
            catch (Exception ex)
            {
                throw new Exception("Błąd podczas pobierania karnetu. Dodatkowa informacja: " + ex.Message);
            }
        }
    }
}
