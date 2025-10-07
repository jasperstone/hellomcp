using DotNetMcpServer.Models;
using System.Diagnostics;

namespace DotNetMcpServer.Tools
{
    public class RunTestsTool : IMcpTool
    {
        public string Name => "dotnet_test";

        public string Title => "Run Tests";

        public string Description => "Runs tests for a specified .NET test project.";

        public Dictionary<string, string> InputSchema => new()
        {
            { "testProjectPath", "string" }
        };

        public async Task<McpResult> ExecuteAsync(McpContext context)
        {
            var testProjectPath = context.Parameters["testProjectPath"].ToString();
            var process = Process.Start("dotnet", $"test {testProjectPath}");
            await process.WaitForExitAsync();

            return new McpResult
            {
                Success = process.ExitCode == 0,
                Data = new { process.ExitCode },
                Message = process.ExitCode == 0 ? "Tests passed" : "Tests failed"
            };
        }
    }
}
