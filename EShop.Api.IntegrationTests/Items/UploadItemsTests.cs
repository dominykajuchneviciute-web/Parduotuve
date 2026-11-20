using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using EShop.Api.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EShop.Api.IntegrationTests.Items;

public class UploadItemsTests(WebApplicationFactory<Program> factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task UploadItems_ValidJsonFile_ReturnsOkWithUploadedItems()
    {
        var json = """[{"itemType":"Clothing","name":"Ikeltas megztinis","condition":0}]""";
        using var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes(json));
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
        content.Add(fileContent, "file", "items.json");

        var uploadRes = await AuthClient.PostAsync("/api/items/upload", content);

        Assert.Equal(HttpStatusCode.OK, uploadRes.StatusCode);
        using var doc = await JsonDocument.ParseAsync(await uploadRes.Content.ReadAsStreamAsync());
        var items = doc.RootElement.EnumerateArray().ToList();
        Assert.Single(items);
        Assert.Equal("Ikeltas megztinis", items[0].GetProperty("name").GetString());
    }

    [Fact]
    public async Task GetMyItems_AfterUploadingFile_ReturnsUploadedItemForOwner()
    {
        var json = """[{"itemType":"Clothing","name":"Ikeltas megztinis","condition":0}]""";
        using var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes(json));
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
        content.Add(fileContent, "file", "items.json");
        await AuthClient.PostAsync("/api/items/upload", content);

        var mineRes = await AuthClient.GetAsync("/api/items/mine");

        Assert.Equal(HttpStatusCode.OK, mineRes.StatusCode);
        using var doc = await JsonDocument.ParseAsync(await mineRes.Content.ReadAsStreamAsync());
        var items = doc.RootElement.EnumerateArray().ToList();
        Assert.Single(items);
        Assert.Equal("Ikeltas megztinis", items[0].GetProperty("name").GetString());
    }
}