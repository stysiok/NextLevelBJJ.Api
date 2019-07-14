using NextLevelBJJ.DataService.Models;
using NextLevelBJJ.DataServices.Abstraction;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;

namespace NextLevelBJJ.DataServices
{
    public class PassTypesService : IPassTypesService
    {
        private NextLevelContext _db { get; set; }

        public PassTypesService(NextLevelContext db)
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

        public Task<PassType> GetPassTypeById(int passTypeId)
        {
            try
            {
                return Task.FromResult(_db.PassTypes.FirstOrDefault(pt => pt.Id == passTypeId && pt.IsEntityAccesible));
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
                return Task.FromResult(_db.PassTypes.FirstOrDefault(pt => pt.Id == passTypeId && pt.IsEntityAccesible).Entries);
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
                return Task.FromResult(_db.PassTypes.FirstOrDefault(pt => pt.Id == passTypeId && pt.IsEntityAccesible).Name == "Dzieci");
            }
            catch (Exception ex)
            {
                throw new Exception("Błąd podczas pobierania informacji czy karnet jest karnetem dziecięcym. Dodatkowa informacja: " + ex.Message);
            }
        }

        public Task<List<PassType>> GetPassTypes()
        {
            try
            {
                return Task.FromResult(_db.PassTypes.Where(pt => pt.IsEntityAccesible).ToList());
            }
            catch (Exception ex)
            {
                throw new Exception("Błąd podczas pobierania informacji o karnetach. Dodatkowa informacja: " + ex.Message);
            }
        }
    }
}
