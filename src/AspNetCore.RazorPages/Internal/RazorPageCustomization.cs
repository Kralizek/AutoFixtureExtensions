using AutoFixture;
using AutoFixture.Kernel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace Kralizek.AutoFixture.Extensions.Internal;

public class RazorPageCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Freeze<DefaultHttpContext>();

        fixture.Freeze<ModelStateDictionary>();

        fixture.Freeze<EmptyModelMetadataProvider>();

        fixture.Customize<ActionContext>(o => o.FromFactory((HttpContext httpContext, ModelStateDictionary modelState) => new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState)).OmitAutoProperties());

        fixture.Customize<ViewDataDictionary>(o => o.FromFactory((IModelMetadataProvider metadataProvider, ModelStateDictionary modelState) => new ViewDataDictionary(metadataProvider, modelState)).OmitAutoProperties());

        fixture.Customize<TempDataDictionary>(o => o.FromFactory((HttpContext httpContext, ITempDataProvider tempDataProvider) => new TempDataDictionary(httpContext, tempDataProvider)).OmitAutoProperties());

        fixture.Customize<PageContext>(o => o.FromFactory((ActionContext actionContext, ViewDataDictionary viewData) => new PageContext(actionContext) { ViewData = viewData }).OmitAutoProperties());

        fixture.Customize<UrlHelper>(o => o.FromFactory((ActionContext actionContext) => new UrlHelper(actionContext)));

        fixture.Customizations.Add(new TypeRelay(typeof(IModelMetadataProvider), typeof(EmptyModelMetadataProvider)));
        fixture.Customizations.Add(new TypeRelay(typeof(HttpContext), typeof(DefaultHttpContext)));
        fixture.Customizations.Add(new TypeRelay(typeof(ITempDataDictionary), typeof(TempDataDictionary)));
        fixture.Customizations.Add(new TypeRelay(typeof(IUrlHelper), typeof(UrlHelper)));
    }
}
