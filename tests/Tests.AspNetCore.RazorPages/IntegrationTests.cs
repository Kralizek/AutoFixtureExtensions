using AutoFixture;

namespace Tests;

[TestFixture]
public class IntegrationTests
{
    [Test, CustomAutoData]
    public void Razor_page_is_created(IFixture fixture)
    {
        fixture.AddRazorPages();

        var page = fixture.Create<IndexPage>();

        Assert.Multiple(() =>
        {
            Assert.That(page, Is.Not.Null);
            
            Assert.That(page.ModelState, Is.Not.Null);
            Assert.That(page.ViewData, Is.Not.Null);
            Assert.That(page.Url, Is.Not.Null);
            Assert.That(page.TempData, Is.Not.Null);
            Assert.That(page.RouteData, Is.Not.Null);
            Assert.That(page.PageContext, Is.Not.Null);
            Assert.That(page.HttpContext, Is.Not.Null);
            Assert.That(page.MetadataProvider, Is.Not.Null);
        });
    }
}