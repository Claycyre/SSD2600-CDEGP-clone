using SSD2600_CDEGP.Services;

namespace SSD2600_CDEGP.Models;

public class FilterModel(ElementService elementService)
{
    public List<Element> Elements { get; set; } = [];

    /// <summary>Atomic numbers of elements that have a product matching the selected types (used to highlight tiles).</summary>
    public HashSet<int> AvailableAtomicNumbers { get; set; } = [];

    public HashSet<string> AvailableApplications { get; set; } =
    ["Medical", "Industrial", "Research"];

    /// <summary>Product use-case/applications currently selected for filtering, e.g. Medical, Industrial, Research.</summary>
    public List<string> SelectedApplications { get; set; } = [];

    public HashSet<string> AvailablePhases { get; set; } = ["Solid", "Liquid", "Gas"];
    public List<string> SelectedPhases { get; set; } = [];

    public string? SortBy { get; set; }

    public void ApplyFilters()
    {
        Elements = [.. elementService.Elements];

        if (SelectedPhases.Count > 0)
            Elements = Elements.ConvertAll(e =>
            {
                if (!SelectedPhases.Contains(e.Phase))
                {
                    e = e with { Category = "filtered" };
                }

                return e;
            });

        // If product-type filter is active, dim elements that have no matching product
        if (SelectedApplications.Count > 0 && AvailableAtomicNumbers.Count > 0)
            Elements = Elements.ConvertAll(e =>
            {
                if (e.Category != "filtered" && !AvailableAtomicNumbers.Contains(e.Number))
                {
                    e = e with { Category = "filtered" };
                }

                return e;
            });

        Elements = SortBy switch
        {
            "name" => [.. Elements.OrderBy(e => e.Name)],
            "mass" => [.. Elements.OrderBy(e => e.AtomicMass)],
            _ => [.. Elements.OrderBy(e => e.Number)],
        };
    }
}
