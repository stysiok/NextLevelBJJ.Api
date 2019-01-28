using NextLevelBJJ.DataService.Models;
using System.Threading.Tasks;

namespace NextLevelBJJ.DataServices.Abstraction
{
    public interface IPassTypesService
    {
        Task<PassType> GetPassTypeById(int passTypeId);

        Task<int> GetPassTypeEntriesById(int passTypeId);
    }
}
