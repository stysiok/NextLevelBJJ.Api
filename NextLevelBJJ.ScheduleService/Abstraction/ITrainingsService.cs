using NextLevelBJJ.ScheduleService.Models;
using System;
using System.Collections.Generic;

namespace NextLevelBJJ.WebContentServices.Abstraction
{
    public interface ITrainingsService
    {
        TrainingDay GetTrainingDay(DayOfWeek dayOfWeek);

        List<TrainingDay> GetTrainingWeek();
    }
}
