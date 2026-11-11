using System.Net;
using System.Net.Http.Json;
using EShop.Api.DTO;
using EShop.Api.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace EShop.Api.Tests.Items;

public class CreateItemValidationTests(WebApplicationFactory<Program> factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task CreateItem_WithEmptyName_ReturnsBadRequest()
    {
        var invalidRequest = new CreateItemRequest { Category = "Clothing", Name = "" };

        var response = await AuthClient.PostAsJsonAsync("/api/items", invalidRequest);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}