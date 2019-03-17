using NextLevelBJJ.WebContentServices.Models;
using NextLevelBJJ.WebContentServices.Abstraction;
using System;
using System.Linq;
using System.Collections.Generic;

namespace NextLevelBJJ.WebContentServices
{
    public class ClassesService : IClassesService
    {
        private readonly ITrainingsService trainingsService;
        private readonly List<TrainingDay> allTrainings;

        public ClassesService(ITrainingsService trainingsService)
        {
            this.trainingsService = trainingsService;
            allTrainings = trainingsService.GetTrainingWeek();
        }

        public Class GetClass(DateTime date, bool kidsClassFilter)
        {
            var dayOfWeek = date.DayOfWeek;

            var classesForDay = allTrainings.FirstOrDefault(t => t.Day == dayOfWeek);

            if (classesForDay == null)
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

        public Class GetUpcomingClass(DateTime currentDate, bool kidsClassFilter)
        {
            var classesForDay = allTrainings.FirstOrDefault(t => t.Day == currentDate.DayOfWeek);
            if (classesForDay == null)
                return null;

            return classesForDay.Classes.FirstOrDefault(c => c.IsKidsClass == kidsClassFilter 
                && c.StartHour >= currentDate.AddMinutes(-15).TimeOfDay
                && c.StartHour < currentDate.AddMinutes(15).TimeOfDay);
        }
    }
}
