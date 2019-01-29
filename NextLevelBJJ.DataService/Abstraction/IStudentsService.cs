using NextLevelBJJ.DataService.Models;
using System.Threading.Tasks;

namespace NextLevelBJJ.DataServices.Abstraction
{
    public interface IStudentsService
    {
        Task<Student> GetStudentByPassCode(string passCode);
    }
}
