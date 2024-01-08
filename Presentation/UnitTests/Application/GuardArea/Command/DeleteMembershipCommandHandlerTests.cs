using Application.GuardArea.Command;
using Domain.Base;
using Domain.Entities.Actor;
using Domain.Entities.Guard;
using Domain.Exceptions;
using Domain.Exceptions.GuardExceptions;

namespace UnitTests.Application.GuardArea.Command;

public class DeleteMembershipCommandHandlerTests : IClassFixture<ContextInitializer>
{
    private readonly CurrentUser _currentUser;
    private readonly ApplicationInMemoryDbContext _context;
    private readonly DeleteMembershipCommandHandler _handler;

    public DeleteMembershipCommandHandlerTests(ContextInitializer initializer)
    {
        _currentUser = initializer.CurrentUser;
        _context = new();
        _handler = new DeleteMembershipCommandHandler(_currentUser, _context);
    }

    [Theory, FixtureData]
    public async Task DeleteMembership_ShouldDelete(DeleteMembershipCommand command, Friend friend)
    {
        // Arrange
        friend.Id = command.FriendId;
        friend.OwnerId = _currentUser.Id;
        friend.Owner = null;
        friend.User.Id = friend.UserId;
        friend.User.Memberships = null;
        friend.User.Friends = null;

        Organization organization = new()
        {
            Id = command.OrganizationId,
            Title = "Test",
            Memberships = new List<Membership>
            {
                new()
                {
                    IsOwner = true,
                    UserId = _currentUser.Id
                }
            }
        };
        Membership expected = new()
        {
            IsOwner = false,
            UserId = friend.UserId,
            OrganizationId = command.OrganizationId
        };
        _context.AddRange(friend, organization, expected);

        await _context.SaveChangesAsync();

        // Act
        await _handler.Handle(command, default);
        var actual = await _context.Organizations
            .Include(p => p.Memberships)
            .FirstAsync(p => p.Id == organization.Id);

        // Assert
        actual.Memberships.Should().NotContain(p => p.Id == expected.Id);
    }

    [Theory, FixtureData]
    public async Task DeleteMembership_WrongMember_ShouldThrowError(DeleteMembershipCommand command, Friend friend)
    {
        // Arrange
        friend.Id = command.FriendId;
        friend.OwnerId = _currentUser.Id;
        friend.Owner = null;
        friend.User.Id = friend.UserId;
        friend.User.Memberships = null;
        friend.User.Friends = null;

        Organization organization = new()
        {
            Id = command.OrganizationId,
            Title = "Test",
            Memberships = new List<Membership>
            {
                new()
                {
                    IsOwner = false,
                    UserId = _currentUser.Id
                }
            }
        };
        Membership expected = new()
        {
            IsOwner = false,
            UserId = -1,
            OrganizationId = command.OrganizationId
        };
        _context.AddRange(friend, organization, expected);

        await _context.SaveChangesAsync();

        // Act
        var task = async () => await _handler.Handle(command, default);

        // Assert
        await task.Should().ThrowAsync<NotFoundException>();
    }

    [Theory, FixtureData]
    public async Task DeleteMembership_NotOwner_ShouldThrowError(DeleteMembershipCommand command, Friend friend)
    {
        // Arrange
        friend.Id = command.FriendId;
        friend.OwnerId = _currentUser.Id;
        friend.Owner = null;
        friend.User.Id = friend.UserId;
        friend.User.Memberships = null;
        friend.User.Friends = null;

        Organization organization = new()
        {
            Id = command.OrganizationId,
            Title = "Test",
            Memberships = new List<Membership>
            {
                new()
                {
                    IsOwner = false,
                    UserId = _currentUser.Id
                }
            }
        };
        Membership expected = new()
        {
            IsOwner = false,
            UserId = friend.UserId,
            OrganizationId = command.OrganizationId
        };
        _context.AddRange(friend, organization, expected);

        await _context.SaveChangesAsync();

        // Act
        var task = async () => await _handler.Handle(command, default);

        // Assert
        await task.Should().ThrowAsync<OwnerException>();
    }

    [Theory, FixtureData]
    public async Task DeleteMembership_WrongUser_ShouldThrowError(DeleteMembershipCommand command)
    {
        // Arrange

        // Act
        var task = async () => await _handler.Handle(command, default);

        // Assert
        await task.Should().ThrowAsync<NotFoundException>();
    }
}
