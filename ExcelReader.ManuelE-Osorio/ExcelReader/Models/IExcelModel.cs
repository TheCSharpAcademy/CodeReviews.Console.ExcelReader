using System.Dynamic;

namespace ExcelReader.Models;

public class IExcelModel : DynamicObject
{
    Dictionary<string, object?> dictionary = [];

    public int Count
    {
        get
        {
            return dictionary.Count;
        }
    }
    public override bool TryGetMember(
        GetMemberBinder binder, out object? result)
    {
        string name = binder.Name.ToLower();
    return dictionary.TryGetValue(name, out result);
    }
    public override bool TrySetMember(
        SetMemberBinder binder, object? value)
    {
        dictionary[binder.Name.ToLower()] = value;
        return true;
    }
}