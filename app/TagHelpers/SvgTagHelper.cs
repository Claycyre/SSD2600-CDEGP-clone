using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SSD2600_CDEGP.TagHelpers;

[HtmlTargetElement("inline-svg", Attributes = "src")]
public class SvgTagHelper(IWebHostEnvironment env) : TagHelper
{
    private readonly IWebHostEnvironment _env = env;

    [HtmlAttributeName("src")]
    public required string Src { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        // Remove the <inline-svg> tag from the final output
        output.TagName = null;

        if (string.IsNullOrEmpty(Src))
            return;

        // Map the virtual path (e.g. ~/images/logo.svg) to a physical disk path
        var filePath = Path.Combine(_env.WebRootPath, Src.TrimStart('~', '/'));

        if (File.Exists(filePath))
        {
            var content = File.ReadAllText(filePath);
            output.Content.SetHtmlContent(content);
        }
    }
}
