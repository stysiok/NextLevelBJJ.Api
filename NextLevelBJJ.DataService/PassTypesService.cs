using NextLevelBJJ.DataService.Models;
using NextLevelBJJ.DataServices.Abstraction;
using System.Threading.Tasks;
using System.Linq;

namespace NextLevelBJJ.DataServices
{
    public class PassTypesService : IPassTypesService
    {
        private NextLevelContext _db { get; set; }

        public PassTypesService(NextLevelContext db)
        {
            _db = db;
        }

        public Task<PassType> GetPassTypeById(int passTypeId)
        {
            return Task.FromResult(_db.PassTypes.FirstOrDefault(pt => pt.Id == passTypeId && pt.IsEnabled && !pt.IsDeleted));
        }
    }
}
