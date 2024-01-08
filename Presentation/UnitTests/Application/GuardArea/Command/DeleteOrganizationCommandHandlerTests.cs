using Application.GuardArea.Command;
using Domain.Base;
using Domain.Entities.Guard;
using Domain.Exceptions;
using Domain.Exceptions.GuardExceptions;

namespace UnitTests.Application.GuardArea.Command;

public class DeleteOrganizationCommandHandlerTests : IClassFixture<ContextInitializer>
{
    private readonly CurrentUser _currentUser;
    private readonly ApplicationInMemoryDbContext _context;
    private readonly DeleteOrganizationCommandHandler _handler;

    public DeleteOrganizationCommandHandlerTests(ContextInitializer initializer)
    {
        _currentUser = initializer.CurrentUser;
        _context = new();
        _handler = new(_context, _currentUser);
    }

    [Theory, FixtureData]
    public async Task DeleteOrganization_ShouldDelete(DeleteOrganizationCommand command)
    {
        // Arrange
        Membership membership = new()
        {
            Id = command.Id,
            OrganizationId = command.Id,
            IsOwner = true,
            UserId = _currentUser.Id,
            Organization = new()
        };
        _context.Add(membership);
        await _context.SaveChangesAsync();

        // Act
        await _handler.Handle(command, default);
        var expected = await _context.Organizations
            .FirstOrDefaultAsync(p => p.Id == command.Id);

        // Assert
        expected.Should().BeNull();
    }

    [Theory, FixtureData]
    public async Task DeleteOrganization_WrongId_ShouldThrowError(DeleteOrganizationCommand command)
    {
        // Arrange

        // Act
        var task = async () => await _handler.Handle(command, default);

        // Assert
        await task.Should().ThrowAsync<NotFoundException>();
    }

    [Theory, FixtureData]
    public async Task DeleteOrganization_NotOwner_ShouldThrowError(DeleteOrganizationCommand command)
    {
        // Arrange
        Membership membership = new()
        {
            Id = command.Id,
            OrganizationId = command.Id,
            IsOwner = false,
            UserId = _currentUser.Id,
            Organization = new()
        };
        _context.Add(membership);
        await _context.SaveChangesAsync();

        // Act
        var task = async () => await _handler.Handle(command, default);

        // Assert
        await task.Should().ThrowAsync<OwnerException>();
    }
}
