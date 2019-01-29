using AutoMapper;
using GraphQL;
using GraphQL.Types;
using NextLevelBJJ.Api.DTO;
using NextLevelBJJ.DataService.Models;
using NextLevelBJJ.DataServices.Abstraction;
using NextLevelBJJ.ScheduleService.Models;
using NextLevelBJJ.WebContentServices.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextLevelBJJ.Api.Types
{
    public class AttendanceType : ObjectGraphType<AttendanceDto>
    {
        public AttendanceType(IClassesService classesService, IPassesService passesService, IPassTypesService passTypesService, IMapper mapper)
        {
            Name = "Attendance";
            Description = "Training in the academy";
            Field(a => a.Id, type: typeof(IdGraphType)).Description("Id of the attendance");
            Field(a => a.CreatedDate).Description("When the attendance took place");
            Field(a => a.IsFree).Description("Was the attendance free of charge");
            Field(a => a.PassId, type: typeof(IdGraphType)).Description("Id of the pass on which the attendance has been recorded");
            Field(a => a.StudentId, type: typeof(IdGraphType)).Description("Id of the student who attended training");
            Field<ClassType>(
                "ClassAttended",
                description: "Class related to the attendance",
                resolve: ctx =>
                {
                    var passId = ctx.Source.PassId;
                    Pass pass = null;
                    try
                    {
                        pass = passesService.GetPass(passId).Result;
                    }
                    catch (Exception ex)
                    {
                        ctx.Errors.Add(new ExecutionError(ex.Message));
                    }

                    var passTypeId = pass.TypeId;
                    bool isKidsPass = false;
                    try
                    {
                        isKidsPass = passTypesService.IsKidsPass(passTypeId).Result;
                    }
                    catch (Exception ex)
                    {
                        ctx.Errors.Add(new ExecutionError("Błąd podczas pobierania danych odnośnie typu karnetu"));
                    }

                    var dateOfCreation = ctx.Source.CreatedDate;
                    Class relatedClass = null;
                    try
                    {
                        relatedClass = classesService.GetClass(dateOfCreation, isKidsPass);
                    }
                    catch (Exception ex)
                    {
                        ctx.Errors.Add(new ExecutionError(ex.Message));
                    }

                    return mapper.Map<ClassDto>(relatedClass);
                }
            );
        }
    }
}
