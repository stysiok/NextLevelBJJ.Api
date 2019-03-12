using GraphQL;
using GraphQL.Types;

namespace NextLevelBJJ.Api.GraphQLClasses
{
    public class NextLevelBJJSchema : Schema
    {
        public NextLevelBJJSchema(IDependencyResolver dependencyResolver) : base(dependencyResolver)
        {
            Query = dependencyResolver.Resolve<NextLevelBJJQuery>();
            Mutation = dependencyResolver.Resolve<NextLevelBJJMutation>();
        }
    }
}
