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
        public NextLevelBJJQuery(ITrainingsService trainingsService, IStudentsService studentsService, IAttendancesService attendancesService, IPassTypesService passTypesService, IMapper mapper)
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
            Field<ListGraphType<AttendanceType>>(
                "attendances",
                description: "Get student's attendances",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "studentId" },
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "take" },
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "skip" }
                ),
                resolve: ctx =>
                {
                    var studentId = ctx.GetArgument<int>("studentId");
                    var take = ctx.GetArgument<int>("take");
                    var skip = ctx.GetArgument<int>("skip");

                    List<Attendance> result = null;
                    try
                    {
                        result = attendancesService.GetStudentAttendences(studentId, skip, take).Result;
                    }
                    catch (Exception ex)
                    {
                        ctx.Errors.Add(new ExecutionError(ex.Message));
                    }

                    return mapper.Map<List<AttendanceDto>>(result);
                }
            );
            Field<ListGraphType<PassTypeType>>(
                "passTypes",
                description: "Get all currently available pass types",
                resolve: ctx =>
                {
                    List<DataService.Models.PassType> result = null;
                    try
                    {
                        result = passTypesService.GetPassTypes().Result;
                    }
                    catch (Exception ex)
                    {
                        ctx.Errors.Add(new ExecutionError(ex.Message));
                    }

                    return mapper.Map<List<PassTypeDto>>(result);
                }
            );
        }
    }
}
