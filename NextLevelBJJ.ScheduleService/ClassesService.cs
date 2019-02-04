using NextLevelBJJ.WebContentServices.Models;
using NextLevelBJJ.WebContentServices.Abstraction;
using System;
using System.Linq;

namespace NextLevelBJJ.WebContentServices
{
    public class ClassesService : IClassesService
    {
        private readonly ITrainingsService trainingsService;

        public ClassesService(ITrainingsService trainingsService)
        {
            this.trainingsService = trainingsService;
        }

        public Class GetClass(DateTime date, bool kidsClassFilter)
        {
            var dayOfWeek = date.DayOfWeek;

            var classesForDay = trainingsService.GetTrainingDay(dayOfWeek);

            if (classesForDay.Classes == null)
                return null;

            var classes = classesForDay.Classes.Where(c => c.IsKidsClass == kidsClassFilter).ToList();
            Class classObj = null;
            double minTimeDiff = Int32.MaxValue;
            foreach(var classInDay in classes)
            {
                var diffSeconds = Math.Abs((date.TimeOfDay - classInDay.StartHour).TotalSeconds);

                if(diffSeconds < minTimeDiff)
                {
                    minTimeDiff = diffSeconds;
                    classObj = classInDay;
                }
            }

            return classObj;
        }
    }
}
