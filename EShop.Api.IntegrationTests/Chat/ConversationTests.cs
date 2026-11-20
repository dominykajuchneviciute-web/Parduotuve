using System.Net;
using System.Text.Json;
using EShop.Api.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EShop.Api.IntegrationTests.Chat;

public class ConversationTests(WebApplicationFactory<Program> factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task GetConversations_WhenConversationExists_ReturnsConversationInList()
    {
        var conversationId = await AuthClient.CreateOtherUserConversationAsync(Factory.Services);

        var listRes = await AuthClient.GetAsync("/api/chat/conversations");

        Assert.Equal(HttpStatusCode.OK, listRes.StatusCode);
        using var doc = await JsonDocument.ParseAsync(await listRes.Content.ReadAsStreamAsync());
        Assert.Contains(doc.RootElement.EnumerateArray(), c => c.GetProperty("id").GetInt32() == conversationId);
    }

    [Fact]
    public async Task DeleteConversation_ExistingConversation_HidesItFromUserList()
    {
        var conversationId = await AuthClient.CreateOtherUserConversationAsync(Factory.Services);

        var delRes = await AuthClient.DeleteAsync($"/api/chat/conversations/{conversationId}");

        Assert.Equal(HttpStatusCode.OK, delRes.StatusCode);
        var listAfterRes = await AuthClient.GetAsync("/api/chat/conversations");
        using var docAfter = await JsonDocument.ParseAsync(await listAfterRes.Content.ReadAsStreamAsync());
        Assert.Empty(docAfter.RootElement.EnumerateArray());
    }
}