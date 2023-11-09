using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using TestWebSite;

namespace Tests.Integration
{
    [TestFixture]
    public class InstanceStandardWebFactoryIntegrationTests
    {
        [AttributeUsage(AttributeTargets.Method)]
        public class TestAutoDataAttribute : AutoDataAttribute
        {
            public TestAutoDataAttribute() : base(() => new Fixture().AddWebApplicationFactorySupport(new WebApplicationFactory<Startup>())) { }
        }

        [Test]
        public async Task Should_work_with_no_auto_data_attribute()
        {
            var instance = new WebApplicationFactory<Startup>();
            
            var fixture = new Fixture().AddWebApplicationFactorySupport(instance);

            var message = fixture.Create<string>();

            var http = fixture.Create<HttpClient>();

            var response = await http.PostAsync("/", new FormUrlEncodedContent(new Dictionary<string, string> { ["message"] = message }));

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.That(responseContent, Is.EqualTo(message));
        }

        [Test, AutoData]
        public async Task Should_work_with_basic_auto_data_attribute(IFixture fixture, string message)
        {
            var instance = new WebApplicationFactory<Startup>();
            
            fixture.AddWebApplicationFactorySupport(instance);

            var http = fixture.Create<HttpClient>();

            var response = await http.PostAsync("/", new FormUrlEncodedContent(new Dictionary<string, string> { ["message"] = message }));

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.That(responseContent, Is.EqualTo(message));
        }

        [Test, TestAutoData]
        public async Task Should_work_with_custom_auto_data_attribute(HttpClient http, string message)
        {
            var response = await http.PostAsync("/", new FormUrlEncodedContent(new Dictionary<string, string> { ["message"] = message }));

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.That(responseContent, Is.EqualTo(message));
        }
    }
}
