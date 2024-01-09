using Application.GuardArea.Query;
using Domain.Base;
using Domain.Entities.Guard;
using Domain.Exceptions;

namespace UnitTests.Application.GuardArea.Query;

public class GetOrganizationByIdQueryHandlerTests : IClassFixture<ContextInitializer>
{
    private readonly CurrentUser _currentUser;
    private readonly ApplicationInMemoryDbContext _context;
    private readonly GetOrganizationByIdQueryHandler _handler;

    public GetOrganizationByIdQueryHandlerTests(ContextInitializer initializer)
    {
        _currentUser = initializer.CurrentUser;
        _context = new();
        _handler = new(_context, _currentUser);
    }

    [Theory, FixtureData]
    public async Task GetOrganization_ShouldGetData(GetOrganizationByIdQuery query)
    {
        // Arrange
        Organization expected = new()
        {
            Id = query.Id,
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
        _context.Add(expected);
        await _context.SaveChangesAsync();

        // Act
        var actual = await _handler.Handle(query, default);

        // Assert
        actual.Title.Should().Be(expected.Title);
    }

    [Theory, FixtureData]
    public async Task GetOrganization_WrongId_ShouldThrowError(GetOrganizationByIdQuery query)
    {
        // Arrange
        Organization expected = new()
        {
            Id = -1,
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
        _context.Add(expected);
        await _context.SaveChangesAsync();

        // Act
        var task = async () => await _handler.Handle(query, default);

        // Assert
        await task.Should().ThrowAsync<NotFoundException>();
    }
}
