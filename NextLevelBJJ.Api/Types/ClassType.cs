using GraphQL.Types;
using NextLevelBJJ.Api.DTO;

namespace NextLevelBJJ.Api.Types
{
    public class ClassType : ObjectGraphType<ClassDto>
    {
        public ClassType()
        {
            Name = "Class";
            Description = "Class in the academy";
            Field(c => c.Day, type: typeof(StringGraphType)).Description("Day on which the class takes place");
            Field(c => c.IsKidsClass).Description("Indicator if classes are for children only");
            Field(c => c.Name).Description("Name of the class");
            Field(c => c.StartHour).Description("Which hour the class starts");
            Field(c => c.FinishHour).Description("Which hour the class ends");
        }
    }
}