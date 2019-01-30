using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using NextLevelBJJ.Api.GraphQLClasses;
using System.Threading.Tasks;

namespace NextLevelBJJ.Api.Controllers
{
    [Route("graphql")]
    public class GraphQLController : Controller
    {
        private readonly GraphQLQuery graphQLQuery;
        private readonly IDocumentExecuter documentExecuter;
        private readonly ISchema schema;

        public GraphQLController(GraphQLQuery GraphQLQuery, IDocumentExecuter DocumentExecuter, ISchema Schema)
        {
            graphQLQuery = GraphQLQuery;
            documentExecuter = DocumentExecuter;
            schema = Schema;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
        {
            var executionOptions = new ExecutionOptions
            {
                Schema = schema,
                Query = query.Query,
                Inputs = query.Variables.ToInputs(),
                ExposeExceptions = true
            };

            var result = await documentExecuter
                            .ExecuteAsync(executionOptions)
                            .ConfigureAwait(false);

            if (result.Errors?.Count > 0)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }
    }
}
