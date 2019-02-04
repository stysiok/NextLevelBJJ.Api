using AutoMapper;
using GraphQL;
using GraphQL.Types;
using NextLevelBJJ.Api.DTO;
using NextLevelBJJ.DataService.Models;
using NextLevelBJJ.DataServices.Abstraction;
using System;
using System.Collections.Generic;

namespace NextLevelBJJ.Api.Types
{
    public class StudentType : ObjectGraphType<StudentDto>
    {
        public StudentType(IAttendancesService attendancesService, IPassesService passesService, IMapper mapper)
        {
            Name = "Student";
            Description = "Student in the academy";
            Field(s => s.Id, type: typeof(IdGraphType)).Description("Id of the student");
            Field(s => s.FirstName).Description("First name of the student");
            Field(s => s.LastName).Description("Last name of the student");
            Field(s => s.HasDeclaration).Description("Indicator if student singed the declaration");
            Field(s => s.PassCode).Description("Pass code of the student's pass");
            Field(s => s.PhoneNumber).Description("Student's phone number");
            Field(s => s.Email).Description("Student's email");
            Field(s => s.Address).Description("Student's address");
            Field(s => s.BirthDate).Description("Student's birth date");
            Field(s => s.Gender).Description("Student's gender");
            Field<ListGraphType<AttendanceType>>(
                "Attendances",
                description: "Student's attandances at the trainings",
                resolve: ctx =>
                {
                    List<Attendance> result = null;
                    try
                    {
                        result = attendancesService.GetStudentAttendences(ctx.Source.Id, 0, 8).Result;
                    }
                    catch (Exception ex)
                    {
                        ctx.Errors.Add(new ExecutionError(ex.Message));
                    }

                    return mapper.Map<List<AttendanceDto>>(result);
                }
            );
            Field<ListGraphType<PassType>>(
                "Passes",
                description: "Student's passes to the academy",
                resolve: ctx =>
                {
                    List<Pass> result = null;
                    try
                    {
                        result = passesService.GetStudentPasses(ctx.Source.Id).Result;
                    }
                    catch (Exception ex)
                    {
                        ctx.Errors.Add(new ExecutionError(ex.Message));
                    }

                    return mapper.Map<List<PassDto>>(result);
                }
            );
            Field<PassType>(
                "RecentPass",
                description: "The most recent pass of the user (current if not expired)",
                resolve: ctx =>
                {
                    Pass result = null;
                    try
                    {
                        result = passesService.GetRecentStudentPass(ctx.Source.Id).Result;
                    }
                    catch (Exception ex)
                    {
                        ctx.Errors.Add(new ExecutionError(ex.Message));
                    }

                    return mapper.Map<PassDto>(result);
                }
            );
            Field<AttendanceType>(
                "LastAttendance",
                description: "The most recent visit in the club",
                resolve: ctx =>
                {
                    Attendance result = null;
                    try
                    {
                        result = attendancesService.GetRecentAttendance(ctx.Source.Id).Result;
                    }
                    catch (Exception ex)
                    {
                        ctx.Errors.Add(new ExecutionError(ex.Message));
                    }

                    return mapper.Map<AttendanceDto>(result);
                }
            );
        }
    }
}
