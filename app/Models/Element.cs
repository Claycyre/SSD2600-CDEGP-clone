using System.Text.Json.Serialization;

namespace SSD2600_CDEGP.Models;

public class Element
{
    public int Number { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("atomic_mass")] public double AtomicMass { get; set; }

    public string Category
    {
        //handles verbose fields in the source json doc
        get => field;
        set => field = value?.Split(',')[0].Trim().Replace(" ", "-").ToLower() ?? string.Empty;
    }

    [JsonPropertyName("xpos")] public int Column { get; set; }

    [JsonPropertyName("ypos")] public int Row { get; set; }

    public string Phase { get; set; } = string.Empty;
}
