using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using DotNetMcpServer.Models;

namespace DotNetMcpServer.Swagger
{
    /// <summary>
    /// Adds a concrete example for McpContext to the generated OpenAPI schema.
    /// </summary>
    public class McpContextSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(McpContext))
            {
                schema.Example = new OpenApiObject
                {
                    ["toolName"] = new OpenApiString("BuildSolution"),
                    ["parameters"] = new OpenApiObject
                    {
                        ["solutionPath"] = new OpenApiString("/workspaces/hellomcp/DotNetMcpServer")
                    }
                };
            }
        }
    }
}
