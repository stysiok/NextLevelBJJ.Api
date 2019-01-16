using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextLevelBJJ.Api.GraphQLClasses
{
    public class NextLevelBJJSchema : Schema
    {
        public NextLevelBJJSchema(IDependencyResolver dependencyResolver) : base(dependencyResolver)
        {
            Query = dependencyResolver.Resolve<NextLevelBJJQuery>();
        }
    }
}
