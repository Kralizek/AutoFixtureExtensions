using System;
using AutoFixture.Kernel;

namespace Kralizek.AutoFixture.Extensions.Internal;

internal class DelegatingSpecimenCommand(Action<object?, ISpecimenContext> command) : ISpecimenCommand
{
    public void Execute(object specimen, ISpecimenContext context) => command(specimen, context);
}