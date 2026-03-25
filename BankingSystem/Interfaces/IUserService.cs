using BankingSystem.Models;

namespace BankingSystem.Interfaces;

public interface IUserService
{
    User Register(User user, string password);
    User Login(string username, string password);
}
