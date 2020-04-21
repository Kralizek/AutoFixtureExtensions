using AutoFixture;
using System;
using AutoFixture.NUnit3;
using AutoFixture.AutoMoq;

namespace Tests
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CustomAutoDataAttribute : AutoDataAttribute
    {
        public CustomAutoDataAttribute() : base(CreateFixture)
        {

        }

        private static IFixture CreateFixture()
        {
            var fixture = new Fixture();

            fixture.Customize(new AutoMoqCustomization
            {
                GenerateDelegates = true,
                ConfigureMembers = true
            });

            return fixture;
        }
    }
}