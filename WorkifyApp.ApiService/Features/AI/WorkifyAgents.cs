using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace WorkifyApp.ApiService.Features.AI;

public static class WorkifyAgents
{
    public static ChatCompletionAgent CreateAnalystAgent(Kernel kernel)
    {
        return new ChatCompletionAgent
        {
            Name = "DataAnalyst",
            Instructions = 
                """
                You are a Data Analyst for Workify.
                Your goal is to query the database using the provided tools to answer the user's questions strictly based on data.
                If you cannot find the data, say so. Do not make up facts.
                Focus on numbers, dates, and financial figures.
                """,
            Kernel = kernel,
            Arguments = new KernelArguments(new OpenAIPromptExecutionSettings { ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions })
        };
    }

    public static ChatCompletionAgent CreateReviewerAgent(Kernel kernel)
    {
        return new ChatCompletionAgent
        {
            Name = "Manager",
            Instructions =
                """
                You are the Manager.
                You review the Data Analyst's output and summarize it for the Executive Team.
                Make it sound professional, confident, and actionable. Add a 'Manager's Note' at the end.
                """,
            Kernel = kernel
        };
    }
}
