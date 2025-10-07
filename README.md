# DotNetMcpServer (hellomcp)

A minimal Model Context Protocol (MCP) server implemented in .NET. The project exposes a small set of tools (BuildSolution, RunTests, GetCoverageReport, SuggestTests) and includes OpenAPI/Swagger documentation and example requests.

## Features

- REST endpoints for MCP-style interactions:
  - `POST /initialize` — exchange client/server info
  - `GET /tools/list` — list available tools
  - `GET /tools/get/{toolName}` — get metadata for a tool
  - `POST /tools/call` and `POST /tools/execute` — invoke a tool
- Swagger/OpenAPI UI (Swashbuckle)
- Example Schema and named examples for `McpContext` in Swagger
- `.http` requests for quick testing (`DotNetMcpServer/DotNetMcpServer.http`)
- Workspace `.vscode/mcp.json` that points to the local OpenAPI JSON

## Run with Dev Container
This repository is designed to run inside a VS Code devcontainer. If you have the Remote - Containers (or Dev Containers) extension:

1. Open the repository in VS Code.
2. Reopen in Container (Command Palette → Remote-Containers: Reopen in Container).
3. Once the container is built, run the server:

```bash
dotnet run --project DotNetMcpServer
```

The server will start and (if using the provided `Properties/launchSettings.json`) listen on `http://localhost:5244` (and possibly `https://localhost:7133`).

## Using `.vscode/mcp.json`
The workspace includes `.vscode/mcp.json` which contains a `servers` array with an entry for the local OpenAPI JSON:

```
{
  "servers": [
    {
      "name": "local-dotnet-mcp",
      "url": "http://127.0.0.1:5244/swagger/v1/swagger.json",
      "description": "Local MCP server (DotNetMcpServer)"
    }
  ]
}
```

Some Copilot Chat or MCP-aware tools will look for this file to auto-discover local MCP/OpenAPI servers. If your Copilot Chat instance runs in the same devcontainer, it can read this file and connect automatically. If Copilot Chat runs remotely, you must expose the local server (for example with `ngrok`) and update `mcp.json` with the public URL.

## Dependabot

This repository includes a Dependabot configuration (`.github/dependabot.yml`) which enables automated dependency update pull requests for configured package ecosystems (for example, NuGet). Dependabot will open PRs to keep dependencies up to date and can be configured to run on a schedule, ignore specific packages, or group updates.

See `.github/dependabot.yml` for the current policy and adjust settings there if you want different update behavior.

## View Swagger UI
After starting the server, open the Swagger UI in your browser:

- http://127.0.0.1:5244/swagger/index.html

The UI will show the `McpContext` schema and the named examples (BuildSolution, GetCoverageReport, RunTests, SuggestTests). You can "Try it out" from the UI.

## Run tests with DotNetMcpServer.http (REST Client)
This project includes a `DotNetMcpServer/DotNetMcpServer.http` file with example requests for:
- `GET /swagger/v1/swagger.json`
- `POST /initialize`
- `GET /tools/list`
- `GET /tools/get/BuildSolution`

To run these from VS Code, install the REST Client extension (`humao.rest-client`) and click "Send Request" above each request in the `.http` file. Alternatively, you can run the equivalent `curl` commands:

```bash
curl http://127.0.0.1:5244/mcp/list
curl -X POST http://127.0.0.1:5244/initialize -H 'Content-Type: application/json' -d '{"client":"my-client","capabilities":["tools","examples"]}'
curl http://127.0.0.1:5244/tools/list
curl http://127.0.0.1:5244/tools/get/BuildSolution
```

## Exposing to Copilot Chat running remotely
If Copilot Chat can't reach `http://127.0.0.1:5244` because it's remote, use a tunneling tool like `ngrok`:

```bash
ngrok http 5244
```

Then update `.vscode/mcp.json` with the public `https://...` URL that ngrok provides (append `/swagger/v1/swagger.json`).

## Development notes
- Swagger is provided via Swashbuckle (`Swashbuckle.AspNetCore`).
- Example schema and named examples are added in `DotNetMcpServer/Swagger`.
- Tools are in `DotNetMcpServer/Tools` and implement `IMcpTool`.

## Troubleshooting
- If the Swagger UI doesn't show examples, ensure the app runs in Development mode (`ASPNETCORE_ENVIRONMENT=Development`).
- If the REST Client in VS Code cannot reach the server, make sure the devcontainer port 5244 is forwarded or you are running the requests inside the container.

---
If you'd like, I can also add README sections with example Copilot Chat steps or pre-configured ngrok commands.

## References

- MCP SPECS: https://modelcontextprotocol.io/docs/learn/architecture — The official Model Context Protocol architecture and flow (init, tools, resources, prompts, calls).
- USING MCP IN VS CODE: https://code.visualstudio.com/docs/copilot/customization/mcp-servers — How to register and use MCP servers with Copilot and VS Code.
