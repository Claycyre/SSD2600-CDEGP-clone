using System.ComponentModel.DataAnnotations;
using System.Reflection;
using SSD2600_CDEGP.Models;

namespace SSD2600_CDEGP.Helpers;

/// <summary>Extension methods for working with enum display names.</summary>
public static class EnumExtensions
{
    /// <summary>Returns the <see cref="DisplayAttribute.Name"/> of an enum value, falling back to <c>value.ToString()</c>.</summary>
    public static string GetDisplayName(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        if (field is null)
            return value.ToString();
        var attr = field.GetCustomAttribute<DisplayAttribute>();
        return attr?.Name ?? value.ToString();
    }

    /// <summary>
    /// Parses a <see cref="UserRole"/> string value and returns its human-readable display name.
    /// Falls back to the raw string if the value cannot be parsed.
    /// </summary>
    public static string GetUserRoleDisplayName(this string roleValue)
    {
        if (Enum.TryParse<UserRole>(roleValue, out var role))
            return role.GetDisplayName();
        return roleValue;
    }
}

/// <summary>Returns short badge abbreviations for product type and state-of-matter labels.</summary>
public static class BadgeUtils
{
    public static string ProductTypeAbbrev(string productType) =>
        productType switch
        {
            "Medical" => "MED",
            "Industrial" => "INDUST",
            "Research" => "RES",
            _ => productType,
        };

    public static string StateOfMatterAbbrev(string state) =>
        state switch
        {
            "Solid" => "SOL",
            "Liquid" => "LIQ",
            "Gas" => "GAS",
            _ => state,
        };
}

/// <summary>Formats a price with its ISO 4217 currency code, e.g. "CAD 8,500.00".</summary>
public static class PriceUtils
{
    public static string Format(double price, string currencyCode = "CAD")
    {
        return $"{currencyCode} {price:N2}";
    }

    public static string FormatPerGram(double price, string currencyCode = "CAD")
    {
        return $"{Format(price, currencyCode)}/g";
    }
}

public record CssClass
{
    private readonly bool condition;
    private readonly string classIfTrue = string.Empty;
    private readonly string classIfFalse = string.Empty;

    public CssClass(string baseClass)
        : this(true, baseClass, "") { }

    public static implicit operator CssClass(string baseClass)
    {
        return new CssClass(baseClass);
    }

    public CssClass(bool Condition, string IfTrue, string IfFalse)
    {
        condition = Condition;
        classIfTrue = IfTrue;
        classIfFalse = IfFalse;
    }

    public static implicit operator CssClass((bool condition, string ifTrue, string ifFalse) value)
    {
        return new CssClass(value.condition, value.ifTrue, value.ifFalse);
    }

    public override string ToString()
    {
        return condition ? classIfTrue : classIfFalse;
    }
}

public class CssClasses
{
    private readonly HashSet<string> classSet;

    public CssClasses()
    {
        classSet = [];
    }

    public CssClasses(IEnumerable<CssClass> cssClassConditionals)
    {
        classSet = [];
        foreach (var conditionalClass in cssClassConditionals)
        {
            Append(conditionalClass.ToString());
        }
    }

    public CssClasses(params CssClass[] cssClassConditionals)
        : this((IEnumerable<CssClass>)cssClassConditionals) { }

    private void NormalizedAppend(string cls)
    {
        classSet.Add(cls);
    }

    public override string ToString()
    {
        return string.Join(' ', classSet);
    }

    public void Append(string cls)
    {
        if (cls.Contains(' '))
        {
            foreach (var part in cls.Split(' '))
            {
                NormalizedAppend(part);
            }

            return;
        }

        NormalizedAppend(cls);
    }

    public void ConditionalAppend(bool condition, string toAppendIfTrue, string toAppendIfFalse)
    {
        if (condition)
        {
            Append(toAppendIfTrue);
        }
        else
        {
            Append(toAppendIfFalse);
        }
    }
}
