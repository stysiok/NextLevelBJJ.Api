using GraphQL.Types;
using NextLevelBJJ.Api.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextLevelBJJ.Api.Types
{
    public class PassTypeType : ObjectGraphType<PassTypeDto>
    {
        public PassTypeType()
        {
            Name = "PassType";
            Description = "Type of the pass";
            Field(pt => pt.Id, type: typeof(IdGraphType)).Description("Id of the pass type");
            Field(pt => pt.Entries).Description("Number of entries for the pass");
            Field(pt => pt.IsOpen).Description("The indicator whether the pass has an infinite number of entries");
            Field(pt => pt.Name).Description("Name of the pass");
            Field(pt => pt.Price).Description("Price of the pass");
        }
    }
}
