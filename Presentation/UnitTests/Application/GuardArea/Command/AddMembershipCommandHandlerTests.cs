using Application.GuardArea.Command;
using Domain.Base;
using Domain.Entities.Actor;
using Domain.Entities.Guard;
using Domain.Exceptions.GuardExceptions;

namespace UnitTests.Application.GuardArea.Command;

public class AddMembershipCommandHandlerTests : IClassFixture<ContextInitializer>
{
    private readonly ApplicationInMemoryDbContext _context;
    private readonly CurrentUser _currentUser;
    private readonly AddMembershipCommandHandler _handler;

    public AddMembershipCommandHandlerTests(ContextInitializer initializer)
    {
        _context = new();
        _currentUser = initializer.CurrentUser;
        _handler = new(_context, _currentUser);
    }

    [Theory, FixtureData]
    public async Task AddMembership_ShouldAdd(AddMembershipCommand command, User user, Friend friend, Organization organization)
    {
        // Arrange
        command.FriendId = friend.Id;
        User current = new()
        {
            Id = _currentUser.Id,
            FirstName = _currentUser.FirstName,
            LastName = _currentUser.LastName,
            Phone = "Test"
        };

        user.Status = Domain.Enums.UserStatus.Normal;
        user.Memberships = null;
        friend.User = user;
        friend.UserId = user.Id;
        friend.OwnerId = _currentUser.Id;
        friend.Owner = current;

        organization.Id = command.OrganizationId;
        organization.Memberships = new List<Membership>
        {
            new()
            {
                IsOwner = true,
                User = current,
                UserId = current.Id
            }
        };
        _context.AddRange(current, user);
        _context.Add(friend);
        _context.Add(organization);

        await _context.SaveChangesAsync();

        // Act
        await _handler.Handle(command, default);
        var actual = await _context.Memberships
            .SingleOrDefaultAsync(p => p.UserId == friend.UserId && p.OrganizationId == organization.Id);

        // Assert
        actual.Should().NotBeNull();
    }

    [Theory, FixtureData]
    public async Task AddMembership_AddingNonOwnerAMember_ShouldThrowError(AddMembershipCommand command, User user, Friend friend, Organization organization)
    {
        // Arrange
        command.FriendId = friend.Id;
        User current = new()
        {
            Id = _currentUser.Id,
            FirstName = _currentUser.FirstName,
            LastName = _currentUser.LastName,
            Phone = "Test"
        };

        user.Status = Domain.Enums.UserStatus.Normal;
        user.Memberships = null;
        friend.User = user;
        friend.UserId = user.Id;
        friend.OwnerId = _currentUser.Id;
        friend.Owner = current;

        organization.Id = command.OrganizationId;
        organization.Memberships = new List<Membership>
        {
            new()
            {
                IsOwner = false,
                User = current,
                UserId = current.Id
            }
        };
        _context.AddRange(current, user);
        _context.Add(friend);
        _context.Add(organization);

        await _context.SaveChangesAsync();

        // Act
        var task = async () => await _handler.Handle(command, default);

        // Assert
        await task.Should().ThrowAsync<OwnerException>();
    }

    [Theory, FixtureData]
    public async Task AddMembership_AddExixtsMember_ShouldThrowError(AddMembershipCommand command, User user, Friend friend, Organization organization)
    {
        // Arrange
        User current = new()
        {
            Id = _currentUser.Id,
            FirstName = _currentUser.FirstName,
            LastName = _currentUser.LastName,
            Phone = "Test"
        };

        user.Status = Domain.Enums.UserStatus.Normal;
        user.Memberships = null;
        friend.User = user;
        friend.UserId = user.Id;
        friend.OwnerId = _currentUser.Id;
        friend.User = current;
        organization.Id = command.OrganizationId;
        organization.Memberships = new List<Membership>
        {
            new()
            {
                IsOwner = true,
                UserId = current.Id
            }
        };
        command.FriendId = friend.Id;
        _context.AddRange(current, user);
        _context.Add(friend);
        _context.Add(organization);

        await _context.SaveChangesAsync();

        // Act
        var task = async () => await _handler.Handle(command, default);

        // Assert
        await task.Should().ThrowAsync<AlreadyMemberException>();
    }
}
