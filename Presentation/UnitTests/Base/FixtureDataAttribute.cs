using AutoFixture;
using AutoFixture.Xunit2;

namespace BackendTests.Base;

internal class FixtureDataAttribute : AutoDataAttribute
{
    public FixtureDataAttribute(int count = 1) :
        base(() =>
            new Fixture
            {
                RepeatCount = count,
            }.Customize(new IgnoreVirtualMembersCustomisation())
        )
    {
    }
}

public class IgnoreVirtualMembersCustomisation : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }
}