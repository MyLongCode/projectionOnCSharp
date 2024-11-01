using Ddd.Taxi.Domain;
using System.Reflection;
using System.Text;

namespace Ddd.Taxi.Infrastructure;

public class ValueType<T>
{
    private readonly List<PropertyInfo> properties;

    public ValueType()
    {
        properties = this.GetType()
                          .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                          .OrderBy(p => p.Name)
                          .ToList();
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        foreach (var property in properties)
        {
            var thisValue = property.GetValue(this, null);
            var objectValue = property.GetValue(obj, null);
            if (thisValue == null & objectValue == null) continue;
            if (thisValue == null || objectValue == null || !thisValue.Equals(objectValue)) return false;
        }
        return true;
    }

    public override int GetHashCode()
    {
        int hash = 17;
        foreach (var property in properties)
            hash = (hash * 1248188) ^ property.GetValue(this, null).GetHashCode();
        return hash;
    }

    public bool Equals(PersonName name) => Equals((object)name);

    public override string ToString()
    {
        var result = new StringBuilder(this.GetType().Name + "(");
        int index = 0;
        foreach (var property in properties)
        {
            if (index != properties.Count - 1)
                result.AppendFormat("{0}: {1}; ", property.Name, property.GetValue(this, null));
            else
                result.AppendFormat("{0}: {1})", property.Name, property.GetValue(this, null));
            index++;
        }
        return result.ToString();
    }
}