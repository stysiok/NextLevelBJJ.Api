using NextLevelBJJ.DataService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NextLevelBJJ.DataServices.Abstraction
{
    public interface IAttendancesService
    {
        Task<List<Attendance>> GetStudentAttendences(int studentId, int skip, int take);

        Task<int> GetAttendancesAmountTrackedOnPass(int passId);

        Task<Attendance> GetRecentAttendance(int studentId);

        Task<int> AddAttendance(int passId, int studentId);

        Task<bool> CurrentClassAlreadyAttended(int studentId, bool isKidsPass);
    }
}
