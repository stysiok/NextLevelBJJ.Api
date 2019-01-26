using AutoMapper;
using GraphQL;
using GraphQL.Types;
using NextLevelBJJ.Api.DTO;
using NextLevelBJJ.DataServices.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextLevelBJJ.Api.Types
{
    public class PassType : ObjectGraphType<PassDto>
    {
        public PassType(IPassTypesService passTypesService, IMapper mapper)
        {
            Name = "Pass";
            Description = "Pass which allows to train in the academy";
            Field(p => p.Id, type: typeof(IdGraphType)).Description("Id of the pass");
            Field(p => p.Price).Description("Price of the pass");
            Field(p => p.CreatedDate).Description("Creation date of the pass");
            Field(p => p.ExpirationDate).Description("When the pass expires");
            Field(p => p.TypeId, type: typeof(IdGraphType)).Description("Id of a pass type which the pass has been based on");
            Field(p => p.StudentId, type: typeof(IdGraphType)).Description("Id of the student to whom the pass is assigned");
            //Field<StudentType>(
            //    "Student",
            //    description: "Student to whom the pass is assigned",
            //    resolve: ctx =>
            //    {
            //        return new StudentType();
            //    }
            //);
            Field<PassTypeType>(
                "PassType",
                description: "Pass type on which the pass has been based",
                resolve: ctx =>
                {
                    var passTypeId = ctx.Source.TypeId;

                    DataService.Models.PassType passType = null;
                    try
                    {
                        passType = passTypesService.GetPassTypeById(passTypeId).Result;
                    }
                    catch (Exception ex)
                    {
                        ctx.Errors.Add(new ExecutionError("Błąd podczas pobierania informacji odnośnie typu karnetu"));
                    }

                    return mapper.Map<PassTypeDto>(passType);
                }
            );
        }
    }
}
