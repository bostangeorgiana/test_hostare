namespace CampusEats.UnitTest.Handlers.Auth.Register;

using Moq;
using Xunit;
using FluentAssertions;
using CampusEats.UnitTest.Helpers;
using CampusEats.Persistence.Repositories;
using CampusEats.Persistence.Entities;
using CampusEats.Features.Auth.Register;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

public class RegisterHandlerTests
{
    [Fact]
    public async System.Threading.Tasks.Task Given_InvalidModel_When_Handle_Then_ReturnsFailure()
    {
        var validator = new Mock<IValidator<RegisterCommand>>();
        var failures = new[] { new ValidationFailure("Email", "Invalid email") };
        validator.Setup(v => v.ValidateAsync(It.IsAny<RegisterCommand>(), It.IsAny<System.Threading.CancellationToken>()))
                 .ReturnsAsync(new ValidationResult(failures));

        var usersMock = new Mock<UserRepository>(ContextHelper.CreateInMemoryDbContext());
        var logger = new Mock<ILogger<RegisterHandler>>();

        var handler = new RegisterHandler(usersMock.Object, validator.Object, logger.Object);

        var command = new RegisterCommand("First","Last","bademail","pwd","pwd");

        var result = await handler.Handle(command, System.Threading.CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Invalid email");
    }

    [Fact]
    public async System.Threading.Tasks.Task Given_ValidCommand_When_Handle_Then_UserIsCreated_PendingRole()
    {
        using var db = ContextHelper.CreateInMemoryDbContext();
        var users = new UserRepository(db);

        var validator = new Mock<IValidator<RegisterCommand>>();
        validator.Setup(v => v.ValidateAsync(It.IsAny<RegisterCommand>(), It.IsAny<System.Threading.CancellationToken>()))
                 .ReturnsAsync(new ValidationResult());

        var logger = new Mock<ILogger<RegisterHandler>>();

        var handler = new RegisterHandler(users, validator.Object, logger.Object);

        var command = new RegisterCommand("John","Doe","john@example.com","password123","password123");

        var result = await handler.Handle(command, System.Threading.CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Email.Should().Be("john@example.com");
        result.Value.Role.Should().Be("Pending");

        var created = await db.Users.FirstOrDefaultAsync(u => u.Email == "john@example.com");
        created.Should().NotBeNull();
        created!.Role.Should().Be("Pending");
        created.Password.Should().NotBe("password123");
    }
}