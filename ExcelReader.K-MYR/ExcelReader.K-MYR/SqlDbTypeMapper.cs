using System.Data;

namespace ExcelReader.K_MYR;

public static class SqlDbTypeMapper
{
    private readonly static Dictionary<Type, SqlDbType> _clrTypeToSqlTypeMaps = new()
    {
            {typeof (Boolean), SqlDbType.Bit},
            {typeof (Boolean?), SqlDbType.Bit},
            {typeof (Byte), SqlDbType.TinyInt},
            {typeof (Byte?), SqlDbType.TinyInt},
            {typeof (String), SqlDbType.NVarChar},
            {typeof (DateTime), SqlDbType.DateTime},
            {typeof (DateTime?), SqlDbType.DateTime},
            {typeof (Int16), SqlDbType.SmallInt},
            {typeof (Int16?), SqlDbType.SmallInt},
            {typeof (Int32), SqlDbType.Int},
            {typeof (Int32?), SqlDbType.Int},
            {typeof (Int64), SqlDbType.BigInt},
            {typeof (Int64?), SqlDbType.BigInt},
            {typeof (Decimal), SqlDbType.Decimal},
            {typeof (Decimal?), SqlDbType.Decimal},
            {typeof (Double), SqlDbType.Float},
            {typeof (Double?), SqlDbType.Float},
            {typeof (Single), SqlDbType.Real},
            {typeof (Single?), SqlDbType.Real},
            {typeof (TimeSpan), SqlDbType.Time},
            {typeof (Guid), SqlDbType.UniqueIdentifier},
            {typeof (Guid?), SqlDbType.UniqueIdentifier},
            {typeof (Byte[]), SqlDbType.Binary},
            {typeof (Byte?[]), SqlDbType.Binary},
            {typeof (Char[]), SqlDbType.Char},
            {typeof (Char?[]), SqlDbType.Char}
        };

    public static SqlDbType GetSqlDbType(Type clrType)
    {
        if (!_clrTypeToSqlTypeMaps.ContainsKey(clrType))
        {
            throw new ArgumentOutOfRangeException(nameof(clrType), @"No mapped type found for " + clrType);
        }

        _clrTypeToSqlTypeMaps.TryGetValue(clrType, out SqlDbType result);
        return result;
    }
}
