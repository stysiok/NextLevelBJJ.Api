using NextLevelBJJ.DataService.Models;
using NextLevelBJJ.DataServices.Abstraction;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextLevelBJJ.DataServices
{
    public class AttendancesService : IAttendancesService
    {
        private NextLevelContext _db { get; set; }

        public AttendancesService(NextLevelContext db)
        {
            _db = db;
        }

        public Task<List<Attendance>> GetStudentAttendences(int studentId, int skip, int take)
        {
            return Task.FromResult(_db.Attendances
                .Where(a => a.StudentId == studentId && a.IsEnabled && !a.IsDeleted)
                .OrderByDescending(a => a.CreatedDate)
                .Skip(skip)
                .Take(take)
                .ToList());
        }

        public Task<int> GetAttendancesAmountTrackedOnPass(int passId)
        {
            return Task.FromResult(_db.Attendances.Count(a => a.PassId == passId && a.IsEnabled && !a.IsDeleted));
        }

        public Task<Attendance> GetRecentAttendance(int studentId)
        {
            return Task.FromResult(_db.Attendances
                .Where(a => a.StudentId == studentId && a.IsEnabled && !a.IsDeleted)
                .OrderByDescending(a => a.CreatedDate)
                .FirstOrDefault());
        }
    }
}
