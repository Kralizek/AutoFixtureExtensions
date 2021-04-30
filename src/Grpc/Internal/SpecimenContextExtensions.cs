using System;
using AutoFixture.Kernel;

namespace Kralizek.AutoFixture.Extensions.Internal
{
    public static class SpecimenContextExtensions
    {
        public static bool TryResolve(this ISpecimenContext context, Type typeToResolve, out object? result)
        {
            result = context.Resolve(typeToResolve) switch
            {
                var x when typeToResolve.IsInstanceOfType(x) => x,
                _ => null
            };

            return result is not null;
        }
    }
}
