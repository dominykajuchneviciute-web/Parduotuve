using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using EShop.Api.DTOs.Chat;
using EShop.Api.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EShop.Api.Tests.Chat;

public class ChatMessagesTests(WebApplicationFactory<Program> factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task SendAndGetMessages_ReturnsSentMessage()
    {
        var conversationId = await AuthClient.CreateOtherUserConversationAsync(Factory.Services);

        var sendRes = await AuthClient.PostAsJsonAsync(
            $"/api/chat/conversations/{conversationId}/messages",
            new SendMessageRequest { Text = "Sveiki, ar dar turite?" });
        Assert.Equal(HttpStatusCode.OK, sendRes.StatusCode);

        var msgRes = await AuthClient.GetAsync($"/api/chat/conversations/{conversationId}/messages");
        Assert.Equal(HttpStatusCode.OK, msgRes.StatusCode);

        using var doc = await JsonDocument.ParseAsync(await msgRes.Content.ReadAsStreamAsync());
        var messages = doc.RootElement.EnumerateArray().ToList();
        Assert.Single(messages);
        Assert.Equal("Sveiki, ar dar turite?", messages[0].GetProperty("text").GetString());
    }
}