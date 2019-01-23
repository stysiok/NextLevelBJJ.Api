using GraphQL.Types;
using NextLevelBJJ.Api.DTO;
using NextLevelBJJ.Api.Types;
using NextLevelBJJ.DataServices.Abstraction;
using NextLevelBJJ.WebContentServices.Abstraction;
using System;
using System.Linq;

namespace NextLevelBJJ.Api.GraphQLClasses
{
    public class NextLevelBJJQuery : ObjectGraphType
    {
        public NextLevelBJJQuery(ITrainingsService trainingsService)
        {
            Name = "Query";
            Description = "Queries to load the data";
            Field<TrainingDayType>(
                "GetTrainingDay",
                description: "Get the training day",
                resolve: ctx =>
                {
                    var s = trainingsService.GetTrainingDay(DateTime.Today.DayOfWeek);

                    return new TrainingDayDto()
                    {
                        Classes = s.Classes
                        .Select(c => new ClassDto() { Day = c.Day, FinishHour = c.FinishHour, Name = c.Name, StartHour = c.StartHour})
                        .ToList(),
                        Day = s.Day
                    };
                }
            );
        }
    }
}
