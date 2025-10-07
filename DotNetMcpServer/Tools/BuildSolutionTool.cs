using DotNetMcpServer.Models;
using System.Diagnostics;

namespace DotNetMcpServer.Tools
{
    public class BuildSolutionTool : IMcpTool
    {
        public string Name => "dotnet_build";
        public string Title => "Build .NET Solution";
        public string Description => "Builds a .NET solution using the dotnet CLI.";

        public Dictionary<string, string> InputSchema => new()
        {
            { "solutionPath", "string" }
        };

        public async Task<McpResult> ExecuteAsync(McpContext context)
        {
            var solutionPath = context.Parameters["solutionPath"].ToString();
            var process = Process.Start("dotnet", $"build {solutionPath}");
            await process.WaitForExitAsync();

            return new McpResult
            {
                Success = process.ExitCode == 0,
                Data = new { process.ExitCode },
                Message = process.ExitCode == 0 ? "Build succeeded" : "Build failed"
            };
        }
    }
}
