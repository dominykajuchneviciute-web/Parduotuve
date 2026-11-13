using System.Net;
using System.Net.Http.Headers;
using System.Text;
using EShop.Api.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EShop.Api.Tests.Items;

public class UploadItemsTests(WebApplicationFactory<Program> factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task UploadItems_And_GetMyItems_ReturnsOk()
    {
        var json = """[{"itemType":"Clothing","name":"Ikeltas megztinis","condition":"New"}]""";
        using var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes(json));
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
        content.Add(fileContent, "file", "items.json");

        var uploadRes = await AuthClient.PostAsync("/api/items/upload", content);
        Assert.Equal(HttpStatusCode.OK, uploadRes.StatusCode);

        var mineRes = await AuthClient.GetAsync("/api/items/mine");
        Assert.Equal(HttpStatusCode.OK, mineRes.StatusCode);
    }
}
