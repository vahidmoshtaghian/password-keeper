using Application.GuardArea.Query;
using Domain.Base;
using Domain.Entities.Actor;
using Domain.Entities.Guard;

namespace UnitTests.Application.GuardArea.Query;

public class GetOrganizationMembershipsQueryHandlerTests : IClassFixture<ContextInitializer>
{
    private readonly CurrentUser _currentUser;
    private readonly ApplicationInMemoryDbContext _context;
    private readonly GetOrganizationMembershipsQueryHandler _handler;

    public GetOrganizationMembershipsQueryHandlerTests(ContextInitializer initializer)
    {
        _currentUser = initializer.CurrentUser;
        _context = new();
        _handler = new(_currentUser, _context);
    }

    [Theory, FixtureData]
    public async Task GetMemberships_ShouldReturnData(GetOrganizationMembershipsQuery query)
    {
        // Arrange
        User expected2 = new()
        {
            Id = 2,
            FirstName = "Test 2 FirstName",
            LastName = "Test 2 LastName",
            Phone = "Test 2 Phone",
        };
        User expected1 = new()
        {
            Id = _currentUser.Id,
            FirstName = "Test 1 FirstName",
            LastName = "Test 1 LastName",
            Phone = "Test 1 Phone",
            Friends = new List<Friend>
            {
                new()
                {
                    FirstName = "Test Friend FirstName",
                    LastName = "Test Friend LastName",
                    User = expected2
                }
            }
        };
        Organization organization = new()
        {
            Id = query.Id,
            Title = "Test 2",
            Memberships = new List<Membership>()
            {
                new()
                {
                    Id = 22,
                    IsOwner = true,
                    UserId = expected1.Id,
                    User = expected1
                },
                new()
                {
                    Id = 23,
                    IsOwner = false,
                    UserId = expected2.Id,
                    User = expected2
                },
            }
        };
        _context.Add(organization);
        await _context.SaveChangesAsync();

        // Act
        var actual = await _handler.Handle(query, default);

        // Assert
        actual.Count().Should().Be(2);
    }

    [Theory, FixtureData]
    public async Task GetMemberships_NoFriendUsers_ShouldProtectId(GetOrganizationMembershipsQuery query)
    {
        // Arrange
        User expected = new()
        {
            Id = 2,
            FirstName = "Test 2 FirstName",
            LastName = "Test 2 LastName",
            Phone = "Test 2 Phone",
        };
        User owner = new()
        {
            Id = _currentUser.Id,
            FirstName = "Test 1 FirstName",
            LastName = "Test 1 LastName",
            Phone = "Test 1 Phone",

        };
        Organization organization = new()
        {
            Id = query.Id,
            Title = "Test 2",
            Memberships = new List<Membership>()
            {
                new()
                {
                    Id = 22,
                    IsOwner = true,
                    UserId = owner.Id,
                    User = owner
                },
                new()
                {
                    Id = 23,
                    IsOwner = false,
                    UserId = expected.Id,
                    User = expected
                },
            }
        };
        _context.Add(organization);
        await _context.SaveChangesAsync();

        // Act
        var actual = await _handler.Handle(query, default);

        // Assert
        actual.First(p => p.FullName != owner.FullName).Id.Should().Be(0);
        actual.First(p => p.FullName != owner.LastName).FullName.Should().Be(expected.LastName);
    }
}
