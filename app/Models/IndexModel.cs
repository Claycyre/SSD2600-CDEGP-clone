using SSD2600_CDEGP.Services;

namespace SSD2600_CDEGP.Models;

public class IndexModel(ElementService elementService)
{
    public List<Element> Elements { get; set; } = [];

    public List<string> Phases { get; set; } = [];

    public string? SortBy { get; set; }

    public void ApplyFilters()
    {
        Elements = new List<Element>(elementService.Elements);

        if (Phases.Count > 0) Elements = Elements.Where(e => Phases.Contains(e.Phase)).ToList();

        Elements = SortBy switch
        {
            "name" => Elements.OrderBy(e => e.Name).ToList(),
            "mass" => Elements.OrderBy(e => e.AtomicMass).ToList(),
            _ => Elements.OrderBy(e => e.Number).ToList()
        };
    }
}
