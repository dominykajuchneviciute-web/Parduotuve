using System.Net;
using System.Net.Http.Json;
using EShop.Api.DTO;
using EShop.Api.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EShop.Api.IntegrationTests.Interactions;

public class CommentsTests(WebApplicationFactory<Program> factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task AddComment_ValidText_ReturnsOkWithCreatedComment()
    {
        var itemId = await AuthClient.CreateTestItemAsync();

        var res = await AuthClient.PostAsJsonAsync(
            $"/api/items/{itemId}/comments",
            new CreateCommentRequest("Kaina galutine?"));

        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        var comment = await res.Content.ReadFromJsonAsync<CommentResponse>();
        Assert.NotNull(comment);
        Assert.Equal("Kaina galutine?", comment.Text);
    }

    [Fact]
    public async Task GetComments_AfterAddingComment_ReturnsCommentInList()
    {
        var itemId = await AuthClient.CreateTestItemAsync();
        await AuthClient.PostAsJsonAsync($"/api/items/{itemId}/comments", new CreateCommentRequest("Kaina galutine?"));

        var list = await Client.GetFromJsonAsync<List<CommentResponse>>($"/api/items/{itemId}/comments");

        Assert.NotNull(list);
        Assert.Single(list);
        Assert.Equal("Kaina galutine?", list[0].Text);
    }

    [Fact]
    public async Task DeleteComment_ByOwner_ReturnsNoContent()
    {
        var itemId = await AuthClient.CreateTestItemAsync();
        var addRes = await AuthClient.PostAsJsonAsync($"/api/items/{itemId}/comments", new CreateCommentRequest("Kaina galutine?"));
        var comment = await addRes.Content.ReadFromJsonAsync<CommentResponse>();

        var delRes = await AuthClient.DeleteAsync($"/api/items/{itemId}/comments/{comment!.Id}");

        Assert.Equal(HttpStatusCode.NoContent, delRes.StatusCode);
    }
}