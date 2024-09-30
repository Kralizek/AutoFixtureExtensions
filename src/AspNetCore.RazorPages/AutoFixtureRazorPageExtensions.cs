using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kralizek.AutoFixture.Extensions.Internal;

namespace AutoFixture
{
    public static class AutoFixtureRazorPageExtensions
    {
        public static IFixture AddRazorPages(this IFixture fixture)
        {
            fixture.Customize(new RazorPageCustomization());

            return fixture;
        }
    }
}