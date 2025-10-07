using DotNetMcpServer.Tools;
using DotNetMcpServer.Models;

var builder = WebApplication.CreateBuilder(args);

// Add Swagger/OpenAPI (Swashbuckle)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SchemaFilter<DotNetMcpServer.Swagger.McpContextSchemaFilter>();
    options.OperationFilter<DotNetMcpServer.Swagger.McpContextExamplesOperationFilter>();
});

var app = builder.Build();

// Register MCP tools
var tools = new List<IMcpTool>
{
    new BuildSolutionTool(),
    new RunTestsTool(),
    new GetCoverageReportTool(),
    new SuggestTestsTool()
};

app.UseSwagger();
app.UseSwaggerUI();

// Prevent 404 on a GET / probe from clients/extensions by providing a simple root route.
// Redirect to the Swagger UI
app.MapGet("/", () => Results.Redirect("/swagger"));

// MCP initialization endpoint — exchange client/server info
app.MapPost("/initialize", (Dictionary<string, object> clientInfo) =>
{
    var serverInfo = new
    {
        name = "DotNetMcpServer",
        version = "0.1",
        tools = tools.Select(t => t.Name).ToArray()
    };

    return Results.Json(new { server = serverInfo, client = clientInfo });
});

// Aliases to match alternate MCP-style paths
app.MapGet("/tools/list", () =>
{
    var toolList = tools.Select(t => new { name = t.Name, title = t.Title, description = t.Description }).ToArray();
    return Results.Json(toolList);
});

app.MapGet("/tools/get/{toolName}", (string toolName) =>
{
    var tool = tools.FirstOrDefault(t => t.Name == toolName || t.Name.Equals(toolName, StringComparison.OrdinalIgnoreCase));
    if (tool == null)
        return Results.NotFound(new { Error = "Tool not found" });

    var meta = new { name = tool.Name, title = tool.Title, description = tool.Description, inputSchema = tool.InputSchema };
    return Results.Json(meta);
});

// MCP call endpoint (use ExecuteAsync under the hood)
app.MapPost("/tools/call", async (McpContext context) =>
{
    var tool = tools.FirstOrDefault(t => t.Name == context.ToolName);
    if (tool == null)
        return Results.BadRequest(new { Error = "Tool not found" });

    var result = await tool.ExecuteAsync(context);
    return Results.Json(result);
});

// resources and prompts endpoints (placeholders)
app.MapGet("/resources/list", () => Results.Json(Array.Empty<object>()));
app.MapGet("/prompts/list", () => Results.Json(Array.Empty<object>()));

app.Run();
