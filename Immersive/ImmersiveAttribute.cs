using System;

[AttributeUsage(AttributeTargets.Assembly)]
public class ImmersiveAttribute : Attribute
{
    public ImmersiveAttribute(string library)
    {
        this.Library = library;
    }

    public string Library { get; }
}