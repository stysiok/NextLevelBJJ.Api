using GraphQL;
using GraphQL.Types;
using NextLevelBJJ.Api.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextLevelBJJ.Api.Types
{
    public class AttendanceType : ObjectGraphType<AttendanceDto>
    {
        public AttendanceType()
        {
            Name = "Attendance";
            Description = "Training in the academy";
            Field(a => a.Id, type: typeof(IdGraphType)).Description("Id of the attendance");
            Field(a => a.CreatedDate).Description("When the attendance took place");
            Field(a => a.IsFree).Description("Was the attendance free of charge");
            Field(a => a.PassId, type: typeof(IdGraphType)).Description("Id of the pass on which the attendance has been recorded");
            Field(a => a.StudentId, type: typeof(IdGraphType)).Description("Id of the student who attended training");
           // Field<PassType>(
           //     "Pass",
           //     description: "Pass on which the attendance has been recoded",
           //     resolve: ctx =>
           //     {
           //         try
           //         {
           //             return null;
           //         }
           //         catch (Exception e)
           //         {
           //             ctx.Errors.Add(new ExecutionError("Błąd podczas pobierania danych na temat karnetu użytownika"));
           //         }
           //     }
           // );
           // Field<StudentType>(
           //    "Student",
           //    description: "Student who attended the training",
           //    resolve: ctx =>
           //    {
           //        return null;
           //    }
           //);
        }
    }
}
