namespace ExcelReader.samggannon.Services;

internal interface IPlayerService
{
    public Task<bool> DeletePlayerDataDb();
    public Task<bool> CreatePlayerDataDb();
}
