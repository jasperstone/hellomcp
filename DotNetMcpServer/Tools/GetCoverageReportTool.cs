using DotNetMcpServer.Models;
using System.Xml.Linq;

namespace DotNetMcpServer.Tools
{
    public class GetCoverageReportTool : IMcpTool
    {
        public string Name => "report_coverage";

        public string Title => "Get Coverage Report";

        public string Description => "Retrieves code coverage report from a specified file.";

        public Dictionary<string, string> InputSchema => new()
        {
            { "reportPath", "string" }
        };

        public async Task<McpResult> ExecuteAsync(McpContext context)
        {
            return await Task.Run(() =>
            {
                var reportPathObj = context.Parameters["reportPath"] ?? throw new InvalidOperationException("The 'reportPath' entry in context.Parameters is missing or empty.");
                var reportPath = reportPathObj.ToString() ?? throw new InvalidOperationException("The 'reportPath' entry in context.Parameters is missing or empty.");
                var doc = XDocument.Load(reportPath);
                var coveragePercent = doc.Descendants("coverage").First().Attribute("line-rate")?.Value ?? "0";

                return new McpResult
                {
                    Success = true,
                    Data = new { CoveragePercent = coveragePercent },
                    Message = "Coverage report fetched"
                };
            });
        }
    }
}
