using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using Kralizek.AutoFixture.Extensions.Kernel;
using NUnit.Framework;

namespace Tests.Kernel
{
    [TestFixture]
    public class AsyncEnumerableRequestSpecificationTests
    {
        [Test, CustomAutoData]
        public void IsSatisfiedBy_returns_true_if_request_is_proper_type(AsyncEnumerableSpecimenBuilder.AsyncEnumerableRequestSpecification sut)
        {
            Assert.That(sut.IsSatisfiedBy(typeof(IAsyncEnumerable<int>)), Is.True);
        }

        [Test, CustomAutoData]
        public void IsSatisfiedBy_returns_false_if_request_is_null(AsyncEnumerableSpecimenBuilder.AsyncEnumerableRequestSpecification sut)
        {
            Assert.That(sut.IsSatisfiedBy(null!), Is.False);
        }

        [Test, CustomAutoData]
        public void IsSatisfiedBy_returns_false_if_request_is_not_proper_type(AsyncEnumerableSpecimenBuilder.AsyncEnumerableRequestSpecification sut)
        {
            Assert.That(sut.IsSatisfiedBy(typeof(object)), Is.False);
        }
    }

    [TestFixture]
    public class AsyncEnumerableSpecimenBuilderTests
    {
        [Test, CustomAutoData]
        public void Constructor_is_guarded(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(AsyncEnumerableSpecimenBuilder).GetConstructors());
        }

        [Test, CustomAutoData]
        public void RequestSpecification_exposes_passed_specification(IRequestSpecification specification)
        {
            var sut = new AsyncEnumerableSpecimenBuilder(specification);

            Assert.That(sut.RequestSpecification, Is.SameAs(specification));
        }

        [Test, CustomAutoData]
        public void Default_specification_is_AsyncEnumerableRequestSpecification()
        {
            var sut = new AsyncEnumerableSpecimenBuilder();

            Assert.That(sut.RequestSpecification, Is.InstanceOf<AsyncEnumerableSpecimenBuilder.AsyncEnumerableRequestSpecification>());
        }

        [Test, CustomAutoData]
        public void Create_is_guarded(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(AsyncEnumerableSpecimenBuilder).GetMethod(nameof(AsyncEnumerableSpecimenBuilder.Create)));
        }

        [Test, CustomAutoData]
        public void Create_returns_NoSpecimen_if_request_is_invalid(AsyncEnumerableSpecimenBuilder sut, SpecimenContext context)
        {
            var result = sut.Create(typeof(object), context);

            Assert.That(result, Is.InstanceOf<NoSpecimen>());
        }

        [Test, CustomAutoData]
        public void Create_returns_instance_of_type_if_request_is_valid(AsyncEnumerableSpecimenBuilder sut, SpecimenContext context)
        {
            var result = sut.Create(typeof(IAsyncEnumerable<int>), context);

            Assert.That(result, Is.InstanceOf<IAsyncEnumerable<int>>());
        }

        [Test, CustomAutoData]
        public async Task Create_returns_finite_sequence(AsyncEnumerableSpecimenBuilder sut, SpecimenContext context)
        {
            var result = sut.Create(typeof(IAsyncEnumerable<int>), context) as IAsyncEnumerable<int>;

            await foreach (var item in result!)
            {
                TestContext.WriteLine($"Item: {item}");
            }

            Assert.Pass();
        }
    }
}
