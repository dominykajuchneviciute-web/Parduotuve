using System.Net;
using System.Text.Json;
using EShop.Api.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EShop.Api.Tests.Chat;

public class ConversationTests(WebApplicationFactory<Program> factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task GetAndDeleteConversation_RemovesConversationFromList()
    {
        var conversationId = await AuthClient.CreateOtherUserConversationAsync(Factory.Services);

        var listRes = await AuthClient.GetAsync("/api/chat/conversations");
        Assert.Equal(HttpStatusCode.OK, listRes.StatusCode);
        using var docBefore = await JsonDocument.ParseAsync(await listRes.Content.ReadAsStreamAsync());
        Assert.Contains(docBefore.RootElement.EnumerateArray(), c => c.GetProperty("id").GetInt32() == conversationId);

        var delRes = await AuthClient.DeleteAsync($"/api/chat/conversations/{conversationId}");
        Assert.Equal(HttpStatusCode.OK, delRes.StatusCode);

        var listAfterRes = await AuthClient.GetAsync("/api/chat/conversations");
        using var docAfter = await JsonDocument.ParseAsync(await listAfterRes.Content.ReadAsStreamAsync());
        Assert.Empty(docAfter.RootElement.EnumerateArray());
    }
}