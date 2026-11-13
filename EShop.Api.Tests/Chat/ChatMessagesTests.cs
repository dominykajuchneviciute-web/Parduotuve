using System.Net;
using System.Net.Http.Json;
using EShop.Api.DTOs.Chat;
using EShop.Api.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EShop.Api.Tests.Chat;

public class ChatMessagesTests(WebApplicationFactory<Program> factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task SendAndGetMessages_ReturnsOk()
    {
        var conversationId = await AuthClient.CreateOtherUserConversationAsync(Factory.Services);

        var sendRes = await AuthClient.PostAsJsonAsync(
            $"/api/chat/conversations/{conversationId}/messages",
            new SendMessageRequest { Text = "Sveiki, ar dar turite?" });
        Assert.Equal(HttpStatusCode.OK, sendRes.StatusCode);

        var msgRes = await AuthClient.GetAsync($"/api/chat/conversations/{conversationId}/messages");
        Assert.Equal(HttpStatusCode.OK, msgRes.StatusCode);
    }
}
