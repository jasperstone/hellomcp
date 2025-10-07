namespace DotNetMcpServer.Models
{
    public class McpResult
    {
        public bool Success { get; set; }
        public required object Data { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
