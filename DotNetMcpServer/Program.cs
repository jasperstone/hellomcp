using DotNetMcpServer.Tools;
using DotNetMcpServer.Models;

var builder = WebApplication.CreateBuilder(args);

// Add Swagger/OpenAPI (Swashbuckle)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SchemaFilter<DotNetMcpServer.Swagger.McpContextSchemaFilter>();
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

app.MapPost("/mcp/execute", async (McpContext context) =>
{
    var tool = tools.FirstOrDefault(t => t.Name == context.ToolName);
    if (tool == null)
        return Results.BadRequest(new { Error = "Tool not found" });

    var result = await tool.ExecuteAsync(context);
    return Results.Json(result);
});

app.Run();
