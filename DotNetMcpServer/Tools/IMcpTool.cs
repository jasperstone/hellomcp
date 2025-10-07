using DotNetMcpServer.Models;

namespace DotNetMcpServer.Tools
{
    public interface IMcpTool
    {
        string Name { get; }
        string Title { get; }
        string Description { get; }
        Dictionary<string, string> InputSchema { get; }
        Task<McpResult> ExecuteAsync(McpContext context);
    }
}
