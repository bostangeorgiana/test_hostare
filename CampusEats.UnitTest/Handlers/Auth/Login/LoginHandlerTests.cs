namespace CampusEats.UnitTest.Handlers.Auth.Login;

using System.Threading;
using Moq;
using Xunit;
using FluentAssertions;
using CampusEats.UnitTest.Helpers;
using CampusEats.Persistence.Entities;
using CampusEats.Persistence.Repositories;
using CampusEats.Services.JWT;
using CampusEats.Features.Auth.Login;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class LoginHandlerTests
{
    [Fact]
    public async System.Threading.Tasks.Task Given_InvalidModel_When_Handle_Then_ReturnsFailure()
    {
        var validator = new Mock<IValidator<LoginCommand>>();
        var failures = new[] { new ValidationFailure("Email", "Invalid") };
        validator.Setup(v => v.ValidateAsync(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new ValidationResult(failures));

        var jwt = new Mock<IJwtTokenService>();
        var logger = new Mock<ILogger<LoginHandler>>();
        var http = new Mock<IHttpContextAccessor>();

        var usersRepo = new Mock<UserRepository>(ContextHelper.CreateInMemoryDbContext());

        var handler = new LoginHandler(usersRepo.Object, jwt.Object, new JwtOptions { ExpiresMinutes = 60 }, validator.Object, logger.Object, http.Object);

        var command = new LoginCommand("a@b.com", "pwd");

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Invalid");
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_UserNotFound_When_Handle_Then_ReturnsFailure()
    {
        using var db = ContextHelper.CreateInMemoryDbContext();
        var users = new UserRepository(db);

        var validator = new Mock<IValidator<LoginCommand>>();
        validator.Setup(v => v.ValidateAsync(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new ValidationResult());

        var jwt = new Mock<IJwtTokenService>();
        var logger = new Mock<ILogger<LoginHandler>>();
        var http = new Mock<IHttpContextAccessor>();

        var handler = new LoginHandler(users, jwt.Object, new JwtOptions { ExpiresMinutes = 60 }, validator.Object, logger.Object, http.Object);

        var command = new LoginCommand("noone@x.com", "pwd");

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_WrongPassword_When_Handle_Then_ReturnsFailure()
    {
        using var db = ContextHelper.CreateInMemoryDbContext();
        var passwordHash = BCrypt.Net.BCrypt.HashPassword("correct");
        db.Users.Add(new User { UserId = 1, Email = "u@x.com", Password = passwordHash, FirstName = "F", LastName = "L", Role = "student" });
        await db.SaveChangesAsync();

        var users = new UserRepository(db);

        var validator = new Mock<IValidator<LoginCommand>>();
        validator.Setup(v => v.ValidateAsync(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new ValidationResult());

        var jwt = new Mock<IJwtTokenService>();
        var logger = new Mock<ILogger<LoginHandler>>();
        var http = new Mock<IHttpContextAccessor>();

        var handler = new LoginHandler(users, jwt.Object, new JwtOptions { ExpiresMinutes = 60 }, validator.Object, logger.Object, http.Object);

        var command = new LoginCommand("u@x.com", "wrong");

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_ValidCredentials_When_Handle_Then_ReturnsSuccess_And_RefreshTokenIsSet()
    {
        using var db = ContextHelper.CreateInMemoryDbContext();
        var plain = "secret";
        var hash = BCrypt.Net.BCrypt.HashPassword(plain);
        db.Users.Add(new User { UserId = 7, Email = "me@x.com", Password = hash, FirstName = "A", LastName = "B", Role = "student" });
        await db.SaveChangesAsync();

        var users = new UserRepository(db);

        var validator = new Mock<IValidator<LoginCommand>>();
        validator.Setup(v => v.ValidateAsync(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new ValidationResult());

        var jwt = new Mock<IJwtTokenService>();
        jwt.Setup(j => j.CreateAccessToken(7, "me@x.com", "student")).Returns("access-token");
        jwt.Setup(j => j.CreateRefreshToken()).Returns("refresh-token");

        var logger = new Mock<ILogger<LoginHandler>>();

        var httpContext = new DefaultHttpContext();
        var http = new Mock<IHttpContextAccessor>();
        http.SetupGet(h => h.HttpContext).Returns(httpContext);

        var handler = new LoginHandler(users, jwt.Object, new JwtOptions { ExpiresMinutes = 60 }, validator.Object, logger.Object, http.Object);

        var command = new LoginCommand("me@x.com", plain);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        Assert.NotNull(result.Value);
        result.Value.AccessToken.Should().Be("access-token");

        var updated = await db.Users.FindAsync(7);
        updated.Should().NotBeNull();
        updated.RefreshToken.Should().Be("refresh-token");
        updated.RefreshTokenExpiresAt.Should().NotBeNull();

        // cookie appended -> Set-Cookie header present
        httpContext.Response.Headers.ContainsKey("Set-Cookie").Should().BeTrue();
    }
}