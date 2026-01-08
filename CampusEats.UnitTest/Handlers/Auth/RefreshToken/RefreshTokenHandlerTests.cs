namespace CampusEats.UnitTest.Handlers.Auth.RefreshToken;

using System.Threading;
using Moq;
using Xunit;
using FluentAssertions;
using CampusEats.UnitTest.Helpers;
using CampusEats.Persistence.Entities;
using CampusEats.Persistence.Repositories;
using CampusEats.Services.JWT;
using CampusEats.Features.Auth.RefreshToken;
using Microsoft.AspNetCore.Http;

public class RefreshTokenHandlerTests
{
    [Fact]
    public async System.Threading.Tasks.Task Given_NoCookie_When_Handle_Then_ReturnsFailure()
    {
        using var db = ContextHelper.CreateInMemoryDbContext();
        var users = new UserRepository(db);

        var jwt = new Mock<IJwtTokenService>();
        var httpContext = new DefaultHttpContext();
        var httpAccessor = new Mock<IHttpContextAccessor>();
        httpAccessor.SetupGet(h => h.HttpContext).Returns(httpContext);

        var handler = new RefreshTokenHandler(users, jwt.Object, new JwtOptions { ExpiresMinutes = 60 }, httpAccessor.Object);

        var command = new RefreshTokenCommand();

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Refresh token missing");
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_InvalidCookie_When_Handle_Then_ReturnsFailure()
    {
        using var db = ContextHelper.CreateInMemoryDbContext();
        var users = new UserRepository(db);

        var jwt = new Mock<IJwtTokenService>();
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["Cookie"] = "refreshToken=invalid";
        var httpAccessor = new Mock<IHttpContextAccessor>();
        httpAccessor.SetupGet(h => h.HttpContext).Returns(httpContext);

        var handler = new RefreshTokenHandler(users, jwt.Object, new JwtOptions { ExpiresMinutes = 60 }, httpAccessor.Object);

        var command = new RefreshTokenCommand();

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Invalid or expired refresh token");
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_ValidCookie_When_Handle_Then_ReturnsNewTokens_And_UpdatesDb_And_SetsCookie()
    {
        using var db = ContextHelper.CreateInMemoryDbContext();
        var user = new User { UserId = 11, Email = "rt@x.com", Password = "p", RefreshToken = "old-token", RefreshTokenExpiresAt = System.DateTime.UtcNow.AddDays(1) };
        db.Users.Add(user);
        await db.SaveChangesAsync();

        var users = new UserRepository(db);

        var jwt = new Mock<IJwtTokenService>();
        jwt.Setup(j => j.CreateAccessToken(user.UserId, user.Email, user.Role)).Returns("new-access");
        jwt.Setup(j => j.CreateRefreshToken()).Returns("new-refresh");

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["Cookie"] = "refreshToken=old-token";
        httpContext.Response.Body = new System.IO.MemoryStream();
        var httpAccessor = new Mock<IHttpContextAccessor>();
        httpAccessor.SetupGet(h => h.HttpContext).Returns(httpContext);

        var handler = new RefreshTokenHandler(users, jwt.Object, new JwtOptions { ExpiresMinutes = 60 }, httpAccessor.Object);
        var command = new RefreshTokenCommand();

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        Assert.NotNull(result.Value);
        result.Value.AccessToken.Should().Be("new-access");

        var updated = await db.Users.FindAsync(user.UserId);
        updated.Should().NotBeNull();
        updated.RefreshToken.Should().Be("new-refresh");
        updated.RefreshTokenExpiresAt.Should().NotBeNull();

        httpContext.Response.Headers.ContainsKey("Set-Cookie").Should().BeTrue();
    }
}