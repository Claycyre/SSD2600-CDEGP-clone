using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;

namespace SSD2600_CDEGP.TagHelpers.PaButton;

[HtmlTargetElement("PaButton")]
public class PaButtonTagHelper : RoutingViewTagHelper
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
}
