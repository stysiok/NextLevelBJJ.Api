using GraphQL.Types;
using NextLevelBJJ.Api.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextLevelBJJ.Api.Types
{
    public class PassType : ObjectGraphType<PassDto>
    {
        public PassType()
        {
            Name = "Pass";
            Description = "Pass which allows to train in the academy";
            Field(p => p.Id, type: typeof(IdGraphType)).Description("Id of the pass");
            Field(p => p.Price).Description("Price of the pass");
            Field(p => p.CreatedDate).Description("Creation date of the pass");
            Field(p => p.ExpirationDate).Description("When the pass expires");
            Field(p => p.TypeId, type: typeof(IdGraphType)).Description("Id of a pass type which the pass has been based on");
            Field(p => p.StudentId, type: typeof(IdGraphType)).Description("Id of the student to whom the pass is assigned");
            Field<StudentType>(
                "Student",
                description: "Student to whom the pass is assigned",
                resolve: ctx =>
                {
                    return new StudentType();
                }
            );
            Field<PassTypeType>(
                "Pass Type",
                description: "Pass type on which the pass has been based",
                resolve: ctx =>
                {
                    return new PassTypeType();
                }
            );
        }
    }
}
