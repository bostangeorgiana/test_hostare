namespace CampusEats.UnitTest.Handlers.Auth.Logout;

using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;
using FluentAssertions;
using CampusEats.Persistence.Repositories;
using CampusEats.Features.Auth.Logout;
using CampusEats.UnitTest.Helpers;

public class LogoutHandlerTests
{
    [Fact]
    public async System.Threading.Tasks.Task Given_AuthenticatedUser_When_Handle_Then_RevokesRefreshTokenAndDeletesCookie()
    {
        var userId = 5;
        using var db = ContextHelper.CreateInMemoryDbContext();
        db.Users.Add(new CampusEats.Persistence.Entities.User { UserId = userId, Email = "x@x.com", Password = "p", RefreshToken = "old", RefreshTokenExpiresAt = System.DateTime.UtcNow.AddDays(1) });
        await db.SaveChangesAsync();
        var users = new UserRepository(db);

        var httpContext = new DefaultHttpContext();
        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) }));
        var httpAccessorMock = new Mock<IHttpContextAccessor>();
        httpAccessorMock.SetupGet(h => h.HttpContext).Returns(httpContext);

        var handler = new LogoutHandler(users, httpAccessorMock.Object);
        var command = new LogoutCommand();

        var result = await handler.Handle(command, System.Threading.CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeTrue();

        var updated = await db.Users.FindAsync(userId);
        updated.Should().NotBeNull();
        updated.RefreshToken.Should().BeNull();
        updated.RefreshTokenExpiresAt.Should().Be(System.DateTime.MinValue);
        httpContext.Response.Headers.ContainsKey("Set-Cookie").Should().BeTrue();
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_NoUserClaim_When_Handle_Then_ReturnsFailureAndDoesNotCallRepo()
    {
        using var db = ContextHelper.CreateInMemoryDbContext();
        var seededUserId = 9;
        db.Users.Add(new CampusEats.Persistence.Entities.User { UserId = seededUserId, Email = "y@x.com", Password = "p", RefreshToken = "keep", RefreshTokenExpiresAt = System.DateTime.UtcNow.AddDays(1) });
        await db.SaveChangesAsync();

        var users = new UserRepository(db);

        var httpContext = new DefaultHttpContext();
        var httpAccessorMock = new Mock<IHttpContextAccessor>();
        httpAccessorMock.SetupGet(h => h.HttpContext).Returns(httpContext);

        var handler = new LogoutHandler(users, httpAccessorMock.Object);
        var command = new LogoutCommand();

        var result = await handler.Handle(command, System.Threading.CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("User not authenticated");

        var unchanged = await db.Users.FindAsync(seededUserId);
        unchanged.Should().NotBeNull();
        unchanged.RefreshToken.Should().Be("keep");
    }
}