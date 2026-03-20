using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;

namespace SSD2600_CDEGP.TagHelpers;

/// CREDIT: https://stackoverflow.com/a/73675946
/// <summary>
/// Implements a tag helper as a Razor view as the template
/// </summary>
/// <remarks>
///     uses convention that /TagHelpers/ has razor template based views for tags
///     For a folder /TagHelpers/Foo
///     * FooTagHelper.cs -> Defines the properties with HtmlAttribute on it (derived from ViewTagHelper)
///     * default.cshtml -> Defines the template with Model=>FooTagHelper
/// </remarks>
public class ViewTagHelper : TagHelper
{
    private string _viewPath;

    public ViewTagHelper()
    {
        _viewPath = $"~/TagHelpers/{GetType().Namespace.Split('.').Last()}/Default.cshtml";
    }

    [HtmlAttributeNotBound]
    [Microsoft.AspNetCore.Mvc.ViewFeatures.ViewContext]
    public ViewContext? Context { get; set; }

    public TagHelperContent? ChildContent { get; set; }

    [HtmlAttributeName]
    public string Classes { get; set; } = string.Empty;

    [HtmlAttributeName]
    public string Attributes { get; set; } = string.Empty;

    [HtmlAttributeName("tag")]
    public string TagName { get; set; } = "a";

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        throw new Exception("Use ProcessAsync()");
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (Context is null)
        {
            throw new ArgumentNullException(nameof(Context));
        }

        // get child content and capture it in our model so we can insert it in our output
        ChildContent = await output.GetChildContentAsync();

        IHtmlHelper? htmlHelper = Context.HttpContext.RequestServices.GetService<IHtmlHelper>();
        ArgumentNullException.ThrowIfNull(htmlHelper);

        (htmlHelper as IViewContextAware)!.Contextualize(Context);
        var content = await htmlHelper.PartialAsync(_viewPath, this);

        output.TagName = null;
        output.Content.SetHtmlContent(content);
    }
}
