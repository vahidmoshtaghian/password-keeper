using Application.GuardArea.Command;
using Domain.Base;
using Domain.Entities.Guard;
using Domain.Exceptions;
using Domain.Exceptions.GuardExceptions;

namespace UnitTests.Application.GuardArea.Command;

public class UpdateOrganizationCommandHandlerTests : IClassFixture<ContextInitializer>
{
    private readonly CurrentUser _currentUser;
    private readonly ApplicationInMemoryDbContext _context;
    private readonly UpdateOrganizationCommandHandler _handler;

    public UpdateOrganizationCommandHandlerTests(ContextInitializer initializer)
    {
        _currentUser = initializer.CurrentUser;
        _context = new();
        _handler = new(_context, _currentUser);
    }

    [Theory, FixtureData]
    public async Task UpdateOrganization_ShouldUpdate(UpdateOrganizationCommand expected, long id)
    {
        // Arrange
        expected.SetId(id);
        Membership membership = new()
        {
            OrganizationId = id,
            IsOwner = true,
            UserId = _currentUser.Id,
            Organization = new()
            {
                Id = id,
                Title = "Test"
            }
        };
        _context.Add(membership);
        await _context.SaveChangesAsync();

        // Act
        await _handler.Handle(expected, default);
        var actual = await _context.Organizations
            .FirstOrDefaultAsync(p => p.Id == id);

        // Assert
        actual.Should().NotBeNull();
        actual.Title.Should().Be(expected.Title);
    }

    [Theory, FixtureData]
    public async Task UpdateOrganization_WrongId_ShouldThrowError(UpdateOrganizationCommand command, long id)
    {
        // Assert

        // Act
        var task = async () => await _handler.Handle(command, default);

        // Assert
        await task.Should().ThrowAsync<NotFoundException>();
    }

    [Theory, FixtureData]
    public async Task UpdateOrganization_NotOwner_ShouldThrowError(UpdateOrganizationCommand command, long id)
    {
        // Arrange
        command.SetId(id);
        Membership membership = new()
        {
            OrganizationId = id,
            IsOwner = false,
            UserId = _currentUser.Id,
            Organization = new()
            {
                Id = id,
                Title = "Test"
            }
        };
        _context.Add(membership);
        await _context.SaveChangesAsync();

        // Act
        var task = async () => await _handler.Handle(command, default);

        // Assert
        await task.Should().ThrowAsync<OwnerException>();
    }
}
