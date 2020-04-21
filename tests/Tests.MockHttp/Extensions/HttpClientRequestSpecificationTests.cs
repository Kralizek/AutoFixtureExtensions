using System.Net.Http;
using AutoFixture.Extensions;
using NUnit.Framework;

namespace Tests.Extensions
{
    [TestFixture]
    public class HttpClientRequestSpecificationTests
    {
        [Test, CustomAutoData]
        public void IsSatisfiedBy_returns_true_if_request_is_HttpClient(HttpClientRequestSpecification sut)
        {
            var result = sut.IsSatisfiedBy(typeof(HttpClient));

            Assert.That(result, Is.True);
        }

        [Test, CustomAutoData]
        public void IsSatisfiedBy_returns_false_if_request_is_null(HttpClientRequestSpecification sut)
        {
            var result = sut.IsSatisfiedBy(null);

            Assert.That(result, Is.False);
        }

        [Test, CustomAutoData]
        public void IsSatisfiedBy_returns_false_if_request_is_not_HttpClient(HttpClientRequestSpecification sut)
        {
            var result = sut.IsSatisfiedBy(typeof(object));

            Assert.That(result, Is.False);
        }
    }
}