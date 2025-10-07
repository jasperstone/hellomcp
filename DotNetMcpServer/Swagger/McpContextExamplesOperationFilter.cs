using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using DotNetMcpServer.Models;

namespace DotNetMcpServer.Swagger
{
    /// <summary>
    /// Adds multiple named examples for McpContext request bodies (one per tool).
    /// </summary>
    public class McpContextExamplesOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.RequestBody == null)
                return;

            if (!operation.RequestBody.Content.TryGetValue("application/json", out var mediaType))
                return;

            // Only add examples when the schema type is McpContext
            var schemaRef = mediaType.Schema?.Reference?.Id ?? mediaType.Schema?.Title;
            if (schemaRef != nameof(McpContext) && schemaRef != typeof(McpContext).FullName)
                return;

            mediaType.Examples = new Dictionary<string, OpenApiExample>
            {
                ["BuildSolution"] = new OpenApiExample
                {
                    Summary = "Build a solution",
                    Value = new OpenApiObject
                    {
                        ["toolName"] = new OpenApiString("BuildSolution"),
                        ["parameters"] = new OpenApiObject
                        {
                            ["solutionPath"] = new OpenApiString("/workspaces/hellomcp/DotNetMcpServer")
                        }
                    }
                },
                ["GetCoverageReport"] = new OpenApiExample
                {
                    Summary = "Fetch coverage report",
                    Value = new OpenApiObject
                    {
                        ["toolName"] = new OpenApiString("GetCoverageReport"),
                        ["parameters"] = new OpenApiObject
                        {
                            ["reportPath"] = new OpenApiString("/workspaces/hellomcp/DotNetMcpServer/coverage.xml")
                        }
                    }
                },
                ["RunTests"] = new OpenApiExample
                {
                    Summary = "Run tests in a solution",
                    Value = new OpenApiObject
                    {
                        ["toolName"] = new OpenApiString("RunTests"),
                        ["parameters"] = new OpenApiObject
                        {
                            ["solutionPath"] = new OpenApiString("/workspaces/hellomcp/DotNetMcpServer")
                        }
                    }
                },
                ["SuggestTests"] = new OpenApiExample
                {
                    Summary = "Suggest tests for a project",
                    Value = new OpenApiObject
                    {
                        ["toolName"] = new OpenApiString("SuggestTests"),
                        ["parameters"] = new OpenApiObject
                        {
                            ["projectPath"] = new OpenApiString("/workspaces/hellomcp/DotNetMcpServer")
                        }
                    }
                }
            };
        }
    }
}
