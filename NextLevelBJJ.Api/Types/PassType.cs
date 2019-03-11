using AutoMapper;
using GraphQL;
using GraphQL.Types;
using NextLevelBJJ.Api.DTO;
using NextLevelBJJ.DataServices.Abstraction;
using System;

namespace NextLevelBJJ.Api.Types
{
    public class PassType : ObjectGraphType<PassDto>
    {
        public PassType(IPassesService passesService, IPassTypesService passTypesService, IAttendancesService attendancesService, IMapper mapper)
        {
            Name = "Pass";
            Description = "Pass which allows to train in the academy";
            Field(p => p.Id, type: typeof(IdGraphType)).Description("Id of the pass");
            Field(p => p.Price).Description("Price of the pass");
            Field(p => p.CreatedDate, type: typeof(DateTimeGraphType)).Description("Creation date of the pass");
            Field(p => p.ExpirationDate, type: typeof(DateTimeGraphType)).Description("When the pass expires");
            Field(p => p.TypeId, type: typeof(IdGraphType)).Description("Id of a pass type which the pass has been based on");
            Field(p => p.StudentId, type: typeof(IdGraphType)).Description("Id of the student to whom the pass is assigned");
            Field(p => p.RemainingEntries)
                .Description("Remaining entries on the pass")
                .Resolve(ctx =>
                    {
                        int result = -1;
                        try
                        {
                            var passId = ctx.Source.Id;
                            result = passesService.GetRemainingEntriesOnPass(passId).Result;
                        }
                        catch (Exception ex)
                        {
                            ctx.Errors.Add(new ExecutionError(ex.Message));
                        }

                        return result;
                    });
            Field<PassTypeType>(
                "PassType",
                description: "Pass type on which the pass has been based",
                resolve: ctx =>
                {
                    DataService.Models.PassType passType = null;
                    try
                    {
                        var passTypeId = ctx.Source.TypeId;
                        passType = passTypesService.GetPassTypeById(passTypeId).Result;
                    }
                    catch (Exception ex)
                    {
                        ctx.Errors.Add(new ExecutionError(ex.Message));
                    }

                    return mapper.Map<PassTypeDto>(passType);
                }
            );
        }
    }
}
