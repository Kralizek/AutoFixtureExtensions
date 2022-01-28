using Kralizek.AutoFixture.Extensions.Kernel;

// ReSharper disable CheckNamespace

namespace AutoFixture
{
    public class AsyncEnumerableCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new AsyncEnumerableSpecimenBuilder());
        }
    }
}