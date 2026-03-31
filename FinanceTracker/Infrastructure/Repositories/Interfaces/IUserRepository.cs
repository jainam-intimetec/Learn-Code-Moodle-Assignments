using Domain.Entities;

namespace Infrastructure.Repositories.Interfaces;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User?> GetByIdAsync(Guid userId);
    Task<User?> GetByUserNameAsync(string userName);
}
