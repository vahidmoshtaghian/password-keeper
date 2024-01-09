using Application.GuardArea.Query;
using Domain.Base;
using Domain.Entities.Guard;

namespace UnitTests.Application.GuardArea.Query;

public class GetAllOrganizationsQueryHandlerTests : IClassFixture<ContextInitializer>
{
    private readonly CurrentUser _currentUser;
    private readonly ApplicationInMemoryDbContext _context;
    private readonly GetAllOrganizationsQueryHandler _handler;

    public GetAllOrganizationsQueryHandlerTests(ContextInitializer initializer)
    {
        _currentUser = initializer.CurrentUser;
        _context = new();
        _handler = new(_context, _currentUser);
    }

    [Fact]
    public async Task GetAll_WithoutSearch_ShouldReturnData()
    {
        // Arrange
        GetAllOrganizationsQuery query = new()
        {
            Page = 1
        };
        Organization organization1 = new()
        {
            Id = 1,
            Title = "Test 1",
            Memberships = new List<Membership>()
            {
                new()
                {
                    Id = 11,
                    IsOwner = true,
                    UserId = -1
                }
            }
        };
        Organization organization2 = new()
        {
            Id = 2,
            Title = "Test 2",
            Memberships = new List<Membership>()
            {
                new()
                {
                    Id = 22,
                    IsOwner = true,
                    UserId = _currentUser.Id
                }
            }
        };
        _context.AddRange(organization1, organization2);
        await _context.SaveChangesAsync();


        // Act
        var actual = await _handler.Handle(query, default);

        // Assert
        actual.Data.Count().Should().Be(1);
    }

    [Theory, FixtureData]
    public async Task GetAll_WithSearch_ShouldReturnData(GetAllOrganizationsQuery query)
    {
        // Arrange
        query.Page = 1;
        Organization organization1 = new()
        {
            Id = 1,
            Title = query.Search,
            Memberships = new List<Membership>()
            {
                new()
                {
                    Id = 11,
                    IsOwner = true,
                    UserId = _currentUser.Id
                }
            }
        };
        Organization organization2 = new()
        {
            Id = 2,
            Title = "Test",
            Memberships = new List<Membership>()
            {
                new()
                {
                    Id = 22,
                    IsOwner = true,
                    UserId = _currentUser.Id
                }
            }
        };
        _context.AddRange(organization1, organization2);
        await _context.SaveChangesAsync();

        // Act
        var actual = await _handler.Handle(query, default);

        // Assert
        actual.Data.Count().Should().Be(1);
        actual.Data.First().Id.Should().Be(organization1.Id);
        actual.Data.First().Title.Should().Be(organization1.Title);
    }
}
