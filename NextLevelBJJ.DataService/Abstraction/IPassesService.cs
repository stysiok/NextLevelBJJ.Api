using NextLevelBJJ.DataService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NextLevelBJJ.DataServices.Abstraction
{
    public interface IPassesService
    {
        Task<List<Pass>> GetStudentPasses(int studentId);

        Task<Pass> GetRecentStudentPass(int studentId);

        Task<Pass> GetPass(int passId);
    }
}
