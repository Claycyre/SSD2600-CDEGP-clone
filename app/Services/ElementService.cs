using SSD2600_CDEGP.Models;
using System.Text.Json;

namespace SSD2600_CDEGP.Services;

public class ElementService
{
    public List<Element> Elements { get; private set; }

    public ElementService(IWebHostEnvironment env)
    {
        var jsonPath = Path.Combine(env.ContentRootPath, "Data/PeriodicTableJSON.json");
        var json = File.ReadAllText(jsonPath);
        
        // Deserialize to a wrapper object first
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var wrapper = JsonSerializer.Deserialize<ElementWrapper>(json, options);
        
        Elements = wrapper?.Elements ?? new List<Element>();
    }

    // Helper class to match the JSON structure
    private class ElementWrapper
    {
        public List<Element> Elements { get; set; } = new();
    }
}
