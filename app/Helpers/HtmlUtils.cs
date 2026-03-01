namespace SSD2600_CDEGP.Helpers;

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
