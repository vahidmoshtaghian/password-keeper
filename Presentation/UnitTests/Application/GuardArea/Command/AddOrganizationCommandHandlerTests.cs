using Application.GuardArea.Command;
using Domain.Base;

namespace UnitTests.Application.GuardArea.Command;

public class AddOrganizationCommandHandlerTests : IClassFixture<ContextInitializer>
{
    private readonly CurrentUser _currentUser;
    private readonly ApplicationInMemoryDbContext _context;
    private readonly AddOrganizationCommandHandler _handler;

    public AddOrganizationCommandHandlerTests(ContextInitializer initializer)
    {
        _currentUser = initializer.CurrentUser;
        _context = new();
        _handler = new AddOrganizationCommandHandler(_context, _currentUser);
    }

    [Theory, FixtureData]
    public async Task AddOrganization_ShouldAdd(AddOrganizationCommand command)
    {
        // Arrange

        // Act
        var actual = await _handler.Handle(command, default);
        var expected = await _context.Organizations.FirstOrDefaultAsync(p => p.Id == actual.Id);

        // Assert

        expected.Should().NotBeNull();
        expected.Memberships.First().Id.Should().Be(_currentUser.Id);
        actual.Should().NotBeNull();
        actual.Title.Should().Be(actual.Title);
    }
}
