using NextLevelBJJ.ScheduleService.Models;
using NextLevelBJJ.WebContentServices.Abstraction;
using System;

namespace NextLevelBJJ.WebContentServices
{
    public class ClassesService : IClassesService
    {
        private readonly ITrainingsService trainingsService;

        public ClassesService(ITrainingsService trainingsService)
        {
            this.trainingsService = trainingsService;
        }

        public Class GetClass(DateTime date)
        {
            var dayOfWeek = date.DayOfWeek;

            var classesForDay = trainingsService.GetTrainingDay(dayOfWeek);

            if (classesForDay.Classes == null)
                throw new Exception("Dla dnia z podanej daty nie ma treningu");

            Class classObj = null;
            int minTimeDiff = Int32.MaxValue;
            foreach(var classInDay in classesForDay.Classes)
            {
                var diffSeconds = Math.Abs((date.TimeOfDay - classInDay.StartHour).Seconds);

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
