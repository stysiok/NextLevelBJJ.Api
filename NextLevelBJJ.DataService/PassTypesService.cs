using NextLevelBJJ.DataService.Models;
using NextLevelBJJ.DataServices.Abstraction;
using System.Threading.Tasks;
using System.Linq;
using System;

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
            try
            {
                return Task.FromResult(_db.PassTypes.FirstOrDefault(pt => pt.Id == passTypeId && pt.IsEnabled && !pt.IsDeleted));
            }
            catch (Exception ex)
            {
                throw new Exception("Błąd podczas pobierania rodzaju karnetu. Dodatkowa informacja: " + ex.Message);
            }
        }

        public Task<int> GetPassTypeEntriesById(int passTypeId)
        {
            try
            {
                return Task.FromResult(_db.PassTypes.FirstOrDefault(pt => pt.Id == passTypeId && pt.IsEnabled && !pt.IsDeleted).Entries);
            }
            catch (Exception ex)
            {
                throw new Exception("Błąd podczas pobierania ilości wejść opartych na rodzaju karnetu. Dodatkowa informacja: " + ex.Message);
            }
        }

        public Task<bool> IsKidsPass(int passTypeId)
        {
            try
            {
                return Task.FromResult(_db.PassTypes.FirstOrDefault(pt => pt.Id == passTypeId && pt.IsEnabled && !pt.IsDeleted).Name == "Dzieci");
            }
            catch (Exception ex)
            {
                throw new Exception("Błąd podczas pobierania informacji czy karnet jest karnetem dziecięcym. Dodatkowa informacja: " + ex.Message);
            }
        }
    }
}
