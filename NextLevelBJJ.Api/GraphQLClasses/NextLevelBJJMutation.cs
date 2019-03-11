using AutoMapper;
using GraphQL;
using GraphQL.Types;
using NextLevelBJJ.Api.Types;
using NextLevelBJJ.DataServices.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextLevelBJJ.Api.GraphQLClasses
{
    public class NextLevelBJJMutation : ObjectGraphType
    {
        public NextLevelBJJMutation(IPassesService passesService, IAttendancesService attendancesService, IMapper mapper)
        {
            Name = "Mutation";
            Description = "Adding data to the Next Level BJJ database";
            Field<BooleanGraphType>(
                "signInForTraining",
                description: "Signs in student for a training",
                arguments: new QueryArguments(
                     new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "studentId", Description = "Id of a student" }
                    ),
                resolve: ctx =>
                {
                    var studentId = ctx.GetArgument<int>("studentId");

                    var pass = passesService.GetRecentStudentPass(studentId).Result;
                    if(pass.ExpirationDate < DateTime.Now)
                    {
                        throw new ExecutionError($"Twój karnet stracił ważność w dniu {pass.ExpirationDate.Date}.");
                    }

                    var remainingEntries = passesService.GetRemainingEntriesOnPass(pass.Id).Result;
                    if (remainingEntries < 0)
                    {
                        throw new ExecutionError("Wykorzystałeś wszystkie wejścia dostępne na karnecie.");
                    }

                    bool signedIn = false;
                    try
                    {
                        signedIn = attendancesService.AddAttendance(studentId, pass.Id).IsCompletedSuccessfully;
                    }
                    catch(Exception ex)
                    {
                        ctx.Errors.Add(new ExecutionError(ex.Message));
                    }

                    return signedIn;
                }
            );
        }
    }
}
