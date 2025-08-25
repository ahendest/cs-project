using cs_project.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using Moq;
using Xunit;

namespace cs_project.Tests.Controllers;

public class AccountControllerTests
{
    private static IConfiguration BuildConfig() => new ConfigurationBuilder()
        .AddInMemoryCollection(new Dictionary<string, string?>
        {
            {"Jwt:Key", "01234567890123456789012345678901"},
            {"Jwt:Issuer", "issuer"},
            {"Jwt:Audience", "audience"}
        })
        .Build();

    private static Mock<UserManager<IdentityUser>> MockUserManager()
    {
        var store = new Mock<IUserStore<IdentityUser>>();
        return new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
    }

    private static Mock<SignInManager<IdentityUser>> MockSignInManager(UserManager<IdentityUser> um)
    {
        return new Mock<SignInManager<IdentityUser>>(um, Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<IdentityUser>>(), null, null, null, null);
    }

    [Fact]
    public async Task Register_ReturnsOk_OnSuccess()
    {
        var um = MockUserManager();
        var sm = MockSignInManager(um.Object);
        um.Setup(u => u.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
        var controller = new AccountController(um.Object, sm.Object, BuildConfig());

        var result = await controller.Register(new AccountController.RegisterDTO { UserName = "u", Email = "e", Password = "p" });

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Register_ReturnsBadRequest_OnFailure()
    {
        var um = MockUserManager();
        var sm = MockSignInManager(um.Object);
        um.Setup(u => u.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());
        var controller = new AccountController(um.Object, sm.Object, BuildConfig());

        var result = await controller.Register(new AccountController.RegisterDTO { UserName = "u", Email = "e", Password = "p" });

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Login_ReturnsOk_OnSuccess()
    {
        var um = MockUserManager();
        var user = new IdentityUser { UserName = "u", Id = "1" };
        um.Setup(u => u.FindByNameAsync("u")).ReturnsAsync(user);
        var sm = MockSignInManager(um.Object);
        sm.Setup(s => s.PasswordSignInAsync(user, "p", false, true)).ReturnsAsync(SignInResult.Success);
        var controller = new AccountController(um.Object, sm.Object, BuildConfig());

        var result = await controller.Login(new AccountController.LoginDTO { UserName = "u", Password = "p" });

        var ok = Assert.IsType<OkObjectResult>(result);
        var token = ok.Value?.GetType().GetProperty("token")?.GetValue(ok.Value);
        Assert.NotNull(token);
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_OnInvalid()
    {
        var um = MockUserManager();
        var user = new IdentityUser { UserName = "u" };
        um.Setup(u => u.FindByNameAsync("u")).ReturnsAsync(user);
        var sm = MockSignInManager(um.Object);
        sm.Setup(s => s.PasswordSignInAsync(user, "p", false, true)).ReturnsAsync(SignInResult.Failed);
        var controller = new AccountController(um.Object, sm.Object, BuildConfig());

        var result = await controller.Login(new AccountController.LoginDTO { UserName = "u", Password = "p" });

        Assert.IsType<UnauthorizedObjectResult>(result);
    }
}
