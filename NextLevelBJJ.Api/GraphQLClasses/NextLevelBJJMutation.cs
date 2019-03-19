using AutoMapper;
using GraphQL;
using GraphQL.Types;
using NextLevelBJJ.DataService.Models;
using NextLevelBJJ.DataServices.Abstraction;
using NextLevelBJJ.WebContentServices.Abstraction;
using System;

namespace NextLevelBJJ.Api.GraphQLClasses
{
    public class NextLevelBJJMutation : ObjectGraphType
    {
        public NextLevelBJJMutation(IPassesService passesService, IAttendancesService attendancesService, IClassesService classesService, IMapper mapper)
        {
            Name = "Mutation";
            Description = "Adding data to the Next Level BJJ database";
            Field<BooleanGraphType>(
                "signInForTraining",
                description: "Signs in student for a training",
                arguments: new QueryArguments(
                     new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "studentId", Description = "Id of a student" },
                     new QueryArgument<BooleanGraphType> { Name = "kidsClassFilter", Description = "Filter only kids classes when looking for upcoming training", DefaultValue = false }
                     ),
                resolve: ctx =>
                {
                    bool signedIn = false;
                    var studentId = ctx.GetArgument<int>("studentId");

                    Pass pass = null;
                    try
                    {
                        pass = passesService.GetRecentStudentPass(studentId).Result;
                        if (pass.ExpirationDate < DateTime.UtcNow)
                        {
                            throw new ExecutionError($"Twój karnet stracił ważność w dniu {pass.ExpirationDate.AddHours(1).ToString("dd/MM/yyyy HH:mm")}.");
                        }
                    }
                    catch (Exception ex)
                    {
                        ctx.Errors.Add(new ExecutionError(ex.Message));
                        return signedIn;
                    }
                    
                    try
                    {
                        var remainingEntries = passesService.GetRemainingEntriesOnPass(pass.Id).Result;

                        if (remainingEntries < 0)
                        {
                            throw new ExecutionError("Wykorzystałeś wszystkie wejścia dostępne na karnecie.");
                        }
                    }
                    catch (Exception ex)
                    {
                        ctx.Errors.Add(new ExecutionError(ex.Message));
                        return signedIn;
                    }

                    var kidsFilter = ctx.GetArgument<bool>("kidsClassFilter");
                    var upcomingClass = classesService.GetUpcomingClass(DateTime.Now, kidsFilter);
                    if (upcomingClass == null)
                    {
                        ctx.Errors.Add(new ExecutionError($"Na trening możesz odbić się na 15 minut przed jego rozpoczęciem oraz na 15 minut po jego rozpoczęciu."));
                        return signedIn;
                    }

                    try
                    {
                        var isClassAlreadyAttended = attendancesService.CurrentClassAlreadyAttended(studentId, kidsFilter).Result;
                        if (isClassAlreadyAttended)
                        {
                            throw new ExecutionError("Nie można zapisać się dwa razy na te same zajęcia.");
                        }
                    }
                    catch (Exception ex)
                    {
                        ctx.Errors.Add(new ExecutionError(ex.Message));
                        return signedIn;
                    }

                    try
                    {
                        signedIn = attendancesService.AddAttendance(studentId, pass.Id).Result > 0;
                    }
                    catch(Exception ex)
                    {
                        ctx.Errors.Add(new ExecutionError(ex.Message));
                        return signedIn;
                    }

                    return signedIn;
                }
            );
        }
    }
}
