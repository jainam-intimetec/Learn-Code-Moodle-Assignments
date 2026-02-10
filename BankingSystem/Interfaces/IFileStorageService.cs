using BankingSystem.Models;

namespace BankingSystem.Interfaces;

public interface IFileStorageService
{
    List<User> LoadUsers();
    void SaveUsers(List<User> users);
}