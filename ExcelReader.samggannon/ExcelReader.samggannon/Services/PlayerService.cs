using ExcelReader.samggannon.Data;

namespace ExcelReader.samggannon.Services;

internal class PlayerService : IPlayerService
{
    private readonly PlayerContext _playerContextDb;

    public PlayerService(PlayerContext playerContextDb)
    {
        _playerContextDb = playerContextDb;
    }

    public async Task<bool> DeletePlayerDataDb()
    {
        using var db = _playerContextDb;
        return await db.Database.EnsureDeletedAsync();
    }

    public async Task<bool> CreatePlayerDataDb()
    {
        using var db = _playerContextDb;
        return await db.Database.EnsureCreatedAsync();
    }
}
