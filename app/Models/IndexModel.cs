using SSD2600_CDEGP.Services;

namespace SSD2600_CDEGP.Models;

public class IndexModel(ElementService elementService)
{
    public List<Element> Elements { get; set; } = [];

    public List<string> Phases { get; set; } = [];

    public string? SortBy { get; set; }

    /// <summary>Product types currently selected for filtering, e.g. Medical, Industrial, Research.</summary>
    public List<string> SelectedTypes { get; set; } = [];

    /// <summary>Atomic numbers of elements that have a product matching the selected types (used to highlight tiles).</summary>
    public HashSet<int> ProductAtomicNumbers { get; set; } = [];

    public void ApplyFilters()
    {
        Elements = new List<Element>(elementService.Elements);

        if (Phases.Count > 0)
            Elements = Elements.ConvertAll(e =>
            {
                if (!Phases.Contains(e.Phase))
                {
                    e = e with { Category = "filtered" };
                }

                return e;
            });

        // If product-type filter is active, dim elements that have no matching product
        if (SelectedTypes.Count > 0 && ProductAtomicNumbers.Count > 0)
            Elements = Elements.ConvertAll(e =>
            {
                if (e.Category != "filtered" && !ProductAtomicNumbers.Contains(e.Number))
                {
                    e = e with { Category = "filtered" };
                }

                return e;
            });

        Elements = SortBy switch
        {
            "name" => Elements.OrderBy(e => e.Name).ToList(),
            "mass" => Elements.OrderBy(e => e.AtomicMass).ToList(),
            _ => Elements.OrderBy(e => e.Number).ToList(),
        };
    }
}
