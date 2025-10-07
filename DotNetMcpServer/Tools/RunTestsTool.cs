using DotNetMcpServer.Models;
using System.Diagnostics;

namespace DotNetMcpServer.Tools
{
    public class RunTestsTool : IMcpTool
    {
        public string Name => "RunTests";

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
