using System.Net;
using EShop.Api.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EShop.Api.Tests.Chat;

public class ConversationTests(WebApplicationFactory<Program> factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task GetAndDeleteConversation_ReturnsOk()
    {
        var conversationId = await AuthClient.CreateOtherUserConversationAsync(Factory.Services);

        var listRes = await AuthClient.GetAsync("/api/chat/conversations");
        Assert.Equal(HttpStatusCode.OK, listRes.StatusCode);

        var delRes = await AuthClient.DeleteAsync($"/api/chat/conversations/{conversationId}");
        Assert.Equal(HttpStatusCode.OK, delRes.StatusCode);
    }
}
