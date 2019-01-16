using GraphQL.Types;
using Newtonsoft.Json.Linq;

namespace NextLevelBJJ.Api.GraphQLClasses
{
    public class GraphQLQuery : ObjectGraphType
    {
        public string OperationName { get; set; }
        public string NamedQuery { get; set; }
        public string Query { get; set; }
        public JObject Variables { get; set; }
    }
}
