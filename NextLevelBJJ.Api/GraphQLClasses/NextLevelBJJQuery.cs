using AutoMapper;
using GraphQL;
using GraphQL.Types;
using NextLevelBJJ.Api.DTO;
using NextLevelBJJ.Api.Types;
using NextLevelBJJ.DataServices.Abstraction;
using NextLevelBJJ.WebContentServices.Models;
using NextLevelBJJ.WebContentServices.Abstraction;
using System;
using NextLevelBJJ.DataService.Models;
using System.Collections.Generic;

namespace NextLevelBJJ.Api.GraphQLClasses
{
    public class NextLevelBJJQuery : ObjectGraphType
    {
        public NextLevelBJJQuery(ITrainingsService trainingsService, IStudentsService studentsService,IMapper mapper)
        {
            Name = "Query";
            Description = "Queries to load the data";
            Field<TrainingDayType>(
                "training",
                description: "Get the training day",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "day", DefaultValue = DateTime.Today.DayOfWeek.ToString() }    
                ),
                resolve: ctx =>
                {
                    var specifiedDay = ctx.GetArgument<string>("day");
                    DayOfWeek dayOfWeek;

                    if(!Enum.TryParse(specifiedDay, out dayOfWeek))
                    {
                        ctx.Errors.Add(new ExecutionError("Podany dzień tygodnia jest nieprawidłowy"));
                    }

                    TrainingDay day = null;
                    try
                    {
                        day = trainingsService.GetTrainingDay(dayOfWeek);
                    }
                    catch (Exception ex)
                    {
                        ctx.Errors.Add(new ExecutionError(ex.Message));
                    }

                    return mapper.Map<TrainingDayDto>(day);
                }
            );
            Field<ListGraphType<TrainingDayType>>(
                "trainings",
                description: "Get the training week",
                resolve: ctx =>
                {
                    List<TrainingDay> day = null;
                    try
                    {
                        day = trainingsService.GetTrainingWeek();
                    }
                    catch (Exception ex)
                    {
                        ctx.Errors.Add(new ExecutionError(ex.Message));
                    }

                    return mapper.Map<List<TrainingDayDto>>(day);
                }
            );
            Field<StudentType>(
                "student",
                description: "Get student by pass id assigned to him",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "passCode" }    
                ),
                resolve: ctx =>
                {
                    var passCode = ctx.GetArgument<string>("passCode");

                    Student result = null;
                    try
                    {
                        result = studentsService.GetStudentByPassCode(passCode).Result;
                    }
                    catch (Exception ex)
                    {
                        ctx.Errors.Add(new ExecutionError(ex.Message));
                    }

                    return mapper.Map<StudentDto>(result);
                }
            );
        }
    }
}
