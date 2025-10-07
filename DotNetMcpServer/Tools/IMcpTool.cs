using DotNetMcpServer.Models;

namespace DotNetMcpServer.Tools
{
    public interface IMcpTool
    {
        string Name { get; }
        Task<McpResult> ExecuteAsync(McpContext context);
    }
}
