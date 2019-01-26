using GraphQL.Types;
using NextLevelBJJ.Api.DTO;
using NextLevelBJJ.WebContentServices.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextLevelBJJ.Api.Types
{
    public class TrainingDayType : ObjectGraphType<TrainingDayDto>
    {
        public TrainingDayType()
        {
            Name = "TrainingDay";
            Description = "Training Day in the academy";
            Field(td => td.Day).Description("Day of the week");
            Field(td => td.Classes, type: typeof(ListGraphType<ClassType>)).Description("Classes on the day");
        }
    }
}
