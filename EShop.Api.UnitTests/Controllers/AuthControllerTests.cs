using System.Security.Claims;
using EShop.Api.Controllers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace EShop.Api.UnitTests.Controllers;

public class AuthControllerTests
{
    private static Mock<UserManager<IdentityUser>> CreateUserManagerMock()
    {
        var store = new Mock<IUserStore<IdentityUser>>();
        return new Mock<UserManager<IdentityUser>>(
            store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
    }

    private static Mock<SignInManager<IdentityUser>> CreateSignInManagerMock(UserManager<IdentityUser> userManager)
    {
        return new Mock<SignInManager<IdentityUser>>(
            userManager,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<IdentityUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<ILogger<SignInManager<IdentityUser>>>().Object,
            new Mock<IAuthenticationSchemeProvider>().Object,
            new Mock<IUserConfirmation<IdentityUser>>().Object);
    }

    [Fact]
    public async Task GetCurrentUser_UserFound_ReturnsOk()
    {
        var userManager = CreateUserManagerMock();
        var signInManager = CreateSignInManagerMock(userManager.Object);

        var identityUser = new IdentityUser { Id = "u-1", Email = "test@vu.lt" };
        userManager.Setup(m => m.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(identityUser);
        userManager.Setup(m => m.GetRolesAsync(identityUser)).ReturnsAsync(new List<string> { "User" });

        var controller = new AuthController(signInManager.Object, userManager.Object)
        {
            ControllerContext = TestHelpers.CreateControllerContext("u-1")
        };

        var result = await controller.GetCurrentUser();

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Logout_CallsSignOutAndReturnsOk()
    {
        var userManager = CreateUserManagerMock();
        var signInManager = CreateSignInManagerMock(userManager.Object);

        var controller = new AuthController(signInManager.Object, userManager.Object);

        var result = await controller.Logout();

        signInManager.Verify(s => s.SignOutAsync(), Times.Once);
        Assert.IsType<OkResult>(result);
    }
}