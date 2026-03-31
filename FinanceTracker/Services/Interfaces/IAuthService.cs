using Domain.Entities;

namespace App.Services.Interfaces;

public interface IAuthService
{
    Task<User> LoginAsync(string userName, string password);
    Task<User> SignUpAsync(string userName, string password);
}
