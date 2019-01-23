using NextLevelBJJ.DataService.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelBJJ.DataServices.Abstraction
{
    public interface IPassTypesService
    {
        Task<PassType> GetPassTypeById(int passTypeId);
    }
}
