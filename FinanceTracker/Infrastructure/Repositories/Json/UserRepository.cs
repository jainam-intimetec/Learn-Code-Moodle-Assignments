using Domain.Entities;
using Infrastructure.Repositories.Interfaces;
using Shared.Helpers;

namespace Infrastructure.Repositories.Json;

public class UserRepository : IUserRepository
{
    private readonly string _path = StoragePathHelper.GetDataFilePath("users.json");

    public async Task AddAsync(User user)
    {
        var users = await JsonStorageHelper.ReadAsync<User>(_path);
        users.Add(user);
        await JsonStorageHelper.WriteAsync(_path, users);
    }

    public async Task<User?> GetByIdAsync(Guid userId)
    {
        var users = await JsonStorageHelper.ReadAsync<User>(_path);
        return users.FirstOrDefault(u => u.Id == userId);
    }

    public async Task<User?> GetByUserNameAsync(string userName)
    {
        var users = await JsonStorageHelper.ReadAsync<User>(_path);

        return users.FirstOrDefault(u =>
            string.Equals(u.UserName, userName.Trim(), StringComparison.OrdinalIgnoreCase));
    }
}
