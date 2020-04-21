using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using NUnit.Framework;
using RichardSzalay.MockHttp;

namespace Tests.Integration
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TestAutoDataAttribute : AutoDataAttribute
    {
        public TestAutoDataAttribute() : base (() => new Fixture().AddMockHttp()) { }
    }

    [TestFixture]
    public class IntegrationTests
    {
        [Test]
        public async Task No_AutoData_should_work()
        {
            var fixture = new Fixture().AddMockHttp();

            var testUri = fixture.Create<Uri>();

            var expectedResult = fixture.Create<string>();

            var handler = fixture.Freeze<MockHttpMessageHandler>();

            handler.When(HttpMethod.Get, testUri.ToString()).Respond(HttpStatusCode.OK, new StringContent(expectedResult));

            var sut = fixture.Create<TestService>();

            var result = await sut.GetString(testUri);

            Assert.That(result, Is.EqualTo(expectedResult));            
        }

        [Test, AutoData]
        public async Task Basic_AutoData_should_work(Fixture fixture, Uri testUri, string expectedResult)
        {
            fixture.AddMockHttp();

            var handler = fixture.Freeze<MockHttpMessageHandler>();

            handler.When(HttpMethod.Get, testUri.ToString()).Respond(HttpStatusCode.OK, new StringContent(expectedResult));

            var sut = fixture.Create<TestService>();

            var result = await sut.GetString(testUri);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test, TestAutoData]
        public async Task Custom_AutoData_should_work([Frozen] MockHttpMessageHandler handler, TestService sut, Uri testUri, string expectedResult)
        {
            handler.When(HttpMethod.Get, testUri.ToString()).Respond(HttpStatusCode.OK, new StringContent(expectedResult));

            var result = await sut.GetString(testUri);

            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}