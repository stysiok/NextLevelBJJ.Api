using NextLevelBJJ.ScheduleService.Models;
using System;

namespace NextLevelBJJ.WebContentServices.Abstraction
{
    public interface IClassesService
    {
        Class GetClass(DateTime date, bool kidsClassFilter);
    }
}
