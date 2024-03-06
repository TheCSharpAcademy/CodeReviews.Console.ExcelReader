using System.Data.Common;
using System.Dynamic;

namespace ExcelReader.Models;

public class ExcelModel() : DynamicObject
{
    Dictionary<string, object?> dictionary = [];
    public int Id {get; set;}
    public int Count
    {
        get
        {
            return dictionary.Count;
        }
    }
    public override bool TryGetMember( GetMemberBinder binder, out object? result)
    {
        string name = binder.Name.ToLower();
        return dictionary.TryGetValue(name, out result);
    }

    public override bool TrySetMember( SetMemberBinder binder, object? value)
    {
        dictionary[binder.Name.ToLower()] = value;
        return true;
    }

    public void SetMember(string name, object? value)
    {
        dictionary[name.ToLower()] = value;
    }
}