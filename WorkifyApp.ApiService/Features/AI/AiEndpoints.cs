using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Agents.Chat;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using WorkifyApp.ApiService.Data;

namespace WorkifyApp.ApiService.Features.AI;

public static class AiEndpoints
{
    public static void MapAiEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/ai")
            .WithTags("AI")
            .WithOpenApi();

        group.MapPost("/chat", ChatAsync);
    }

    static async Task<Ok<string>> ChatAsync([FromBody] ChatRequest request, WorkifyDbContext db, Kernel kernel)
    {
        // 1. Prepare Kernel with Plugins
        kernel.ImportPluginFromObject(new WorkifyDataPlugin(db), "WorkifyData");

        // 2. Create Agents
        var analyst = WorkifyAgents.CreateAnalystAgent(kernel);
        var manager = WorkifyAgents.CreateReviewerAgent(kernel);

        // 3. Create Agent Group Chat
        // The flow is: User -> Analyst -> Manager -> Termination
        AgentGroupChat chat = new(analyst, manager)
        {
            ExecutionSettings = new()
            {
                // Stop after the manager speaks
                TerminationStrategy = new ApprovalTerminationStrategy()
                {
                    Agents = [manager],
                    MaximumIterations = 5
                }
            }
        };

        // 4. Run the chat
        chat.AddChatMessage(new ChatMessageContent(AuthorRole.User, request.Message));

        var lastResponse = string.Empty;
        await foreach (var content in chat.InvokeAsync())
        {
            if (content.AuthorName == manager.Name)
            {
                lastResponse = content.Content ?? "";
            }
        }

        return TypedResults.Ok(lastResponse);
    }
}

public class ApprovalTerminationStrategy : TerminationStrategy
{
    protected override Task<bool> ShouldAgentTerminateAsync(Agent agent, IReadOnlyList<ChatMessageContent> history, CancellationToken cancellationToken)
    {
        // Terminate immediately after the Manager (Reviewer) speaks
        return Task.FromResult(agent.Name == "Manager");
    }
}

public record ChatRequest(string Message);
