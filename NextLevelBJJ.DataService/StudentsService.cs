using NextLevelBJJ.DataService.Models;
using NextLevelBJJ.DataServices.Abstraction;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NextLevelBJJ.DataServices
{
    public class StudentsService : IStudentsService
    {
        private NextLevelContext _db { get; set; }

        public StudentsService(NextLevelContext db)
        {
            try
            {
                if (db.ChangeTracker != null)
                    db.ChangeTracker.QueryTrackingBehavior = Microsoft.EntityFrameworkCore.QueryTrackingBehavior.NoTracking;
            }
            catch (Exception)
            {
            }
            _db = db;
        }

        public Task<Student> GetStudentByPassCode(string passCode)
        {
            try
            {
                return Task.FromResult(_db.Students.FirstOrDefault(s => s.PassCode == passCode && s.IsEntityAccesible));
            }
            catch (Exception ex)
            {
                throw new Exception("Błąd podczas pobierania profilu klubowicza. Dodatkowa informacja: " + ex.Message);
            }
        }
    }
}
