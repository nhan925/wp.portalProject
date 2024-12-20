using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Google;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.Extensions.AI;

namespace SpacePortal.Core.Services;
public class GeminiService
{
    private readonly string _germiniModelID;
    private readonly string _geminiApiKey;
    private Kernel kernel;
    private ChatHistory history = new();

    public GeminiService(string germiniModelID, string geminiApiKey)
    {
        this._germiniModelID = germiniModelID;
        this._geminiApiKey = geminiApiKey;

        #pragma warning disable SKEXP0070
        IKernelBuilder kernelBuilder = Kernel.CreateBuilder();
        kernelBuilder.AddGoogleAIGeminiChatCompletion(
            modelId: _germiniModelID,
            apiKey: _geminiApiKey,
            apiVersion: GoogleAIVersion.V1 // Optional
        );
        kernel = kernelBuilder.Build();
    }

    public IAsyncEnumerable<StreamingChatMessageContent> CallApiToChat(string message)
    {
        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
        history.AddUserMessage(message);

        var response = chatCompletionService.GetStreamingChatMessageContentsAsync(
            chatHistory: history,
            kernel: kernel
        );
        return response;
    }
}
