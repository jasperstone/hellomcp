namespace DotNetMcpServer.Models
{
    public class McpContext
    {
        public string ToolName { get; set; } = string.Empty;
        public Dictionary<string, object> Parameters { get; set; } = [];
    }
}
