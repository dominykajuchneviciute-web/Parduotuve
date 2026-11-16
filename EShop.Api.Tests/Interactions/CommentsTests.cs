using System.Net;
using System.Net.Http.Json;
using EShop.Api.DTO;
using EShop.Api.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EShop.Api.Tests.Interactions;

public class CommentsTests(WebApplicationFactory<Program> factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task AddComment_And_DeleteComment_WorksCorrectly()
    {
        var itemId = await AuthClient.CreateTestItemAsync();

        var res = await AuthClient.PostAsJsonAsync(
            $"/api/items/{itemId}/comments",
            new CreateCommentRequest("Kaina galutine?"));
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);

        var comment = await res.Content.ReadFromJsonAsync<CommentResponse>();
        Assert.NotNull(comment);

        var delRes = await AuthClient.DeleteAsync($"/api/items/{itemId}/comments/{comment.Id}");
        Assert.Equal(HttpStatusCode.NoContent, delRes.StatusCode);

        var list = await Client.GetFromJsonAsync<List<CommentResponse>>($"/api/items/{itemId}/comments");
        Assert.NotNull(list);
        Assert.Empty(list);
    }
}
