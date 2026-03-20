using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;

namespace SSD2600_CDEGP.TagHelpers.PaButton;

[HtmlTargetElement("PaButton")]
public class PaButtonTagHelper : ViewTagHelper
{
    public enum Themes
    {
        None,
        Danger,
        Primary,
    }

    public enum Styles
    {
        Fill,
        Outline,
    }

    public enum Sizes
    {
        Base,
        Small,
    }

    [HtmlAttributeName]
    public Themes? Theme { get; set; } = Themes.None;

    [HtmlAttributeName]
    public Styles? Style { get; set; } = Styles.Fill;

    [HtmlAttributeName]
    public Sizes? Size { get; set; } = Sizes.Base;

    [HtmlAttributeName]
    public string Classes { get; set; } = string.Empty;

    [HtmlAttributeName]
    public string Attributes { get; set; } = string.Empty;

    [HtmlAttributeName("tag")]
    public string TagName { get; set; } = "a";

    [HtmlAttributeName("asp-area")]
    public string? AspArea { get; set; }

    [HtmlAttributeName("asp-controller")]
    public string? AspController { get; set; }

    [HtmlAttributeName("asp-action")]
    public string? AspAction { get; set; }

    [HtmlAttributeName("asp-page")]
    public string? AspPage { get; set; }

    [HtmlAttributeName("asp-page-handler")]
    public string? AspPageHandler { get; set; }

    [HtmlAttributeName("asp-route")]
    public string? AspRoute { get; set; }

    [HtmlAttributeName("asp-protocol")]
    public string? AspProtocol { get; set; }

    [HtmlAttributeName("asp-host")]
    public string? AspHost { get; set; }

    [HtmlAttributeName("asp-fragment")]
    public string? AspFragment { get; set; }

    [HtmlAttributeName("asp-all-route-data")]
    public IDictionary<string, string>? AspAllRouteData { get; set; }

    [HtmlAttributeName(DictionaryAttributePrefix = "asp-route-")]
    public IDictionary<string, string> AspRouteValues { get; set; } =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    [HtmlAttributeNotBound]
    public IDictionary<string, string> AllRouteValues { get; private set; } =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    [HtmlAttributeNotBound]
    public string? Href { get; private set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        AllRouteValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        if (AspAllRouteData is not null)
        {
            foreach (var kvp in AspAllRouteData)
            {
                AllRouteValues[kvp.Key] = kvp.Value;
            }
        }

        if (AspRouteValues is not null)
        {
            foreach (var kvp in AspRouteValues)
            {
                AllRouteValues[kvp.Key] = kvp.Value;
            }
        }

        if (!string.IsNullOrEmpty(AspArea))
        {
            AllRouteValues["area"] = AspArea;
        }

        Href = BuildHref();

        await base.ProcessAsync(context, output);
    }

    private string? BuildHref()
    {
        if (Context is null)
        {
            return null;
        }

        var urlHelperFactory = Context.HttpContext.RequestServices.GetService<IUrlHelperFactory>();
        if (urlHelperFactory is null)
        {
            return null;
        }

        IUrlHelper url = urlHelperFactory.GetUrlHelper(Context);

        if (!string.IsNullOrEmpty(AspPage))
        {
            return url.Page(
                AspPage,
                AspPageHandler,
                AllRouteValues,
                AspProtocol,
                AspHost,
                AspFragment
            );
        }

        if (!string.IsNullOrEmpty(AspAction) || !string.IsNullOrEmpty(AspController))
        {
            return url.Action(
                AspAction,
                AspController,
                AllRouteValues,
                AspProtocol,
                AspHost,
                AspFragment
            );
        }

        if (!string.IsNullOrEmpty(AspRoute))
        {
            return url.RouteUrl(AspRoute, AllRouteValues, AspProtocol, AspHost, AspFragment);
        }

        return null;
    }
}
